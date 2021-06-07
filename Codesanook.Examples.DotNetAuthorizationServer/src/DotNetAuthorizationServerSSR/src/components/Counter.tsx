import React, { useState } from 'react';
import { css } from '@emotion/react'

const style = css`
  border: 1px solid #ccc;
  margin-top: 200px;
  padding: 10px;
`;

type Props = {
  initValue: number
}

const Counter = ({ initValue }: Props) => {
  const [counter, setCounter] = useState(initValue);
  const handleButtonClicked = () => {
    setCounter(counter + 1);
  }
  return (
    <div css={style}>
      <button onClick={handleButtonClicked}>
        Click me na
      </button>
      <div>
        Current value {counter}
      </div>
    </div>
  )
};

export default Counter;
