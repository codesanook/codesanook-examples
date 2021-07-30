import React from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { ApplicationPaths, LoginActions } from "./constant";
import Login from './Login';

interface PropType { }

const AuthorizationRoutes = ({ ...props }: PropType) => {

  function loginAction(name: string) {
    return (<Login action={name}></Login>);
  }

  return (
    <>
      <Route path={ApplicationPaths.Login} render={() => loginAction(LoginActions.Login)} />
      {/* <Route path={ApplicationPaths.LoginFailed} render={() => loginAction(LoginActions.LoginFailed)} /> */}
      <Route path={ApplicationPaths.LoginCallback} render={() => loginAction(LoginActions.LoginCallback)} />
    </>
  );
};

export default AuthorizationRoutes;
