import { useHttpClient } from '../http-client';
import type { IJwt } from '../models/IJwtToken';
import type { ILogInRequest } from '../models/ILogInRequest';
import type { ISignUpRequest } from '../models/ISignUpRequest';
import type { IUser } from '../models/IUser';

export const authenticate = async (
  request: ILogInRequest,
  abortSignal?: AbortSignal,
): Promise<IJwt> => {
  const httpClient = useHttpClient();
  const { data } = await httpClient.post<IJwt>('users/log-in', request, {
    signal: abortSignal,
  });
  return data;
};

export const getCurrentUser = async (abortSignal?: AbortSignal): Promise<IUser> => {
  const httpClient = useHttpClient();
  const { data } = await httpClient.get<IUser>('users/me', {
    signal: abortSignal,
  });
  return data;
};

export const checkUsernameAvailability = async (
  username: string,
  abortSignal?: AbortSignal,
): Promise<boolean> => {
  const httpClient = useHttpClient();
  const { data } = await httpClient.get<boolean>(`users/username-available/${username}`, {
    signal: abortSignal,
  });
  return data;
};

export const signUp = async (
  signUpRequest: ISignUpRequest,
  abortSignal?: AbortSignal,
): Promise<void> => {
  const httpClient = useHttpClient();
  await httpClient.post<void>('users/sign-up', signUpRequest, {
    signal: abortSignal,
  });
};

export const confirmAccount = async (emailConfirmationToken: string, abortSignal?: AbortSignal) => {
  const httpClient = useHttpClient();
  await httpClient.put(`users/confirm-account/${emailConfirmationToken}`, undefined, {
    signal: abortSignal,
  });
};

export const requestPasswordReset = async (request: { email: string }, abortSignal?: AbortSignal): Promise<void> => {
  const httpClient = useHttpClient();
  await httpClient.post<void>('users/forgot-password', request, {
    signal: abortSignal,
  });
};

export const resetPassword = async (token: string, password: string, confirmPassword: string, abortSignal?: AbortSignal): Promise<void> => {
  const httpClient = useHttpClient();
  await httpClient.post<void>('users/reset-password', { token, password, confirmPassword }, {
    signal: abortSignal,
  });
};
