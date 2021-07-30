import React, { Component } from "react";
import { Home } from "./components/Home";
import { FetchData } from "./components/FetchData";
import { Counter } from "./components/Counter";
import { Router, Switch, Route } from "react-router-dom";
import { ApplicationPaths } from "./components/Authorization/constant";
import AuthorizationRoutes from "./components/Authorization/AuthorizationRoutes";
import Oauth2Testing from "./components/Authorization/Oauth2Testing";
import AuthorizationCallback from "./components/Authorization/AuthorizationCallback";

import { createBrowserHistory } from 'history';
const browserHistory = createBrowserHistory();

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Router history={browserHistory}>
        <Switch>
          <Route exact path="/" component={Home} />
          <Route exact path="/oauth2-testing" component={Oauth2Testing} />
          <Route exact path="/authentication/login-callback" component={AuthorizationCallback} />

          <Route path="/counter" component={Counter} />
          <Route path="/fetch-data" component={FetchData} />
          <Route
            path={ApplicationPaths.ApiAuthorizationPrefix}
            component={AuthorizationRoutes}
          />
        </Switch>
      </Router>
    );
  }
}
