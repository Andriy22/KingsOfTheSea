import { createStore } from "vuex";
import { auth } from "@/store/auth.module";
import { message } from "@/store/message.module";
import { account } from "@/store/account.module";

export default createStore({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    auth,
    message,
    account,
  },
});
