import React from 'react';
import Task from './Task';
import { Droppable } from 'react-beautiful-dnd'

export default function Column({ column, tasks }) {

  return (
    <>
      <div
        css={{
          margin: '8px',
          border: '1px solid lightgrey',
          borderRadius: '2px'
        }}
      >
        <h3
          css={{ padding: '8px' }}
        >
          {column.title}
        </h3>
        <Droppable droppableId={column.id}>
          {
            (provided, snapshot) => (
              <div
                ref={provided.innerRef}
                {...provided.droppableProps}
                css={{
                  padding: '8px',
                  backgroundColor: snapshot.isDraggingOver ? 'skyblue' : 'white'
                }}
              >
                { tasks.map((task, index) =>
                  <Task key={task.id} task={task} index={index} />
                )}
                {provided.placeholder}
              </div>
            )
          }
        </Droppable>
      </div>
    </>
  );

}
