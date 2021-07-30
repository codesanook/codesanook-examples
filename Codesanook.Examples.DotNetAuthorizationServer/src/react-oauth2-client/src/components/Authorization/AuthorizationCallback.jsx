import React, { useEffect, useState } from 'react';
import { getCodeVerifier } from './pkce';
import queryString from 'query-string';
import axios from 'axios';

export default function AuthorizationCallback({ location }) {

  const [code, setCode] = useState('');

  useEffect(() => {
    const getToken = async () => {
      console.log(JSON.stringify(location, null, 2));

      // const params = new URLSearchParams(location.search);
      // getting access token, refresh token
      const params = queryString.parse(location.search);
      // Set AuthorizationCode
      setCode(params.code);

      // http://localhost:3000/oauth/token
      // .SetAuthorizationEndpointUris("/connect/authorize")
      // .SetTokenEndpointUris("/connect/token")
      // .SetUserinfoEndpointUris("/connect/userinfo");


      // Axios is able to accept a URLSearchParams instance which also set the appropriate Content-type header to application/x-www-form-urlencoded


      console.log("about to get token");
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

      // set local state
      localStorage.setItem('access_token', response.data.access_token);
      localStorage.setItem('refresh_token', response.data.refresh_token);
      window.location.href = '/oauth2-testing';
    };

    getToken();
    // To do you can make call back to home page
  }, [location]); // run one time on load

  return <div>Getting token from authorization code: {code}</div>;
};
