import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LoginView from "@/views/auth/LoginView";
import RegistrationView from "@/views/auth/RegistrationView";
import GameView from "@/views/game/GameView";
import store from "@/store";
import ProfileView from "@/views/ProfileView";
import TopPlayerView from "@/views/TopPlayerView";

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/login",
    name: "login",
    component: LoginView,
  },
  {
    path: "/register",
    name: "registration",
    component: RegistrationView,
  },
  {
    path: "/game",
    name: "game",
    component: GameView,
  },
  {
    path: "/profile",
    name: "profile",
    component: ProfileView,
  },
  {
    path: "/leaderboard",
    name: "leaderboard",
    component: TopPlayerView,
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const authRoutes = ["game", "leaderboard", "profile"];
  if (authRoutes.includes(to.name) && !store.state.auth.status.loggedIn) {
    next({ path: "/login", replace: true });
  } else {
    next();
  }
});

export default router;
