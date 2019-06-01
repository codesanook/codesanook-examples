import * as React from 'react';

const Profile = (props) => {

    return (
        <div>
            <ul>
                <li>First name: {props.user.firstName}</li>
                <li>Last name:{props.user.lastName}</li>
            </ul>
        </div>
    );
};

export default Profile;
