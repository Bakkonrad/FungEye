import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import LogInView from "../views/LogInView.vue";
import RegisterView from "@/views/RegisterView.vue";
import AtlasView from "../views/AtlasView.vue";
import MyProfileView from "@/views/MyProfileView.vue";
import PortalView from "@/views/PortalView.vue";
import RecognizeView from "@/views/RecognizeView.vue";
import WeatherView from "@/views/WeatherView.vue";
import MushroomView from "@/views/MushroomView.vue";
import AdminView from "@/views/AdminView.vue";
import UserPostsView from "@/views/UserPostsView.vue";
import UserProfile from "@/views/UserProfileView.vue";
import UserProfileAdminView from "@/views/UserProfile-AdminView.vue";
import ForgotPassword from "@/views/ForgotPasswordView.vue";
import ResetPasswordView from "@/views/ResetPasswordView.vue";
import PostView from "@/views/PostView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/log-in",
      name: "logIn",
      component: LogInView,
    },
    {
      path: "/register",
      name: "register",
      component: RegisterView,
      props: { admin: false },
    },
    {
      path: "/atlas",
      name: "atlas",
      component: AtlasView,
    },
    {
      path: "/my-profile",
      name: "myProfile",
      component: MyProfileView,
    },
    {
      path: "/portal",
      name: "portal",
      component: PortalView,
      props: { defaultTab: "all-posts" },
    },
    {
      path: "/portal/all-posts",
      name: "allPosts",
      component: PortalView,
      props: { defaultTab: "all-posts" },
    },
    {
      path: "/portal/followed-posts",
      name: "followedPosts",
      component: PortalView,
      props: { defaultTab: "followed-posts" },
    },
    {
      path: "/portal/search-users",
      name: "searchUsers",
      component: PortalView,
      props: route => ({ defaultTab: "search", query: route.query.q || "" }),
    },
    {
      path: "/recognize",
      name: "recognize",
      component: RecognizeView,
    },
    {
      path: "/weather",
      name: "weather",
      component: WeatherView,
    },
    {
      path: "/mushroom/:id",
      name: "MushroomView",
      component: MushroomView,
      props: true,
    },
    {
      path: "/admin",
      name: "admin",
      component: AdminView,
    },
    {
      path: "/user-posts/:id",
      name: "UserPosts",
      component: UserPostsView,
      props: true,
    },
    {
      path: "/user-profile/:id",
      name: "userProfile",
      component: UserProfile,
      props: true,
    },
    {
      path: "/admin/user-profile/:id",
      name: "UserProfileAdmin",
      component: UserProfileAdminView,
      props: true,
    },
    {
      path: "/forgot-password",
      name: "forgotPassword",
      component: ForgotPassword,
    },
    {
      path: "/register/admin",
      name: "registerAdmin",
      component: RegisterView,
      props: { admin: true },
    },
    {
      path: "/resetPassword",
      name: "resetPassword",
      component: ResetPasswordView,
      props: (route) => ({ token: route.query.token }),
    },
    {
      path: '/post/:id',
      name: 'post',
      component: PostView
    }
  ]
})

export default router;
