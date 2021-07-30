import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import AuthorizationTestingFlow from "./components/Authorization/AuhorizationTestingFlow";
import AuthorizationCallback from "./components/Authorization/AuthorizationCallback";
import './App.css';

export default function App() {

  return (
    <Router>
      <Switch>
        <Route exact path="/" component={AuthorizationTestingFlow} />
        <Route exact path="/authentication/login-callback" component={AuthorizationCallback} />
      </Switch>
    </Router>
  );
}
