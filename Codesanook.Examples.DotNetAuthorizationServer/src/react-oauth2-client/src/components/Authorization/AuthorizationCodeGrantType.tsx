import { useState, useEffect } from 'react';
import { generateCodeChallenge } from './PKCE';

const AuthorizationCodeGrantType = () => {
  const [actionUrl, setActionUrl] = useState('');

  useEffect(() => {
    const codeChallengeMethod = 'sha256';
    const codeChallenge = generateCodeChallenge(codeChallengeMethod);
    console.log(`Before sending code challenge value: ${codeChallenge}`);

    const parameters: Record<string, string> = {
      client_id: process.env.REACT_APP_CLIENT_ID as string,
      redirect_uri: process.env.REACT_APP_REDIRECT_URI as string,
      code_challenge: codeChallenge,
      code_challenge_method: 'S256',
      response_type: 'code',
      scope: 'api offline_access' // set offline_access for getting refresh token in response body
    };

    // https://stackoverflow.com/a/44609277/1872200
    const actionUrl = new URL(process.env.REACT_APP_AUTHORIZATION_ENDPOINT as string);
    Object.keys(parameters).forEach(key => actionUrl.searchParams.append(key, parameters[key]));
    setActionUrl(actionUrl.toString());

  }, []);

  return (
    <div className='block-list-item'>
      <a href={actionUrl}>Get access token and refresh token with authorization code flow</a>
    </div>
  );
};

export default AuthorizationCodeGrantType;
