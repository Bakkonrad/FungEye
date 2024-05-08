import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LogInView from '../views/LogInView.vue'
import AtlasView from '../views/AtlasView.vue'
import MyProfileView from '@/views/MyProfileView.vue'
import PortalView from '@/views/PortalView.vue'
import RecognizeView from '@/views/RecognizeView.vue'
import WeatherView from '@/views/WeatherView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/log-in',
      name: 'logIn',
      component: LogInView
    },
    {
      path: '/atlas',
      name: 'atlas',
      component: AtlasView
    },
    {
      path: '/my-profile',
      name: 'myProfile',
      component: MyProfileView
    },
    {
      path: '/portal',
      name: 'portal',
      component: PortalView
    },
    {
      path: '/recognize',
      name: 'recognize',
      component: RecognizeView
    },
    {
      path: '/weather',
      name: 'weather',
      component: WeatherView
    }
  ]
})

export default router
