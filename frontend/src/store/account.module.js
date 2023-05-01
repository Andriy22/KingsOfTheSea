import accountService from "@/services/account.service";
import Router from "@/router";

export const account = {
  namespaced: true,
  state: {
    requests: {
      createUser: {
        isLoading: false,
        isError: false,
      },
      stats: {
        isLoading: false,
        data: {},
      },
      top: {
        isLoading: false,
        data: {},
      },
    },
  },
  actions: {
    getTopPlayers({ commit }) {
      commit("SET_LOADING", { request: "top", isLoading: true });

      accountService.getTopPlayers().then(
        (data) => {
          commit("SET_LOADING", { request: "top", isLoading: false });
          commit("SET_DATA", { request: "top", data: data });
        },
        () => {
          commit("SET_LOADING", { request: "top", isLoading: false });
          commit("SET_DATA", { request: "top", data: {} });
        }
      );
    },
    getStats({ commit }) {
      commit("SET_LOADING", { request: "stats", isLoading: true });

      accountService.getStats().then(
        (data) => {
          commit("SET_LOADING", { request: "stats", isLoading: false });
          commit("SET_DATA", { request: "stats", data: data });
        },
        () => {
          commit("SET_LOADING", { request: "stats", isLoading: false });
          commit("SET_DATA", { request: "stats", data: {} });
        }
      );
    },
    createUser({ commit, dispatch }, data) {
      commit("SET_LOADING", { request: "createUser", isLoading: true });
      commit("SET_ERROR", { request: "createUser", isError: false });

      accountService
        .createAccount(
          data.email,
          data.nickname,
          data.password,
          data.passwordConfirmation
        )
        .then(
          () => {
            commit("SET_LOADING", { request: "createUser", isLoading: false });
            commit("SET_ERROR", { request: "createUser", isError: false });

            Router.push("/login").then(() => {});

            dispatch(
              "message/displayMessage",
              {
                isHidden: false,
                text: "Ваш аккаунт створено! Будь ласка, авторизуйтесь.",
                color: "success",
              },
              { root: true }
            );
          },
          () => {
            commit("SET_LOADING", { request: "createUser", isLoading: false });
            commit("SET_ERROR", { request: "createUser", isError: true });
          }
        );
    },
  },
  getters: {},
  mutations: {
    SET_LOADING(state, { request, isLoading }) {
      state.requests[request].isLoading = isLoading;
    },
    SET_ERROR(state, { request, isError }) {
      state.requests[request].isError = isError;
    },
    SET_DATA(state, { request, data }) {
      state.requests[request].data = data;
    },
  },
};
