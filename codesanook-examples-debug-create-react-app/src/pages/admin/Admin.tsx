import React from 'react';
import Home from './Home'
import User from './User'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link,
  useRouteMatch
} from "react-router-dom";


export default function Admin() {
  let match = useRouteMatch();

  return (
    <div>
      <ul>
        <li>
          <Link to='/'>
            back to home
          </Link>
        </li>
        <li>
          <Link to={`${match.url}/user`}>
            user admin
          </Link>
        </li>
      </ul>
      <Switch>
        <Route path={`${match.path}/home`}>
          <Home />
        </Route>
        <Route path={`${match.path}/user`}>
          <User />
        </Route>
      </Switch>
    </div>
  )
}




