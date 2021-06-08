import React, { useEffect } from "react";
import { NavItem, NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import authService, {
  AuthenticationResultStatus,
} from "./AuthorizationService";
import { QueryParameterNames, LoginActions } from "./constant";

interface PropType {
  action: string;
}

const Login = ({ action, ...props }: PropType) => {
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
      // case LoginActions.LoginFailed:
      //     const params = new URLSearchParams(window.location.search);
      //     const error = params.get(QueryParameterNames.Message);
      //     this.setState({ message: error });
      //     break;
      // case LoginActions.Profile:
      //     this.redirectToProfile();
      //     break;
      // case LoginActions.Register:
      //     this.redirectToRegister();
      //     break;
      default:
        throw new Error(`Invalid action '${action}'`);
    }
  };

  
  const processLoginCallback = async () =>{
      const url = window.location.href;
      const result = await authService.completeSignIn(url);
      // const result = await authService.completeSignIn(url);
      // switch (result.status) {
      //     case AuthenticationResultStatus.Redirect:
      //         // There should not be any redirects as the only time completeSignIn finishes
      //         // is when we are doing a redirect sign in flow.
      //         throw new Error('Should not redirect.');
      //     case AuthenticationResultStatus.Success:
      //         await this.navigateToReturnUrl(this.getReturnUrl(result.state));
      //         break;
      //     case AuthenticationResultStatus.Fail:
      //         this.setState({ message: result.message });
      //         break;
      //     default:
      //         throw new Error(`Invalid authentication result status '${result.status}'.`);
      // }
  }

  const login = async (returnUrl: string) => {
    const state = { returnUrl };
    const result = await authService.signIn(state);
    // switch (result.status) {
    //   case AuthenticationResultStatus.Redirect:
    //     break;
    //   case AuthenticationResultStatus.Success:
    //     await navigateToReturnUrl(returnUrl);
    //     break;
    //   case AuthenticationResultStatus.Fail:
    //     // setState({ message: result.message });
    //     break;
    //   default:
    //     throw new Error(`Invalid status result ${result.status}.`);
    // }
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
    // return (
    //   (state && state.returnUrl) || fromQuery || `${window.location.origin}/`
    // );
    return fromQuery || `${window.location.origin}/`;
  };

  const navigateToReturnUrl = (returnUrl: string) => {
    // It's important that we do a replace here so that we remove the callback uri with the
    // fragment containing the tokens from the browser history.
    window.location.replace(returnUrl);
  };

  return <>Login Page</>;
};

export default Login;
