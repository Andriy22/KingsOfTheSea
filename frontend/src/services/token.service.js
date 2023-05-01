class TokenService {
  getLocalRefreshToken() {
    const user = JSON.parse(localStorage.getItem("auth"));
    return user?.refresh_token;
  }

  getLocalAccessToken() {
    const user = JSON.parse(localStorage.getItem("auth"));
    return user?.access_token;
  }

  updateLocalAccessToken(token) {
    const user = JSON.parse(localStorage.getItem("auth"));
    user.access_token = token;
    localStorage.setItem("auth", JSON.stringify(user));
  }

  updateLocalRefreshToken(token) {
    const user = JSON.parse(localStorage.getItem("auth"));
    user.refresh_token = token;
    localStorage.setItem("auth", JSON.stringify(user));
  }

  getUser() {
    return JSON.parse(localStorage.getItem("auth"));
  }

  setUser(user) {
    localStorage.setItem("auth", JSON.stringify(user));
  }

  removeUser() {
    localStorage.removeItem("auth");
  }
}

export default new TokenService();
