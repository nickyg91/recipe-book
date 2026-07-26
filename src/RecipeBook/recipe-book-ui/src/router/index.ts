import { createRouter, createWebHistory } from 'vue-router';
import { registerAuthenticatedGuard } from './guards/authentication-guard';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: 'LogIn',
      path: '/log-in',
      component: () => import('@/views/authentication/LogIn.vue'),
    },
    {
      name: 'Home',
      path: '/',
      component: () => import('@/views/home/HomePage.vue'),
      meta: { requiresAuth: true },
    },
  ],
});

registerAuthenticatedGuard(router);

export default router;
