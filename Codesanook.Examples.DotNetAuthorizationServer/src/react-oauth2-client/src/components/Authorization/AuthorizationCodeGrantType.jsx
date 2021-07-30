import React, { useState, useEffect } from 'react';
import { generateCodeChallenge } from './pkce';

const AuthorizationCodeGrantType = () => {
  const [actionUrl, setActionUrl] = useState('');

  useEffect(() => {
    const codeChallengeMethod = 'sha256';
    const codeChallenge = generateCodeChallenge(codeChallengeMethod);
    console.log(`Before sending code challenge value: ${codeChallenge}`);

    const parameters = {
      client_id: process.env.REACT_APP_CLIENT_ID,
      redirect_uri: process.env.REACT_APP_REDIRECT_URI,
      code_challenge: codeChallenge,
      code_challenge_method: 'S256',
      response_type: 'code',
    };

// https://stackoverflow.com/a/44609277/1872200
    const actionUrl = new URL(process.env.REACT_APP_AUTHORIZATION_ENDPOINT);
    Object.keys(parameters).forEach(key => actionUrl.searchParams.append(key, parameters[key]));
    setActionUrl(actionUrl.toString());

  }, []);

  return (
    <div>
      <a href={actionUrl}>Log in</a>
    </div>
  );
};

export default AuthorizationCodeGrantType;
