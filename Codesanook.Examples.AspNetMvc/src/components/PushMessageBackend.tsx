import {FunctionComponent, MouseEvent, ChangeEvent} from 'react';
import React, {useState, useEffect, useContext} from 'react';
import PushMessageInputField from './PushMessageInputField';
import { ConnectionContext } from './ConnectionContext'

const PushMessageBackend = () => {
    const [message, setMessage] = useState('');
    const [connection, setConnection] = useContext(ConnectionContext); 
    const hub = connection.createHubProxy('pushMessageHub');
    const sendMessage = (e: MouseEvent<HTMLButtonElement>) => {
        hub.invoke('sendMessage', message);
        setMessage('');
    };

    const handleTextChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
        const value = e.currentTarget.value;
        setMessage(value);
    };

    return (
        <>
            <h6>PushMessageBackend</h6>
            <textarea value={message} onChange={handleTextChange} />
            <div>
                <button onClick={sendMessage}>send</button>
            </div>
        </>
    );
};

export default PushMessageBackend;
