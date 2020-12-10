// Credit https://dev.to/florantara/creating-a-drag-and-drop-list-with-react-hooks-4c0i
import React from 'react';
import ReactDOM from 'react-dom';
import './drag-and-drop.html';
import './style.scss';

const initialDnDState = {
  draggedFrom: null,
  draggedTo: null,
  originalOrder: [],
  updatedOrder: [],
};

const items = [
  { number: '1', title: '🇦🇷 Argentina' },
  { number: '2', title: '🤩 YASS' },
  { number: '3', title: '👩🏼‍💻 Tech Girl' },
  { number: '4', title: '💋 Lipstick & Code' },
  { number: '5', title: '💃🏼 Latina' },
];

// The only component we'll have:
// It will loop through the items
// and display them.
// For now, this is a static array.
const DragToReorderList = () => {
  // We'll use the initialDndState created above
  const [dragAndDrop, setDragAndDrop] = React.useState(initialDnDState);

  // The initial state of "list"
  // is going to be the static "items" array
  const [list, setList] = React.useState(items);

  const onDragStart = (event) => {
    // It receives a DragEvent
    // which inherits properties from
    // MouseEvent and Event
    // so we can access the element
    // through event.currentTarget

    // Later, we'll save
    // in a hook variable
    // the item being dragged

    // We'll access the "data-position" attribute
    // of the current element dragged
    const initialPosition = Number(event.currentTarget.dataset.position);

    // Set hook
    setDragAndDrop({
      // we spread the previous content
      // of the hook variable
      // so we don't override the properties
      // not being updated
      ...dragAndDrop,

      draggedFrom: initialPosition, // set the draggedFrom position
      originalOrder: list, // store the current state of "list"
    });

    // Note: this is only for Firefox.
    // Without it, the DnD won't work.
    // But we are not using it.
    event.dataTransfer.setData('text/html', '');
  };

  const onDragOver = (event) => {
    // It also receives a DragEvent.
    // Later, we'll read the position
    // of the item from event.currentTarget
    // and store the updated list state

    // We need to prevent the default behavior
    // of this event, in order for the onDrop
    // event to fire.
    // It may sound weird, but the default is
    // to cancel out the drop.

    event.preventDefault();

    // Store the content of the original list
    // in this variable that we'll update
    let newList = dragAndDrop.originalOrder;

    // index of the item being dragged
    const { draggedFrom } = dragAndDrop;

    // index of the drop area being hovered
    const draggedTo = Number(event.currentTarget.dataset.position);

    // get the element that's at the position of "draggedFrom"
    const itemDragged = newList[draggedFrom];

    // filter out the item being dragged
    const remainingItems = newList.filter((item, index) => index !== draggedFrom);
    // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/slice
    const before = remainingItems.slice(0, draggedTo);
    const after = remainingItems.slice(draggedTo);
    // update the list
    newList = [
      ...before,
      itemDragged,
      ...after,
    ];

    // since this event fires many times
    // we check if the targets are actually
    // different:
    if (draggedTo !== dragAndDrop.draggedTo) {
      setDragAndDrop({
        ...dragAndDrop,

        // save the updated list state
        // we will render this onDrop
        updatedOrder: newList,
        draggedTo,
      });
    }
  };

  const onDrop = () => {
    // Here, we will:
    // - update the rendered list
    // - and reset the DnD state
    // we use the updater function
    // for the "list" hook
    setList(dragAndDrop.updatedOrder);

    // and reset the state of
    // the DnD
    setDragAndDrop({
      ...dragAndDrop,
      draggedFrom: null,
      draggedTo: null,
    });
  };

  return (
    <section>
      <ul>
        {list.map((item, index) => (
          <li
            data-position={index}
            key={item.number}
            draggable="true"
            onDragStart={onDragStart}
            onDragOver={onDragOver}
            onDrop={onDrop}
            className={dragAndDrop && dragAndDrop.draggedTo === Number(index) ? 'dropArea' : ''}
          >
            <span>{item.number}</span>
            <p>{item.title}</p>
            <i className="fas fa-arrows-alt-v" />
          </li>
        ))}
      </ul>
    </section>
  );
};

ReactDOM.render(
  <DragToReorderList />,
  document.getElementById('root'),
);
