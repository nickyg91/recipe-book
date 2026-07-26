import { useHttpClient } from '../http-client';
import type { IJwt } from '../models/IJwtToken';
import type { ILogInRequest } from '../models/ILogInRequest';
import type { IUser } from '../models/IUser';

export const authenticate = async (
  request: ILogInRequest,
  abortSignal?: AbortSignal,
): Promise<IJwt> => {
  const httpClient = useHttpClient();
  return await httpClient.post('users/log-in', request, {
    signal: abortSignal,
  });
};

export const getCurrentUser = async (abortSignal?: AbortSignal): Promise<IUser> => {
  const httpClient = useHttpClient();
  return await httpClient.get('users/me', {
    signal: abortSignal,
  });
};
