// Ref: https://gist.github.com/tonyxu-io/21eb57ab2a4aeb2a3ee10f77542abe64
import CryptoJS from "crypto-js";
// import randomBytes from "randombytes";
// import LocalStorage from "../../utils/LocalStorage";
import { differenceInSeconds } from 'date-fns';

const CodeVerifierKey = "code_verifier";
const CodeVerifierTimeoutKey = "code_verifier_timeout";
const CodeVerifierTimeout = 3; // seconds

class OauthService {

  init(){
    if(this.isCodeVerifierExpired()){
        localStorage.setItem(CodeVerifierKey, this.generateCodeVerifier());
        localStorage.setItem(CodeVerifierTimeoutKey, new Date().toString());
        console.log(`init codeVerifier: ${this.getCodeVerifier()}`);
        console.log("Code Verifier Expired");
        return;
    }
    console.log("Using the existing Code Verifier");
  }

  isCodeVerifierExpired(){
    const currentTimeout = new Date(localStorage.getItem(CodeVerifierTimeoutKey) || new Date().toString());
    if(differenceInSeconds(new Date(), currentTimeout) > CodeVerifierTimeout) return true;
    return false;
  }

  generateCodeVerifier() {
    return this.generateRandomString(128);
  }

  generateRandomString(length: number) {
    var text = "";
    var possible =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
    for (var i = 0; i < length; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  }

  generateCodeChallenge(code_verifier: string) {
    return this.base64URL(CryptoJS.SHA256(code_verifier));
  }

  base64URL(text: CryptoJS.lib.WordArray) {
    return CryptoJS.enc.Base64.stringify(text)
      .replace(/=/g, "")
      .replace(/\+/g, "-")
      .replace(/\//g, "_");
  }

  getCodeChallenge() {
    return this.generateCodeChallenge(this.getCodeVerifier());
  }

  getCodeVerifier() {
    return localStorage.getItem(CodeVerifierKey) || "";
  }

}

const oauthService = new OauthService();

export default oauthService;
