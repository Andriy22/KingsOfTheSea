<template>
  <div class="game-wrapper">
    <div class="game-container">
      <div class="pl">
        <div class="board-container">
          <div class="cords letters">
            <div class="letter">A</div>
            <div class="letter">B</div>
            <div class="letter">C</div>
            <div class="letter">D</div>
            <div class="letter">E</div>
            <div class="letter">F</div>
            <div class="letter">G</div>
            <div class="letter">H</div>
            <div class="letter">I</div>
            <div class="letter">J</div>
          </div>
          <div class="cords numbers">
            <div class="number">1</div>
            <div class="number">2</div>
            <div class="number">3</div>
            <div class="number">4</div>
            <div class="number">5</div>
            <div class="number">6</div>
            <div class="number">7</div>
            <div class="number">8</div>
            <div class="number">9</div>
            <div class="number">10</div>
          </div>
          <div class="board">
            <div v-for="(row, rowIndex) in board" :key="rowIndex">
              <div
                v-for="(cell, cellIndex) in row"
                :key="cellIndex"
                :class="{
          ship: cell === 1 || cell === 3,
          hit: cell === 2,
          sunk: cell === 3,
          miss: cell === 4,

        }"
                class="spot"
              ></div>
            </div>
          </div>
        </div>
        <div class="board-info"><h3 class="name">{{ ownNickName }}</h3><h4 class="alive-ships">ELO:
          {{ this.ownELO }}</h4>
        </div>
      </div>
      <div class="game-info pulse top-bot-borders" style="color: rgb(43, 197, 87);">
        Ваш хід
      </div>
      <div class="pc">
        <div class="board-container">
          <div class="cords letters">
            <div class="letter">A</div>
            <div class="letter">B</div>
            <div class="letter">C</div>
            <div class="letter">D</div>
            <div class="letter">E</div>
            <div class="letter">F</div>
            <div class="letter">G</div>
            <div class="letter">H</div>
            <div class="letter">I</div>
            <div class="letter">J</div>
          </div>
          <div class="cords numbers">
            <div class="number">1</div>
            <div class="number">2</div>
            <div class="number">3</div>
            <div class="number">4</div>
            <div class="number">5</div>
            <div class="number">6</div>
            <div class="number">7</div>
            <div class="number">8</div>
            <div class="number">9</div>
            <div class="number">10</div>
          </div>
          <div class="board">
            <div v-for="(row, rowIndex) in enemyBoard" :key="rowIndex">
              <div
                v-for="(cell, cellIndex) in row"
                :key="cellIndex"
                :class="{
          ship: cell === 1 || cell === 3,
          hit: cell === 2,
          sunk: cell === 3,
          miss: cell === 4,
          points_event_none: cell !== 0
        }"
                class="spot"
                @click="hit(rowIndex, cellIndex, cell)"
              ></div>
            </div>
          </div>
        </div>
        <div class="board-info"><h3 class="name">{{ enemyNickName }}</h3><h4 class="alive-ships">ELO:
          {{ enemyELO }}</h4></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TheGame",
  data() {
    return {
      gameInfo: null,
      board: [],
      enemyBoard: [],
      enemyNickName: "Player1",
      enemyELO: "1000 | +-30",
      ownNickName: "Player2",
      ownELO: "1000 | +-30"
    };
  },
  methods: {
    initTheGame(board, enemyBoard, ownInfo, enemyInfo) {
      this.gameInfo = document.querySelector(".game-info");
      this.board = board;
      this.enemyBoard = enemyBoard;

      this.ownELO = ownInfo.elo;
      this.ownNickName = ownInfo.nickname;

      this.enemyNickName = enemyInfo.nickname;
      this.enemyELO = enemyInfo.elo;
    },

    rerender(board, enemyBoard) {
      this.board = board;
      this.enemyBoard = enemyBoard;
    },

    updateGameInfo(msg = "", color = "rgb(43, 197, 87)") {
      this.gameInfo.textContent = msg;
      this.gameInfo.style.color = color;

      if (!this.gameInfo.classList.contains("pulse")) {
        this.gameInfo.classList.add("pulse");
        this.gameInfo.classList.add("top-bot-borders");
      }

      if (!msg) {
        this.gameInfo.classList.remove("pulse");
        this.gameInfo.classList.remove("top-bot-borders");
      }
    },
    hit(x, y, cell) {
      if (cell !== 0) return;
      this.$emit("round", JSON.stringify({ x: x, y: y }));
    },

    resetTheGame() {
      this.gameInfo = null;
      this.board = [];
      this.enemyBoard = [];
      this.enemyNickName = "Player1";
      this.enemyELO = "1000 | +-30";
      this.ownNickName = "Player2";
      this.ownELO = "1000 | +-30";
    }
  }
};
</script>

<style scoped>

.points_event_none {
  pointer-events: none;
}

.spot {
  width: var(--spot-size);
  height: var(--spot-size);
  background-color: #1e90ff;
  opacity: 0.8;
  box-sizing: border-box;
  display: inline-block;
}

.ship {
  background-color: #8b0000;
  position: relative;
}

.hit:before {
  content: 'x';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.hit {
  background-color: #ffa500;
}

.miss:before {
  content: '-';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.miss {
  background-color: gray;
}

.sunk {
  background-color: #262727;
}

.sunk:before {
  content: 'x';
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.game-container {
  font-family: bfont;
  --spot-size: 3rem;
  padding: 4rem;
  display: flex;

  font-size: 10px !important;
  justify-content: space-around;
}

.ship-column {
  grid-auto-flow: column;
  position: absolute;
}

.ship-row {
  grid-auto-flow: row;
  position: absolute;
}

.pl, .pc {
  display: flex;
  flex-direction: column;
}

.pl >>> .ship,
.pc >>> .ship {
  cursor: initial;
}

.pc >>> .spot,
.pl >>> .spot {
  text-shadow: 0 0 2px black;
  text-align: center;
  font-size: var(--spot-size);
  border-width: 10px;
}

.pc >>> .spot {
  cursor: crosshair;

}

.pc >>> .spot:hover {
  background-color: rgb(98, 0, 255);
}

.pc >>> .spot .ship {
  display: grid;
  grid-auto-flow: column;
  background-color: rgb(187, 82, 82);
  grid-gap: 4px;
  z-index: 69;
}

.pc >>> .spot .part {
  width: var(--spot-size);
  height: var(--spot-size);
  background-color: rgb(218, 100, 100);
  border: 4px solid rgb(70, 70, 70);
}

.pc >>> .spot .part,
.pl >>> .spot .part {
  text-shadow: 0 0 2px black;
  text-align: center;
  font-size: var(--spot-size);
  line-height: 0.745;
}

.pc >>> .board-info,
.pl >>> .board-info {
  text-shadow: 0 0 2px black;
  text-align: center;
  padding-left: var(--spot-size);
}

.pl >>> .board-info {
  color: rgb(43, 197, 87);
}

.pc >>> .board-info {
  color: rgb(226, 54, 54);
}

.pc >>> .board-info .name,
.pl >>> .board-info .name {
  border-bottom: 1px solid;
  font-size: 2.6rem;
}

.pc >>> .board-info .alive-ships,
.pl >>> .board-info .alive-ships {
  font-size: 2.4rem;
}

.game-info {
  font-size: 3.0rem;
  align-self: center;
  text-align: center;
  text-shadow: 0 0 6px black;
  width: 100%;
}

.top-bot-borders {
  padding: 1rem;
  margin: 1rem;
  border-top: 1px solid;
  border-bottom: 1px solid;
}

.pulse {
  animation: pulse 500ms alternate infinite;
}

.pc >>> .resize,
.pl >>> .resize {
  animation: resize 200ms 1 reverse;
}

html {
  font-size: 10px !important;
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

@keyframes pulse {
  100% {
    box-shadow: inset 0 0 2px 1px rgb(82, 82, 82),
    0 0 2px 1px rgb(255, 255, 255);
    color: rgb(255, 255, 255);
  }
}

@keyframes resize {
  100% {
    transform: scale(1.16);
    background-color: rgb(226, 54, 54);;
  }
}

@media screen and (max-width: 1400px) {
  .pc >>> .board-info .alive-ships,
  .pl >>> .board-info .alive-ships {
    font-size: 1.8rem;
  }

  .game-info {
    font-size: 2.2rem;
    padding: 0.4rem;
  }

  .pc >>> .board-info .name,
  .pl >>> .board-info .name {
    font-size: 2rem;
  }
}

@media screen and (max-width: 1280px) {
  .game-container {
    padding: 1rem;
    display: grid;
    grid-auto-columns: auto auto;
    grid-auto-rows: auto auto;
  }

  .game-info {
    grid-column: 1 / 3;
    order: -1;
  }

}

@media screen and (max-width: 1000px) {
  .pc >>> .spot .ship {
    grid-gap: 2px;
  }

  .pc >>> .spot .part,
  .pl >>> .spot .part {
    line-height: 0.7;
  }

  .game-info {
    order: 0;
  }

  .game-container {
    justify-content: center;
  }
}

@media screen and (max-width: 820px) {
  .game-container {
    padding: 2rem;
    display: flex;
    flex-direction: column;
    align-items: center;
  }
}

@media screen and (max-width: 359px) {
  .pc >>> .spot .ship {
    grid-gap: 1px;
  }

  .pc >>> .spot .part {
    border-width: 1px;
  }

  .pc >>> .spot .part,
  .pl >>> .spot .part {
    line-height: 0.9;
  }
}


</style>


