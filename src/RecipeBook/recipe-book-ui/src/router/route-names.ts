export const RouteNames = {
  LogIn: 'LogIn',
  Recipes: 'Recipes',
  ConfirmAccount: 'ConfirmAccount',
  ForgotPassword: 'ForgotPassword',
  ResetPassword: 'ResetPassword',
} as const;

export type RouteName = (typeof RouteNames)[keyof typeof RouteNames];
