import './App.css';
import { useState } from 'react';
import config from './config';

function App() {
  const [counter, setCounter] = useState(0);

  const handleButtonClick = () => {
    let valueToAdd = 1;
    setCounter(previous => previous + valueToAdd);
  }

  return (
    <>
      <h1>{config.siteName}</h1>
      <div className="App">

        <p>
          counter value  {counter}
        </p>
        <button onClick={handleButtonClick}>Click Me</button>
      </div>
    </>
  );
}

export default App;
