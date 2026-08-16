import { authenticate, getCurrentUser } from '@/core/api/user.api';
import type { IJwt } from '@/core/models/IJwtToken';
import type { ILogInRequest } from '@/core/models/ILogInRequest';
import type { IUser } from '@/core/models/IUser';
import { defineStore } from 'pinia';
import { computed, readonly, ref } from 'vue';

export const useUserStore = defineStore('userStore', () => {
  const token = ref<IJwt | undefined>();
  const currentlyLoggedInUser = ref<IUser | undefined>();

  const logIn = async (request: ILogInRequest, abortSignal?: AbortSignal) => {
    const jwt = await authenticate(request, abortSignal);
    localStorage.setItem('access_token', jwt.token);
    localStorage.setItem('refresh_token', jwt.refreshToken);
    token.value = jwt;
    currentlyLoggedInUser.value = await getCurrentUser(abortSignal);
  };

  const checkAuth = async () => {
    const accessToken = localStorage.getItem('access_token');
    const refreshToken = localStorage.getItem('refresh_token');

    if (accessToken && refreshToken) {
      token.value = { token: accessToken, refreshToken };
      currentlyLoggedInUser.value = await getCurrentUser();
    }
  };

  const isLoggedIn = computed(() => {
    return currentlyLoggedInUser.value !== undefined;
  });

  return {
    token: readonly(token),
    currentlyLoggedInUser: readonly(currentlyLoggedInUser),
    logIn,
    checkAuth,
    isLoggedIn,
  };
});
