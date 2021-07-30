// Helper functions for PKCE (Proof Key of Code Exchange) Extension for OAuth2
// see https://tools.ietf.org/html/rfc7636#section-4
import { createHash, randomBytes } from 'crypto';
import base64Url from 'base64url';

const codeVerifyKey = 'code_verifier';

export const generateCodeChallenge = (codeChallengeMethod: string) => {
  // generate a code verifier for PKCE extension of Auth Code Flow
  // see https://auth0.com/docs/api-auth/tutorials/authorization-code-grant-pkce
  // https://tools.ietf.org/html/rfc7636#section-4

// Code verify reference https://datatracker.ietf.org/doc/html/rfc7636#section-4.1
  const codeVerifier = base64Url(randomBytes(47));

  // store the codeVerifier as we'll need it later on when exchanging the auth code for a token
  console.log(`Save code verify ${codeVerifier} with length ${codeVerifier.length} to a local storage.`);
  localStorage.setItem(codeVerifyKey, codeVerifier);
  // Generate code challenge to be sent with authorization request

  // Code challenge is from base64URLEncode(sha256(verifier));
  // Code challenge reference https://datatracker.ietf.org/doc/html/rfc7636#section-4.2

  const codeChallenge = base64Url(
    createHash(codeChallengeMethod).update(codeVerifier).digest()
  );
  console.log(`code challenge value: ${codeChallenge}`);
  return codeChallenge;
};

export const getCodeVerifier = () => {
  // Obtain the stored code verifier (stored by generateCodeChallenge)
  const codeVerifier = localStorage.getItem(codeVerifyKey);

  if (codeVerifier === null) {
    throw new Error('No Code Verifier found');
  }

  return codeVerifier;
};
