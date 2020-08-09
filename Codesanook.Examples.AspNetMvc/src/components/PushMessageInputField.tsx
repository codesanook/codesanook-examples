import React from 'react';

interface PushMessageProps {
    message: string;
    isReadOnly?: boolean;
}

// ES6 object destructuring syntax
const PushMessageInputField = ({ isReadOnly = false, ...props }) => {
    return (
        <div>
            <div>
                <textarea value={props.message} readOnly={isReadOnly} rows={5}>
                </textarea>
            </div>
        </div>
    );
};

export default PushMessageInputField;
