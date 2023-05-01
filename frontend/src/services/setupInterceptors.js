import axiosInstance from "./api";
import TokenService from "./token.service";
import Router from "@/router";

const setup = (store) => {
  axiosInstance.interceptors.request.use(
    (config) => {
      const token = TokenService.getLocalAccessToken();
      if (token) {
        config.headers["Authorization"] = "Bearer " + token;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  axiosInstance.interceptors.response.use(
    (res) => {
      return res;
    },
    async (err) => {
      const originalConfig = err.config;

      if (err.response.status !== 401) {
        if (err.response.data?.error) {
          store.dispatch("message/displayMessage", {
            isHidden: false,
            text: err.response.data?.error,
          });
        } else {
          if (err.response.data?.errors) {
            store.dispatch("message/displayMessage", {
              isHidden: false,
              text: Object.values(err.response.data?.errors)[0][0],
            });
          }
        }
      }

      if (originalConfig.url !== "/auth/authorize" && err.response) {
        // Access Token was expired

        if (err.response.status === 401 && !originalConfig._retry) {
          originalConfig._retry = true;

          try {
            const rs = await axiosInstance.post("/auth/refresh", {
              accessToken: TokenService.getLocalAccessToken(),
              refreshToken: TokenService.getLocalRefreshToken(),
            });

            const user = rs.data;
            store.dispatch("auth/refreshToken", user);
            TokenService.updateLocalAccessToken(user.accessToken);
            TokenService.updateLocalRefreshToken(user.refreshToken);

            return axiosInstance(originalConfig);
          } catch (_error) {
            store.dispatch("message/displayMessage", {
              isHidden: false,
              text: "Ви не авторизовані...",
            });
            await Router.push("/login");

            return Promise.reject(_error);
          }
        }
      }

      return Promise.reject(err);
    }
  );
};

export default setup;
