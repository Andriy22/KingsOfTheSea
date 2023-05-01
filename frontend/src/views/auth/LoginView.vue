<template>
  <v-container class="align-center justify-center fill-height">
    <v-card
      class="auth-card"
      max-width="800"
      min-width="250"
      variant="outlined"
      width="700"
    >
      <v-card-title class="headline text-center">Авторизація</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form v-model="isValid">
          <v-col class="mb-0 mt-0" cols="10" offset="1">
            <v-text-field
              v-model="email"
              :rules="emailRules"
              density="comfortable"
              label="Введіть Email"
              prepend-icon="mdi-email-outline"
              variant="outlined"
            ></v-text-field>
          </v-col>
          <v-col class="mb-0 mt-0" cols="10" offset="1">
            <v-text-field
              v-model="password"
              :rules="passRules"
              density="comfortable"
              label="Введіть пароль"
              prepend-icon="mdi-lock"
              type="password"
              variant="outlined"
            ></v-text-field>
          </v-col>
          <v-col cols="10" offset="1">
            <v-btn
              :disabled="!isValid"
              :loading="$store.state.auth.status.isLoading"
              width="20%"
              @click="onLogin"
              >Ввійти
            </v-btn>
          </v-col>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>
<script>
export default {
  data: () => ({
    isLoading: false,
    passRules: [(value) => !!value || "Поле обов'язкове."],
    password: "",
    email: "",
    isValid: false,
    emailRules: [
      (value) => !!value || "Поле обов'язкове.",
      (value) => {
        const pattern =
          /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return pattern.test(value) || "Неправильний емейл.";
      },
    ],
  }),
  mounted() {
    this.$store.dispatch("auth/logout");
  },
  methods: {
    onLogin() {
      this.isLoading = true;
      this.$store.dispatch("auth/login", {
        email: this.email,
        password: this.password,
      });
    },
  },
};
</script>

<style scoped>
.auth-card {
  margin-bottom: 20rem;
}
</style>
