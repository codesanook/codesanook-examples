import { useEffect, useState } from 'react';
import axios from 'axios';
import { useHistory, useLocation } from "react-router-dom";
import { getCodeVerifier } from './PKCE';

export default function AuthorizationCallback() {
  const [code, setCode] = useState('');
  const history = useHistory();
  const location = useLocation()

  useEffect(() => {
    const getToken = async () => {
      console.log(JSON.stringify(location, null, 2));

      const searchParams = new URLSearchParams(location.search.substring(1)); // substring(1) to remove ? character
      const authorizationCode = searchParams.get('code') as string;
      //const params = queryString.parse(location.search);

      // Set AuthorizationCode
      setCode(authorizationCode);

      const config = {
        headers: {
          // We need to use form-url encode https://github.com/openiddict/openiddict-core/issues/437
          // https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.3
          'Content-Type': 'application/x-www-form-urlencoded'
        }
      }

      // Axios is able to accept a URLSearchParams instance which also set the appropriate Content-type header to application/x-www-form-urlencoded
      const parameters: Record<string, string> = {
        grant_type: 'authorization_code',
        client_id: process.env.REACT_APP_CLIENT_ID as string,
        redirect_uri: process.env.REACT_APP_REDIRECT_URI as string,
        code: authorizationCode,
        code_verifier: getCodeVerifier(),
      };

      // Request token
      const response = await axios.post(
        process.env.REACT_APP_TOKEN_ENDPOINT as string,
        new URLSearchParams(parameters),
        config,
      );

      alert('Got token and set to ');
      // Set token to local storage
      localStorage.setItem('access_token', response.data.access_token);
      localStorage.setItem('refresh_token', response.data.refresh_token);

      // Back to home page root
      history.push('/');
    };

    getToken();
    // https://stackoverflow.com/a/55854902/1872200
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []); // run one time on load

  return <div>Getting token from authorization code: {code}</div>;
};
