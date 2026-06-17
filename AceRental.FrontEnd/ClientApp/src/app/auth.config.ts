import { AuthConfig } from 'angular-oauth2-oidc';

export const authCodeFlowConfig: AuthConfig = {
  // URL de votre serveur d'identité (IdentityServer)
  issuer: 'http://localhost:5001',

  // URL vers laquelle l'utilisateur est redirigé après s'être connecté
  redirectUri: 'http://localhost:3000',

  // L'identifiant de votre application Angular, doit correspondre à la configuration du client dans IdentityServer
  clientId: 'ace-rental-angular',

  // Les "scopes" (périmètres) demandés.
  // 'openid', 'profile', 'email' sont standards pour OIDC.
  // 'api' est le scope pour accéder à votre API.
  scope: 'openid profile email api',

  // Type de flux d'authentification
  responseType: 'code',

  // Pour le débogage, à mettre à false en production
  showDebugInformation: true,
  useSilentRefresh: true,
  // Cette page doit exister dans ton dossier 'public' ou 'assets'
  silentRefreshRedirectUri: 'http://localhost:3000/silent-refresh.html',
  
  // Délai avant expiration pour déclencher le refresh (en secondes)
  silentRefreshTimeout: 20000, 
  
  // Optionnel : ne pas rafraîchir si on a déjà un token actif
  clearHashAfterLogin: true,
};