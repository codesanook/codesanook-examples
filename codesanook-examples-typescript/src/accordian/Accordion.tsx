import React, { useRef, useState } from 'react';
import Chevron from './Chevron';

export default function Accordion({ chapter, children }) {

  const [, setActive] = useState(false);
  const [height, setHeight] = useState('0');
  const content = useRef(null);

  const toggleAccordion = () => {
    setActive(previousActive => {
      const newState = !previousActive;
      setHeight(newState ? 'auto' : '0');
      return newState;
    });
  }

  return (
    <div>
      <span>{chapter.title}</span>
      <button onClick={toggleAccordion}
        css={{
          cursor: 'pointer',
          border: 'none',
          outline: 'none',
        }}>
        <Chevron width={10} fill={'#7777'} />
      </button>
      <div
        ref={content}
        css={{
          maxHeight: `${height}`,
          overflow: 'auto', // we need overflow auto to hide max height 0
        }}
      >
        {children}
      </div>
    </div>
  );
};
