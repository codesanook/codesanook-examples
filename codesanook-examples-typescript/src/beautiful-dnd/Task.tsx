import React from 'react';
import { Draggable } from 'react-beautiful-dnd';

export default function Task({ task, index }) {

  const clickHanlder = () => {
    console.log('clicked');
  };

  return (
    <Draggable draggableId={task.id} index={index}>
      { (provided, snapshot) =>
        <div
          {...provided.draggableProps}
          ref={provided.innerRef}
          css={{
            border: '1px solid lightgrey',
            borderRadius: '2px',
            padding: '8px',
            marginBottom: '8px',
            backgroundColor: snapshot.isDragging ? 'lightgreen' : 'white',
            display: 'flex'
          }} onClick={clickHanlder}>
          <div
            {...provided.dragHandleProps}
            css={{
              width: '20px',
              height: '20px',
              backgroundColor: 'orange',
              borderRadius: '4px',
              marginRight: '8px'
            }}>
          </div>
          {task.content}
        </div>
      }
    </Draggable>
  );
}
