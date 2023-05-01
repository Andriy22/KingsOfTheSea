<template>
  <v-container class="align-center justify-center fill-height">
    <v-card
      class="auth-card"
      max-width="800"
      min-width="250"
      variant="outlined"
      width="700"
    >
      <v-card-title class="headline text-center">Реєстрація</v-card-title>
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
              v-model="nickname"
              :rules="nicknameRules"
              density="comfortable"
              label="Введіть nickname"
              prepend-icon="mdi-account-star"
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

          <v-col class="mb-0 mt-0" cols="10" offset="1">
            <v-text-field
              v-model="confirmationPassword"
              :rules="confirmPasswordRules"
              density="comfortable"
              label="Підтвердіть пароль"
              prepend-icon="mdi-lock"
              type="password"
              variant="outlined"
            ></v-text-field>
          </v-col>
          <v-col cols="10" offset="1">
            <v-btn
              :disabled="!isValid"
              :loading="$store.state.account.requests.createUser.isLoading"
              @click="onRegister"
              >Зареєструватися
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
    email: "",
    password: "",
    confirmationPassword: "",
    isValid: false,
    nickname: "",
    passRules: [(value) => !!value || "Поле обов'язкове."],
    confirmPasswordRules: [(v) => !!v || "Поле обов'язкове."],
    nicknameRules: [
      (v) => !!v || "Поле обов'язкове.",
      (v) =>
        (v && v.length > 2) ||
        "Довжина нікнейму повинна бути більше 2 символів",
    ],
    emailRules: [
      (value) => !!value || "Поле обов'язкове.",
      (value) => {
        const pattern =
          /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return pattern.test(value) || "Неправильний емейл.";
      },
    ],
  }),
  methods: {
    onRegister() {
      this.$store.dispatch("account/createUser", {
        email: this.email,
        nickname: this.nickname,
        password: this.password,
        passwordConfirmation: this.confirmationPassword,
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
