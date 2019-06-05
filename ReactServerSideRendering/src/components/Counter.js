import * as React from 'react';
const Counter = (props) => {
    const [counter, setCounter] = React.useState(props.defaultCounterValue);
    const handleButtonClick = () => {
        setCounter(counter + 1);
    };
    return (React.createElement("div", null,
        React.createElement("h1", null,
            "Current counter ",
            counter),
        React.createElement("p", null, "Click button to increase counter"),
        React.createElement("button", { onClick: handleButtonClick }, "Increase counter")));
};
export default Counter;
