import './scss/style.scss';
import './index.html';
import React, { useState } from 'react';
import { render } from 'react-dom';
import { Controlled as CodeMirror } from 'react-codemirror2'

// https://github.com/scniro/react-codemirror2#requiring-codemirror-resources
import 'codemirror/mode/xml/xml';
import 'codemirror/mode/javascript/javascript';

const Editor = () => {

    const [value, setValue] = useState('');

    return (
        <CodeMirror
            options={{
                mode: 'xml',
                theme: 'material',
                lineNumbers: true
            }}
            value={value}
            onBeforeChange={(editor, data, editorValue) => {
                setValue(editorValue);
            }}
            onChange={(editor, data, editorValue) => {
                //setValue(editorValue);
                console.log(editorValue);
            }}
        />
    )
}
render(
    <Editor />,
    document.getElementById('root')
);
