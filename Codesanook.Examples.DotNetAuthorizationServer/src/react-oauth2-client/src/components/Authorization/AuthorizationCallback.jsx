import { useEffect, useState } from 'react';
import { getCodeVerifier } from './PKCE';
import queryString from 'query-string';
import axios from 'axios';
import { useHistory, useLocation } from "react-router-dom";

export default function AuthorizationCallback() {

  const [code, setCode] = useState('');
  const history = useHistory();
  const location  = useLocation()

  useEffect(() => {
    const getToken = async () => {
      console.log(JSON.stringify(location, null, 2));

      // const params = new URLSearchParams(location.search);
      // getting access token, refresh token
      const params = queryString.parse(location.search);

      // Set AuthorizationCode
      setCode(params.code);

      // Axios is able to accept a URLSearchParams instance which also set the appropriate Content-type header to application/x-www-form-urlencoded
      const codeVerify = getCodeVerifier();
      const parameters = {
        grant_type: 'authorization_code',
        client_id: process.env.REACT_APP_CLIENT_ID,
        redirect_uri: process.env.REACT_APP_REDIRECT_URI,
        code: params.code,
        code_verifier: codeVerify,
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
        process.env.REACT_APP_TOKEN_ENDPOINT,
        new URLSearchParams(parameters),
        config,
      );

      alert('Got token');

      // Set token to local storage
      localStorage.setItem('access_token', response.data.access_token);
      localStorage.setItem('refresh_token', response.data.refresh_token);
      history.push('/');
    };

    getToken();
    // To do you can make call back to home page
  }, [location]); // run one time on load

  return <div>Getting token from authorization code: {code}</div>;
};
