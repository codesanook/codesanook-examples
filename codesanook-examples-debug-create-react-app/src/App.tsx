import React from 'react';
// import { useState } from 'react';
// import { css } from '@emotion/react'
import { BrowserRouter, Switch, Route, Link } from 'react-router-dom'
import Admin from './pages/admin/Admin';
import About from './pages/frontend/About';
import Frontend from './pages/frontend/Frontend';
import Home from './pages/frontend/Home';

function App() {
  // const [counter, setCounter] = useState(0);

  // const handleButtonClick = () => {
  //   const valueToAdd = 1;
  //   setCounter(previous => previous + valueToAdd);
  // }

  // const style = css`
  //   border: 1px solid #ccc;
  // `;

  // return (
  //   <div css={style}>
  //     <p>
  //       counter value:  <span>{counter}</span>
  //     </p>
  //     <button onClick={handleButtonClick}>Click Me</button>
  //   </div>
  // );
  return (
    <>
      <BrowserRouter>
        <div>
          <main>
            <Switch>
              <Route path='/admin'>
                <Admin />
              </Route>
              <Route path='/'>
                <Frontend />
              </Route>
            </Switch>
          </main>
        </div>
      </BrowserRouter>
    </>
  )
}

export default App;
