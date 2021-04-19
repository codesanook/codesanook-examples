import { useState } from 'react';
import { css } from '@emotion/react'

function App() {
  const [counter, setCounter] = useState(0);

  const handleButtonClick = () => {
    const valueToAdd = 1;
    setCounter(previous => previous + valueToAdd);
  }

  const style = css`
    border: 1px solid #ccc;
  `;

  return (
    <div css={style}>
      <p>
        counter value:  <span>{counter}</span>
      </p>
      <button onClick={handleButtonClick}>Click Me</button>
    </div>
  );
}

export default App;
