<template>
  <TheGameBoardRedactor :isOpen="isGameBoardRedactorOpen" @start-game="handleStartGame" />
  <TheGame
    v-if="!isGameBoardRedactorOpen"
    v-show="isGameStarted"
    ref="game"
    @round="handleRound"
  />
  <div v-show="!isGameStarted && !isGameBoardRedactorOpen">
    <v-col class="game-searching" cols="6">
      <div class="game-searching-info" style="color: rgb(43, 197, 87);">
        Пошук суперника...
        <v-btn color="red" icon="mdi-close" rounded variant="outlined" @click="cancelGame"></v-btn>

        <v-progress-linear
          color="green-darken-2"
          indeterminate
        ></v-progress-linear>
      </div>
    </v-col>
  </div>
</template>

<script>
import TheGameBoardRedactor from "../../components/GameBoardRedactor";
import TheGame from "@/components/TheGameRender";
import * as signalR from "@microsoft/signalr";
import { HttpTransportType } from "@microsoft/signalr";
import { API } from "@/config";

export default {
  name: "App",

  components: {
    TheGameBoardRedactor,
    TheGame
  },

  data: () => ({
    isGameMenuOpen: true,
    isGameBoardRedactorOpen: true,
    isGameStarted: false,
    gameMenuOptions: {
      resume: {
        isDisabled: true
      }
    },
    connection: null,
    playerConnectionId: null
  }),

  methods: {
    cancelGame() {
      this.isGameStarted = false;
      this.isGameBoardRedactorOpen = true;

      this.connection.invoke("CancelFindGame")
        .catch((err) => console.error(err));
    },

    hideGameMenu() {
      this.isGameMenuOpen = false;
    },

    handleShowGameMenu() {
      this.isGameMenuOpen = true;
    },

    openGameBoardRedactor() {
      this.isGameBoardRedactorOpen = true;
      this.isGameStarted = false;
    },

    closeGameBoardRedactor() {
      this.isGameBoardRedactorOpen = false;
    },

    handleNewGame() {
      this.gameMenuOptions.resume.isDisabled = false;
      this.isGameStarted = false;

      this.hideGameMenu();
      this.openGameBoardRedactor();
      this.$refs.game.resetTheGame();
    },

    handleResumeGame() {
      this.hideGameMenu();
    },

    handleStartGame(plBoard, plBoardElement, plShips) {
      this.closeGameBoardRedactor();
      // Connect to the SignalR hub
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API}live/battleship`, {
          transport: HttpTransportType.WebSockets,
          accessTokenFactory: () => {
            return this.$store.state.auth.user.access_token;
          }
        })
        .build();

      this.connection.start()
        .then(() => {
          // Send the player's ships to the server
          this.connection.invoke("FindGame", JSON.stringify(plShips))
            .catch((err) => {
              this.$refs.game.updateGameInfo("The game server is down:(  Гра не буде зарахована!", "rgb(226, 54, 54)");
              if (!this.isGameStarted && !this.isGameBoardRedactorOpen) {
                this.isGameBoardRedactorOpen = true;
              }
            });

          // Receive the game session details from the server
          this.connection.on("StartGame", (ownBoard, enemyBoard, gameInfo) => {
            this.isGameStarted = true;
            this.playerConnectionId = this.connection.connectionId;

            const ownInfo = {};
            const enemyInfo = {};

            if (gameInfo.player1ConnectionId === this.playerConnectionId) {
              ownInfo.nickname = gameInfo.player1Name;
              ownInfo.elo = `${gameInfo.player1Elo} (+${gameInfo.player1EloWinDelta} | ${gameInfo.player1EloLoseDelta})`;
            } else {
              ownInfo.nickname = gameInfo.player2Name;
              ownInfo.elo = `${gameInfo.player2Elo} (+${gameInfo.player2EloWinDelta} | ${gameInfo.player2EloLoseDelta})`;
            }

            if (gameInfo.player1ConnectionId !== this.playerConnectionId) {
              enemyInfo.nickname = gameInfo.player1Name;
              enemyInfo.elo = `${gameInfo.player1Elo}`;
            } else {
              enemyInfo.nickname = gameInfo.player2Name;
              enemyInfo.elo = `${gameInfo.player2Elo}`;
            }

            this.$refs.game.initTheGame(ownBoard, enemyBoard, ownInfo, enemyInfo);
          });

          // Receive the opponent's shot details from the server
          this.connection.on("boardsUpdated", (ownBoard, enemyBoard) => {
            // Update the player's game board to show the hit
            this.$refs.game.rerender(ownBoard, enemyBoard);
          });

          this.connection.on("turn", (connectionId) => {
            // show players turn
            if (connectionId === this.playerConnectionId) {
              this.$refs.game.updateGameInfo("Ваша черга атакувати!", "rgb(43, 197, 87)");
            } else {
              this.$refs.game.updateGameInfo("Суперник переходить в атаку!", "rgb(226, 54, 54)");
            }
          });

          this.connection.on("GameOver", (winnerId) => {
            if (winnerId === this.playerConnectionId) {
              // Notify the player that they won the game
              this.$refs.game.updateGameInfo("Вітаємо вас з перемогою у грі!", "rgb(43, 197,87)");
            } else {
              // Notify the player that they lost the game
              this.$refs.game.updateGameInfo("Суперник виграв гру!", "rgb(226, 54, 54)");
            }

            setTimeout(this.handleNewGame, 3000);
          });

          this.connection.on("OpponentDisconnected", () => {
            // Notify the player that their opponent has disconnected
            this.$refs.game.updateGameInfo("Ваш суперник відключився... Ви перемогли!", "rgb(226, 54, 54)");

            setTimeout(this.handleNewGame, 3000);
          });
        })
        .catch((err) => {
          this.$refs.game.updateGameInfo("Server crashed... Гра не буде зарахована!", "rgb(226, 54, 54)");
          if (!this.isGameStarted && !this.isGameBoardRedactorOpen) {
            this.isGameBoardRedactorOpen = true;
          }
        });
    },

    handleRound(pl2CordAttack) {
      const { x, y } = JSON.parse(pl2CordAttack);
      this.connection.invoke("FireShot", x, y)
        .catch((err) => this.$refs.game.updateGameInfo("Server crashed... Гра не буде зарахована!", "rgb(226, 54, 54)"));
    }
  },
  beforeUnmount() {
    // Disconnect from SignalR when the component is destroyed
    if (this.connection) {
      if (this.connection.state === signalR.HubConnectionState.Connected) {
        this.connection.stop().then(() => {
          console.log("SignalR disconnected");
        });
      }
    }

  },
  beforeDestroy() {
    // Disconnect from SignalR when the component is destroyed
    if (this.connection) {
      if (this.connection.state === signalR.HubConnectionState.Connected) {
        this.connection.stop().then(() => {
          console.log("SignalR disconnected");
        });
      }
    }
  }
};
</script>

<style scoped>
/*@font-face {*/
/*  font-family: bfont;*/
/*src: url('@/assets/04B_30__.TTF');*/
/*}*/

*,
*::before,
*::after {
  padding: 0;
  margin: 0;
  box-sizing: border-box;
  font-size: inherit;
  user-select: none !important;
}

html {
  font-size: 10px !important;
}

.game-searching-info {
  font-size: 3.0rem;
  align-self: center;
  text-align: center;
  text-shadow: 0 0 6px black;
  width: 100%;
}

.top-searching-bot-borders {
  padding: 1rem;
  margin: 1rem;
  border-top: 1px solid;
  border-bottom: 1px solid;
}

.pulse {
  animation: pulse 1s alternate infinite;
}


.game-searching {
  position: absolute;
  left: 50%;
  top: 48%;
  -webkit-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}

@media screen and (max-width: 860px) {
  html {
    font-size: 7px !important;
  }
}

@media screen and (max-width: 410px) {
  html {
    font-size: 6px !important;
  }
}

@media screen and (max-width: 359px) {
  html {
    font-size: 5px !important;
  }
}

@media screen and (max-width: 350px) {
  html {
    font-size: 4px !important;
  }
}
</style>

