import axios, { type AxiosRequestConfig, type AxiosResponse } from 'axios';
import type { IJwt } from './models/IJwtToken';
const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('access_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    const authEndpoints = ['/log-in', '/sign-up', '/confirm-account'];
    if (authEndpoints.some((ep) => originalRequest.url.includes(ep))) {
      return Promise.reject(error);
    }

    // Handle 401: trigger token refresh
    if (error.response?.status === 401) {
      // If we already retried once with a new token, don't loop
      if (originalRequest.headers.Authorization?.startsWith('Bearer ')) {
        // Check if this retry also got a 401 (refresh failed)
        if (originalRequest.headers['X-Retried']) {
          localStorage.removeItem('access_token');
          localStorage.removeItem('refresh_token');
          return Promise.reject(new Error('Unauthorized'));
        }
      }

      const refreshToken = localStorage.getItem('refresh_token');
      if (!refreshToken) {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        return Promise.reject(new Error('Unauthorized'));
      }

      try {
        const response = await axiosInstance.put<IJwt>(`/api/users/token/${refreshToken}/refresh`);

        // Save new tokens
        localStorage.setItem('access_token', response.data.accessToken);
        localStorage.setItem('refresh_token', response.data.refreshToken);

        // Retry original request with new token
        originalRequest.headers.Authorization = `Bearer ${response.data.accessToken}`;
        originalRequest.headers['X-Retried'] = 'true';

        return axiosInstance(originalRequest);
      } catch (refreshError) {
        // Refresh failed - clear tokens
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  },
);

export const useHttpClient = () => {
  const get = async <T>(
    url: string,
    axiosConfig?: AxiosRequestConfig,
  ): Promise<AxiosResponse<T>> => {
    const result = await axiosInstance.get<T>(url, axiosConfig);
    return result;
  };

  const put = async <T>(
    url: string,
    payload?: unknown,
    axiosConfig?: AxiosRequestConfig,
  ): Promise<AxiosResponse<T>> => {
    const result = await axiosInstance.put<T>(url, payload, axiosConfig);
    return result;
  };

  const post = async <T>(
    url: string,
    payload: unknown,
    axiosConfig?: AxiosRequestConfig,
  ): Promise<AxiosResponse<T>> => {
    const result = await axiosInstance.post<T>(url, payload, axiosConfig);
    return result;
  };

  const del = async <T>(
    url: string,
    axiosConfig?: AxiosRequestConfig,
  ): Promise<AxiosResponse<T>> => {
    const result = await axiosInstance.delete<T>(url, axiosConfig);
    return result;
  };

  const httpClient = {
    get: get,
    put: put,
    post: post,
    delete: del,
  };

  return httpClient;
};
