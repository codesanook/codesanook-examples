import {FunctionComponent, MouseEvent, ChangeEvent} from 'react';
import React, {useState, useEffect, useContext} from 'react';
import PushMessageInputField from './PushMessageInputField';
import { ConnectionContext } from './ConnectionContext'

const PushMessageBackend = () => {
    const connection = useContext(ConnectionContext); 
    const [hub, setHub] = useState();
    const [message, setMessage] = useState('');

    useEffect(() => {
        setHub(connection.createHubProxy('pushMessageHub'));
    }, []); // Run only one time

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
