<template>
  <v-app id="inspire">
    <div class="background-image-fixed">
      <v-app-bar>
        <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

        <v-toolbar-title>Kings of the Sea</v-toolbar-title>

        <v-spacer></v-spacer>
        <v-menu
          v-if="$store.state.auth.status.loggedIn"
          :offset-y="true"
          location="bottom"
          transition="slide-y-transition"
        >
          <template v-slot:activator="{ props }">
            <v-btn outlined v-bind="props">
              {{ $store.state.auth.user.userName }}
            </v-btn>
          </template>
          <v-list>
            <v-list-item class="cursor-pointer">
              <router-link class="text-decoration-none color-inherit" to="/">
                <v-list-item-title>Налаштування</v-list-item-title>
              </router-link>
            </v-list-item>
            <router-link class="text-decoration-none color-inherit" to="/login">
              <v-list-item class="cursor-pointer">
                <v-list-item-title>Вийти</v-list-item-title>
              </v-list-item>
            </router-link>
          </v-list>
        </v-menu>
      </v-app-bar>

      <v-navigation-drawer v-model="drawer">
        <v-list nav>
          <router-link class="text-decoration-none color-inherit" to="/">
            <v-list-item
              prepend-icon="mdi-view-dashboard"
              title="Головна"
              value="home"
            ></v-list-item>
          </router-link>

          <router-link
            v-if="$store.state.auth.status.loggedIn"
            class="text-decoration-none color-inherit"
            to="/game"
          >
            <v-list-item
              prepend-icon="mdi-play"
              title="Грати"
              value="play"
            ></v-list-item>
          </router-link>
          <router-link
            v-if="$store.state.auth.status.loggedIn"
            class="text-decoration-none color-inherit"
            to="/profile"
          >
            <v-list-item
              prepend-icon="mdi-account"
              title="Профіль"
              value="profile"
            ></v-list-item>
          </router-link>

          <router-link
            v-if="$store.state.auth.status.loggedIn"
            class="text-decoration-none color-inherit"
            to="/leaderboard"
          >
            <v-list-item
              prepend-icon="mdi-crown"
              title="Таблиця лідерів"
              value="leaderboard"
            ></v-list-item>
          </router-link>
          <router-link
            v-if="!$store.state.auth.status.loggedIn"
            class="text-decoration-none color-inherit"
            to="/login"
          >
            <v-list-item
              prepend-icon="mdi-lock"
              title="Авторизація"
              value="login"
            ></v-list-item>
          </router-link>
          <router-link
            v-if="!$store.state.auth.status.loggedIn"
            class="text-decoration-none color-inherit"
            to="/register"
          >
            <v-list-item
              prepend-icon="mdi-account-star"
              title="Реєстрація"
              value="register"
            ></v-list-item>
          </router-link>
        </v-list>
      </v-navigation-drawer>

      <v-main>
        <router-view></router-view>
      </v-main>

      <v-snackbar
        v-model="$store.state.message.showMessage"
        :color="$store.state.message.color"
        :timeout="5000"
        variant="outlined"
      >
        {{ $store.state.message.message }}
      </v-snackbar>
    </div>
  </v-app>
</template>

<script>
export default {
  data: () => ({ drawer: null })
};
</script>

<style lang="css">
*,
*::before,
*::after {
  padding: 0;
  margin: 0;
  box-sizing: border-box;
  font-size: inherit;
  user-select: none !important;
  font-family: 'Roboto', sans-serif;
}


.color-inherit {
  color: inherit !important;
}

.background-image-fixed {
  min-height: 100vh;
  opacity: 1;
  background-attachment: fixed;
  background-image: url("./assets/battle_ship_background.jpg");
}

.refresh {
  animation: rotation 2s infinite linear reverse;
}

@keyframes rotation {
  from {
    -webkit-transform: rotate(0deg);
  }
  to {
    -webkit-transform: rotate(359deg);
  }
}
</style>
