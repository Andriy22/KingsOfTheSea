<template>
  <v-app id="inspire">
    <v-system-bar>
      <v-spacer></v-spacer>

      <v-icon>mdi-square</v-icon>

      <v-icon>mdi-circle</v-icon>

      <v-icon>mdi-triangle</v-icon>
    </v-system-bar>

    <v-app-bar>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>Kings of the Sea</v-toolbar-title>
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
        <router-link class="text-decoration-none color-inherit" to="/login">
          <v-list-item
            prepend-icon="mdi-lock"
            title="Авторизація"
            value="login"
          ></v-list-item>
        </router-link>
        <router-link class="text-decoration-none color-inherit" to="/register">
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
  </v-app>
</template>

<script>
export default {
  data: () => ({ drawer: null }),
};
</script>

<style lang="css">
.color-inherit {
  color: inherit !important;
}
</style>
