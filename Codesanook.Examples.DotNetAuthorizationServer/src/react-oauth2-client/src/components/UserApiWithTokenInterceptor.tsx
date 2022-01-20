// https://thedutchlab.com/blog/using-axios-interceptors-for-refreshing-your-api-token
import { useState } from 'react';
import axios, { AxiosError } from 'axios';

const client = createClient();
const endpoint = new URL('users', process.env.REACT_APP_API_ENDPOINT).toString();

export default function UserApiWithTokenInterceptor() {
  const [userProfile, setUserProfile] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleClick = async () => {
    setIsLoading(true);
    // Clear existing userProfile
    setUserProfile('');
    try {
      const response = await client.get(endpoint);
      await delay(1000);
      setUserProfile(JSON.stringify(response.data, null, 2));
    } catch (error) {
      console.error(error);
    }
    setIsLoading(false);
  };

  return (
    <>
      <div className='block-list-item'>
        <button className='link-button' onClick={handleClick}>Get users API ({endpoint})</button>
      </div>
      {
        isLoading && (<h4>Loading...</h4>)
      }
      {
        userProfile &&
        <pre>
          <code>{userProfile}</code>
        </pre>
      }
    </>
  );
};

function createClient() {
  const client = axios.create();

  // Request interceptor for API calls
  client.interceptors.request.use(
    async config => {
      const accessToken = localStorage.getItem('access_token');
      config.headers = {
        Authorization: `Bearer ${accessToken}`,
        Accept: 'application/json',
        'Content-Type': 'application/x-www-form-urlencoded'
      }
      return config;
    },
    error => Promise.reject(error)
  );

  // Response interceptor for API calls
  client.interceptors.response.use(
    response => response,
    async error => {
      const originalRequest = error.config;
      if (error.response.status === 401 && !originalRequest._retry) {
        // Get new access token and retry again
        originalRequest._retry = true;
        const accessToken = await getNewAccessToken();
        axios.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
        return client(originalRequest);
      }
      return Promise.reject(error);
    });

  return client;
}

// Get new access and refresh token
async function getNewAccessToken() {
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

  // Request a new token
  try {
    const response = await axios.post(
      process.env.REACT_APP_TOKEN_ENDPOINT as string,
      new URLSearchParams(parameters),
      config,
    );

    // Set token to local storage
    localStorage.setItem('access_token', response.data.access_token);
    localStorage.setItem('refresh_token', response.data.refresh_token);
    alert('Got new access + refresh token and set to a local storage');
  } catch (err) {
    if (axios.isAxiosError(err)) {
      const axiosError = err as AxiosError;
      alert(`Cannot get a new access + refresh token, statusCode: ${axiosError.response?.status}, message: ${axiosError.message}`);
    }
    console.error(err);
  }
};

function delay(millisecond: number) {
  return new Promise(resolve => setTimeout(resolve, millisecond));
}
