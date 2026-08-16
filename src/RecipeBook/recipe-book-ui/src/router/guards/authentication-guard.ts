import { useUserStore } from '@/stores/userStore';
import type { Router } from 'vue-router';

export const registerAuthenticatedGuard = (router: Router) => {
  router.beforeEach((to) => {
    const userStore = useUserStore();

    if (to.meta.requiresAuth) {
      if (!userStore.token) {
        return '/log-in';
      }
    }
    return true;
  });
};
