import { hashCodeVerify, uid } from './utils';

const codeVerifier = uid(43);
const codeChallenge = hashCodeVerify(codeVerifier, 'sha256')
console.log(`codeVerify ${codeVerifier}`);
console.log(`codeChallenge ${codeChallenge}`);
