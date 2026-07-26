import { useUserStore } from '@/stores/userStore';
import type { Router } from 'vue-router';

export const registerAuthenticatedGuard = (router: Router) => {
  router.beforeEach((to) => {
    const userStore = useUserStore();

    // Only run guard for routes that require auth
    if (to.meta.requiresAuth) {
      if (!userStore.token) {
        return '/log-in'; // redirect unauthenticated users
      }
    }
    return true; // allow navigation for all other routes
  });
};
