// https://upmostly.com/tutorials/how-to-use-the-usecontext-hook-in-react
import React, { useState, createContext, useEffect } from 'react';
import { hubConnection } from 'signalr-no-jquery';

export const ConnectionContext = createContext(
    [{}, () => { }] // init value
);

export const ConnectionProvider = props => {
    const signalConnection = hubConnection();
    const [connection, setConnection] = useState(signalConnection);
    useEffect(() => {
        connection.start().done(() => {
            console.log('Backend connected');
        });
    }, []); // Run only one time

    return (
        <ConnectionContext.Provider value={[connection, setConnection]}>
            {props.children}
        </ConnectionContext.Provider>
    );
}
