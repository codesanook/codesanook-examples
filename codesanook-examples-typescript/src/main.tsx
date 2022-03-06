import './scss/style.scss';
import './index.html';
import React, { useState } from 'react';
import { render } from 'react-dom';
import { Controlled as CodeMirror } from 'react-codemirror2';
import Svg from './components/ConnectedScatterPlotD3Component';

// https://github.com/scniro/react-codemirror2#requiring-codemirror-resources
// Ad vim mode
// https://stackoverflow.com/a/15184747/1872200
import 'codemirror/addon/dialog/dialog';
import 'codemirror/addon/search/searchcursor'
import 'codemirror/mode/clike/clike';
import 'codemirror/keymap/vim';

const Editor = () => {
  const [value, setValue] = useState('');

  return (
    <CodeMirror
      options={{
        theme: 'material',
        mode: "text/x-csrc",
        keyMap: 'vim',
        lineNumbers: true,
      }}
      value={value}
      onBeforeChange={(editor, data, value) => {
        setValue(value);
      }}
      onChange={(editor, data, value) => {
        console.log(value);
      }}
    />
  )
}

render(
  <Svg />,
  document.getElementsByClassName('d3-component')[0]
)

render(
  <Editor />,
  document.getElementsByClassName('text-editor')[0]
);
