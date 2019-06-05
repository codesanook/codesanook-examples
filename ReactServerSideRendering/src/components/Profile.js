import * as React from 'react';
const Profile = (props) => {
    return (React.createElement("div", null,
        React.createElement("ul", null,
            React.createElement("li", null,
                "First name: ",
                props.user.firstName),
            React.createElement("li", null,
                "Last name:",
                props.user.lastName))));
};
export default Profile;
