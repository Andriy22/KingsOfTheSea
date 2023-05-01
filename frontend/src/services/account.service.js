import api from "@/services/api";

class AccountService {
  createAccount(email, userName, password, passwordConfirmation) {
    return api
      .post("account/create-account", {
        email,
        userName,
        password,
        passwordConfirmation,
      })
      .then((response) => response.data);
  }

  getStats() {
    return api
      .get("account/get-profile-stats")
      .then((response) => response.data);
  }

  getTopPlayers() {
    return api.get("account/get-top-players").then((response) => response.data);
  }
}

export default new AccountService();
