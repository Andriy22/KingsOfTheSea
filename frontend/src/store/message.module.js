export const message = {
  namespaced: true,
  state: {
    message: "",
    color: "error",
    showMessage: false,
  },
  actions: {
    displayMessage(context, data) {
      context.commit("SHOW_MESSAGE", data);
    },
  },
  getters: {},
  mutations: {
    SHOW_MESSAGE(state, data) {
      state.showMessage = !data.isHidden;
      state.message = data.text;
      state.color = data.color || "error";
    },
  },
};
