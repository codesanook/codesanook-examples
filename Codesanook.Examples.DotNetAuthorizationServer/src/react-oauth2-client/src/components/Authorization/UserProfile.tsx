import React, { useState } from 'react';
import axios from 'axios';

const UserProfile = () => {
  const [userProfile, setUserProfile] = useState('');

  const handleClick = async () => {
    try {
      const accessToken = localStorage.getItem('access_token');
      var config = {
        headers: { Authorization: `Bearer ${accessToken}` },
      };

      const response = await axios.get(new URL('users', process.env.REACT_APP_API_ENDPOINT).toString(), config);
      setUserProfile(JSON.stringify(response.data, null, 2));
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>
      <p>
        <button onClick={handleClick}>Get user profile</button>
      </p>
      <h4>profile</h4>
      <pre>
        <code>{userProfile}</code>
      </pre>
    </div>
  );
};

export default UserProfile;
