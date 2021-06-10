export const ApplicationName = 'postman';

export const QueryParameterNames = {
  ReturnUrl: 'returnUrl',
  Message: 'message'
};

// export const LogoutActions = {
//   LogoutCallback: 'logout-callback',
//   Logout: 'logout',
//   LoggedOut: 'logged-out'
// };

export const LoginActions = {
  Login: 'login',
  LoginCallback: 'login-callback',
  LoginFailed: 'login-failed',
  Profile: 'profile',
  Register: 'register'
};

const prefix = '/authentication';

export const ApplicationPaths = {
  DefaultLoginRedirectPath: '/',
  ApiAuthorizationClientConfigurationUrl: `_configuration/${ApplicationName}`,
  ApiAuthorizationPrefix: prefix,

  Login: `${prefix}/${LoginActions.Login}`,
//   LoginFailed: `${prefix}/${LoginActions.LoginFailed}`,
  LoginCallback: `${prefix}/${LoginActions.LoginCallback}`,
  Register: `${prefix}/${LoginActions.Register}`,
//   Profile: `${prefix}/${LoginActions.Profile}`,
//   LogOut: `${prefix}/${LogoutActions.Logout}`,
//   LoggedOut: `${prefix}/${LogoutActions.LoggedOut}`,
//   LogOutCallback: `${prefix}/${LogoutActions.LogoutCallback}`,
  // IdentityRegisterPath: 'Identity/Account/Register',
  // IdentityManagePath: 'Identity/Account/Manage'
};

export const LocalStorageConfigs = {
  Oauth: {
    CodeVerifierKey: "code_verifier",
    CodeVerifierTimeoutKey: "code_verifier_timeout",
    // If it's timeout, it'll regenerate a code verifier.
    CodeVerifierTimeout: 3, // seconds
  },
  Token: {
    AccessTokenKey: "access_token",
    RefreshTokenKey: "refresh_token",
  }
};
