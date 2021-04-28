import React from 'react';
import Home from './Home'
import About from './About'
import {
  Switch,
  Route,
  Link,
} from "react-router-dom";
import { TitleTarget } from '../../Title';

export default function Frontend() {
  return (
    <div>
      <ul>
        <li>
          <Link to='/'>
            Home
          </Link>
        </li>
        <li>
          <Link to='/about'>
            About
          </Link>
        </li>
        <li>
          <Link to='/admin'>
            Admin
          </Link>
        </li>
      </ul>
      <h1><TitleTarget /></h1>
      <Switch>
        <Route path='/about'>
          <About />
        </Route>
        <Route path='/'>
          <Home />
        </Route>
      </Switch>
    </div>
  )
}




