import React, { Component } from "react";
import {   BrowserRouter as Router, Switch, Route } from "react-router-dom";
import OauthTestingFlow from "./components/Authorization/OauthTestingFlow";
import AuthorizationCallback from "./components/Authorization/AuthorizationCallback";

export default function App() {

  return (
    <Router>
      <Switch>
        <Route exact path="/" component={OauthTestingFlow} />
        <Route exact path="/authentication/login-callback" component={AuthorizationCallback} />
      </Switch>
    </Router>
  );
}
