import React, { useContext } from 'react';
import { FaRegSun, FaTrash } from 'react-icons/fa';
import TableOfContentContext from './TableOfContentContext';

export default function BlockSetting() {

  const { dispatch } = useContext(TableOfContentContext);
  const handleClick = () => {
    dispatch({ type: "DEFAULT" });
  };

  return (
    <span>
      <FaRegSun
        onClick={handleClick}
        css={{
          cursor: "pointer"
        }}
      />
      <FaTrash
        css={{
          cursor: "pointer"
        }}
      />
    </span>
  )
}
