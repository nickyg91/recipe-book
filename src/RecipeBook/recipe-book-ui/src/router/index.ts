import { createRouter, createWebHistory } from 'vue-router';
import { registerAuthenticatedGuard } from './guards/authentication-guard';
import { RouteNames } from './route-names';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: RouteNames.LogIn,
      path: '/log-in',
      component: () => import('@/views/authentication/LogIn.vue'),
    },
    {
      name: RouteNames.Recipes,
      path: '/recipes',
      component: () => import('@/views/recipes/RecipesPage.vue'),
      meta: { requiresAuth: true },
    },
    {
      name: RouteNames.ConfirmAccount,
      path: '/confirm-account/:emailConfirmationToken',
      component: () => import('@/views/authentication/ConfirmAccount.vue'),
      props: true,
    },
    {
      name: RouteNames.ForgotPassword,
      path: '/forgot-password',
      component: () => import('@/views/authentication/ForgotPassword.vue'),
    },
    {
      name: RouteNames.ResetPassword,
      path: '/reset-password/:token',
      component: () => import('@/views/authentication/ResetPassword.vue'),
      props: true,
    },
    {
      path: '/',
      redirect: { name: RouteNames.Recipes },
    },
  ],
});

registerAuthenticatedGuard(router);

export default router;
