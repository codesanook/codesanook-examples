import React, { useState } from 'react';
import { render } from 'react-dom';
import { css } from '@emotion/react';
import { ReactSortable } from 'react-sortablejs';
import './nested-drag-and-drop.html';

const styleBlockWrapper = css({
  position: 'relative',
  background: 'white',
  padding: '20px',
  marginBottom: '10px',
  border: '1px solid lightgray',
  borderRadius: '4px',
  cursor: 'move',
});

const sortableOptions = {
  animation: 150,
  fallbackOnBody: true,
  swapThreshold: 0.65,
  ghostClass: 'ghost',
  group: 'shared',
};

export default function App() {
  const [blocks, setBlocks] = useState([
    {
      id: 1,
      content: 'item 1',
      parent_id: null,
      type: 'container',
      children: [
        {
          id: 2,
          content: 'item 2',
          width: 3,
          type: 'text',
          parent_id: 1,
        },
        {
          id: 3,
          content: 'item 3',
          width: 3,
          type: 'text',
          parent_id: 1,
        },
      ],
    },
    {
      id: 4,
      content: 'item 2',
      parent_id: null,
      type: 'container',
      children: [
        {
          id: 5,
          content: 'item 5',
          width: 3,
          parent_id: 2,
          type: 'container',
          children: [
            {
              id: 8, content: 'item 8', width: 6, type: 'text', parent_id: 5,
            },
            {
              id: 9, content: 'item 9', width: 6, type: 'text', parent_id: 5,
            },
          ],
        },
        {
          id: 6,
          content: 'item 6',
          width: 2,
          type: 'text',
          parent_id: 2,
        },
        {
          id: 7,
          content: 'item 7',
          width: 2,
          type: 'text',
          parent_id: 2,
        },
      ],
    },
  ]);

  return (
  // eslint-disable-next-line react/jsx-no-comment-textnodes
    <div>
      <ReactSortable list={blocks} setList={setBlocks} {...sortableOptions}>
        {blocks.map((block, blockIndex) => (
          // eslint-disable-next-line @typescript-eslint/no-use-before-define
          <BlockWrapper
            key={block.id}
            block={block}
            blockIndex={[blockIndex]}
            setBlocks={setBlocks}
          />
        ))}
      </ReactSortable>
    </div>
  );
}

function Container({ block, blockIndex, setBlocks }) {
  return (
    <>
      <ReactSortable
                // eslint-disable-next-line react/prop-types
        key={block.id}
        list={block.children}
        setList={(currentList) => {
          setBlocks((sourceList) => {
            const tempList = [...sourceList];
            // eslint-disable-next-line no-underscore-dangle
            const _blockIndex = [...blockIndex];
            const lastIndex = _blockIndex.pop();
            const lastArr = _blockIndex.reduce(
              (arr, i) => arr[i].children,
              tempList,
            );
            console.log(lastIndex);
            lastArr[lastIndex].children = currentList;
            return tempList;
          });
        }}
        // eslint-disable-next-line react/jsx-props-no-spreading
        {...sortableOptions}
      >
        {block.children
                    && block.children.map((childBlock, index) => (
                      // eslint-disable-next-line @typescript-eslint/no-use-before-define
                      <BlockWrapper
                        key={childBlock.id}
                        block={childBlock}
                        blockIndex={[...blockIndex, index]}
                        setBlocks={setBlocks}
                      />
                    ))}
      </ReactSortable>
    </>
  );
}
function BlockWrapper({ block, blockIndex, setBlocks }) {
  // console.log(block);
  if (!block) return null;
  if (block.type === 'container') {
    return (
      <div css={styleBlockWrapper} className="block">
        container:
        {' '}
        {block.content}
        <Container
          block={block}
          setBlocks={setBlocks}
          blockIndex={blockIndex}
        />
      </div>
    );
  }
  return (
    <div css={styleBlockWrapper} className="block">
      text:
      {' '}
      {block.content}
    </div>
  );
}

render(
  <App />,
  document.getElementById('root'),
);
