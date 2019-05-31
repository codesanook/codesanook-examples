const Sample = (props) => {

    const handleButtonClick = () => {
        alert($`Hello ${props.name}`);
    };

    return (
        <div>
            <h1>Hello world {props.name}</h1>
            <p>
                How to be {props.name}
            </p>
            <button onClick={handleButtonClick}>Say hello </button>
        </div>
    );
};
