import React, { useEffect } from 'react';
import { render, Color } from 'ink';

// https://dev.to/zhiyueyi/how-to-create-a-simple-react-countdown-timer-4mc3
// https://github.com/vadimdemedes/ink
const Counter = () => {
    const [counter, setCounter] = React.useState(0);

    useEffect(() => {
        const timer = setInterval(
            () => setCounter((previousValue) => previousValue + 1),
            100
        );
        return () => clearInterval(timer);
    }, []);

    return <Color green>{counter} tests passed</Color>;
};
render(<Counter />);
