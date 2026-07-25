import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      name: 'LogIn',
      path: '/log-in',
      component: async () => await import('@/views/authentication/LogIn.vue'),
    },
    {
      name: 'Home',
      path: '/',
      component: async () => await import('@/views/home/HomePage.vue'),
    },
  ],
});

export default router;
