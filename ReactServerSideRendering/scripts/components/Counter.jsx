import * as React from 'react';

const Counter = (props) => {

    const [counter, setCounter] = React.useState(0);
    setCounter(props.defaultCounter);

    const handleButtonClick = () => {
        counter++;
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
