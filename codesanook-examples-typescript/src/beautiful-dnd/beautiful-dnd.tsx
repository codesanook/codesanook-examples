import React, { useEffect, useState } from 'react';
import ReactDOM from 'react-dom';
import '@atlaskit/css-reset';
import './style.scss';
import './beautiful-dnd.html';
import initialData from './initial-data';
import Column from './Column';
import { DragDropContext } from 'react-beautiful-dnd';

function App() {
  const [state, setState] = useState(initialData);

  const onDragEnd = (result) => {
    const { destination, source, draggableId } = result;
    if (!destination) {
      return;
    }

    if (destination.droppableId === source.droppableId &&
      destination.index === source.index
    ) {
      return;
    }

    const column = state.columns[source.droppableId];
    const newTaskIs = Array.from(column.taskIds);

    newTaskIs.splice(source.index, 1); // remove source
    newTaskIs.splice(destination.index, 0, draggableId);

    const newColumn = {
      ...column,
      taskIds: newTaskIs
    };

    const newState = {
      ...state,
      columns: {
        ...state.columns,
        [newColumn.id]: newColumn
      }
    };
    setState(newState);
  };

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <div>
        {state.columnOrder.map((columnId) => {
          const column = state.columns[columnId];
          const tasks = column.taskIds.map((taskId) => state.tasks[taskId]);
          return (
            <Column key={column.id} column={column} tasks={tasks} />
          );
        })}
      </div>
    </DragDropContext>
  );
}

ReactDOM.render(
  <App />,
  document.getElementById('root'),
);
