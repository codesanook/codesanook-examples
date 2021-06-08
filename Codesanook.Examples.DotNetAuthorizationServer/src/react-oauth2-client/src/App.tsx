import React, { Component } from "react";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { FetchData } from "./components/FetchData";
import { Counter } from "./components/Counter";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import {ApplicationPaths} from "./components/Authorization/constant";
import AuthorizationRoutes from "./components/Authorization/AuthorizationRoutes";

import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      // <Layout>
        <Router>
          <Switch>
            <Route exact path="/" component={Home} />
            <Route path="/counter" component={Counter} />
            {/* <AuthorizeRoute path="/fetch-data" component={FetchData} /> */}
            <Route
              path={ApplicationPaths.ApiAuthorizationPrefix}
              component={AuthorizationRoutes}
            />
           </Switch>
        </Router>
      // </Layout>
    );
  }
}
