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
    {
      name: 'ConfirmAccount',
      path: '/confirm-account/:emailConfirmationToken',
      component: () => import('@/views/authentication/ConfirmAccount.vue'),
      props: true,
    },
    {
      name: 'ForgotPassword',
      path: '/forgot-password',
      component: () => import('@/views/authentication/ForgotPassword.vue'),
    },
    {
      name: 'ResetPassword',
      path: '/reset-password/:token',
      component: () => import('@/views/authentication/ResetPassword.vue'),
      props: true,
    },
  ],
});

registerAuthenticatedGuard(router);

export default router;
