import axios from 'axios';

export default function AuthorizationCallback() {

  const getToken = async () => {
    // Axios is able to accept a URLSearchParams instance which also set the appropriate Content-type header to application/x-www-form-urlencoded
    const refreshToken = localStorage.getItem('refresh_token') as string;
    const parameters: Record<string, string> = {
      grant_type: 'refresh_token',
      client_id: process.env.REACT_APP_CLIENT_ID as string,
      refresh_token: refreshToken,
    };

    const config = {
      headers: {
        // We need to use form-url encode https://github.com/openiddict/openiddict-core/issues/437
        // https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.3
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }

    // Request token
    const response = await axios.post(
      process.env.REACT_APP_TOKEN_ENDPOINT as string,
      new URLSearchParams(parameters),
      config,
    );

    // Set token to local storage
    localStorage.setItem('access_token', response.data.access_token);
    localStorage.setItem('refresh_token', response.data.refresh_token);
    alert('Got token and set to local storage');
  };

  return (
    <div className='block-list-item'>
      <button className='link-button' onClick={getToken}>Get a new token from a refresh token</button>
    </div>
  );
};
