import React, { useState, useEffect, useContext } from 'react';
import PushMessageInputField from './PushMessageInputField';
import { ConnectionContext } from './ConnectionContext'

const PushMessageFrontend = () => {
    const [existingMessage, setExistingMessage] = useState('');
    const connection = useContext(ConnectionContext);

    useEffect(() => {
        const hub = connection.createHubProxy('pushMessageHub');
        // set up event listeners i.e. for incoming "message" event
        hub.on('addNewMessage', message => {
            console.log(message);
            setExistingMessage( previousMessage => 
                [previousMessage, message.trim()]
                .filter(m => m) 
                .join('\n')
            );
        });
    }, []); // Run only one time

    return (
        <>
            <h6>PushMessageFrontend</h6>
            <PushMessageInputField message={existingMessage} isReadOnly={true} />
        </>
    );
};

export default PushMessageFrontend;

