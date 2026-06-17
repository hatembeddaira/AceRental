import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
  // URL de ton serveur d'identité (OAuth2/OIDC provider)
  issuer: 'http://localhost:5001/connect/authorize',
  // URL vers laquelle l'utilisateur est redirigé après s'être connecté
  redirectUri: window.location.origin + '/index.html',
  // L'identifiant de ton application Angular enregistré sur le serveur
  clientId: 'ace-rental-angular',
  // Le scope requis (openid et profile sont standards pour l'OIDC)
  scope: 'openid profile email api',
  responseType: 'code', // Requis pour l'Authorization Code Flow
  showDebugInformation: true, // À passer à false en production
  useSilentRefresh: true,// Active le rafraîchissement automatique des jetons en arrière-plan
};