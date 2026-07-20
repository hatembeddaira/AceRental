import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
  issuer: 'http://localhost:5001',
  redirectUri: 'http://localhost:3000',
  clientId: 'ace-rental-angular',
  scope: 'openid profile email api',
  responseType: 'code',
  showDebugInformation: true,
  useSilentRefresh: true,
  silentRefreshRedirectUri: 'http://localhost:3000/silent-refresh.html',
  silentRefreshTimeout: 20000,
  clearHashAfterLogin: true,
};