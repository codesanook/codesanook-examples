import React, { useEffect } from "react";
import authService from "./AuthorizationService";
import { QueryParameterNames, LoginActions } from "./constant";

interface PropsType {
  action: string;
}

const Login = ({ action, ...props }: PropsType) => {
  useEffect(() => {
    init();
  }, []);

  const init = () => {
    switch (action) {
      case LoginActions.Login:
        login(getReturnUrl());
        break;
      case LoginActions.LoginCallback:
        processLoginCallback();
        break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  };


  const processLoginCallback = async () => {
    const url = window.location.href;
    const result = await authService.completeSignIn(url);
    window.location.href = '/fetch-data';
  }

  const login = async (returnUrl: string) => {
    const state = { returnUrl };
    const result = await authService.signIn(state);
  };

  const getReturnUrl = () => {
    const params = new URLSearchParams(window.location.search);
    const fromQuery = params.get(QueryParameterNames.ReturnUrl);
    if (fromQuery && !fromQuery.startsWith(`${window.location.origin}/`)) {
      // This is an extra check to prevent open redirects.
      throw new Error(
        "Invalid return url. The return url needs to have the same origin as the current page."
      );
    }
    return fromQuery || `${window.location.origin}/`;
  };

  const navigateToReturnUrl = (returnUrl: string) => {
    // It's important that we do a replace here so that we remove the callback uri with the
    // fragment containing the tokens from the browser history.
    window.location.replace(returnUrl);
  };

  return <><h1>Login Page</h1>See the token in the Console</>;
};

export default Login;
