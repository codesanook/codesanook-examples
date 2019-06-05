import * as React from 'react';

const Counter = (props:any) => {

    const [counter, setCounter] = React.useState(props.defaultCounterValue);
    const handleButtonClick = () => {
        setCounter(counter + 1);
    };

    return (
        <div>
            <h1>Current counter {counter}</h1>
            <p>
                Click button to increase counter
            </p>
            <button onClick={handleButtonClick}>Increase counter</button>
        </div>
    );
};

export default Counter;
