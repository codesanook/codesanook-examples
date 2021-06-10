import oauthService from './OauthService';
import axios from 'axios';

const loginInfo = {
  "auth_url": "https://localhost:5001/connect/authorize",
  "access_token_url": "https://localhost:5001/connect/token",
  "client_id": "react-spa",
  "state": "",
  "redirect_uri": "http://localhost:3000/authentication/login-callback",
  // "post_logout_redirect_uri": "https://localhost:5001/authentication/logout-callback",
  "scope": "api openid offline_access"
}

export const AuthenticationResultStatus = {
  Redirect: 'redirect',
  Success: 'success',
  Fail: 'fail'
};

class AuthorizationService {

  async signIn(state: any) {
    /*
        Contrust the request: https://localhost:5001/connect/authorize?
            response_type=code&
            state=&
            client_id=postman
            &scope=api openid offline_access&
            redirect_uri=https://oauth.pstmn.io/v1/callback
            &code_challenge=F6MFge9ixbYht1nQvu5s3gNbF5JB4A97j-4d_qUAY4A
            &code_challenge_method=S256
        */
    oauthService.init();
    const authParams: string = new URLSearchParams(this.createAuthParams(loginInfo)).toString();
    window.location.href = `${loginInfo.auth_url}?${authParams}`;
  }

  async completeSignIn(url: string) {
    let params = (new URL(url)).searchParams;
    if (!params.has("code")) return false;
    const code = params.get("code") || "";

    const response = await axios.post(loginInfo.access_token_url, this.createAccessTokenParams(loginInfo, code), {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    });
    console.log(response.data);
  }

  createAccessTokenParams({ redirect_uri, client_id, scope }: any, code: string) {
    console.log(`Call: ${oauthService.getCodeVerifier()}`);
    const params = {
      grant_type: 'authorization_code',
      code,
      redirect_uri,
      client_id,
      code_verifier: oauthService.getCodeVerifier(),
      scope
    }
    return new URLSearchParams(params).toString();
  }

  createAuthParams({ state, client_id, redirect_uri, scope }: any) {
    return {
      response_type: "code",
      state,
      client_id,
      scope,
      redirect_uri,
      code_challenge: oauthService.getCodeChallenge(),
      code_challenge_method: "S256"
    };
  }

  error(message: string) {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  success(state: any) {
    return { status: AuthenticationResultStatus.Success, state };
  }

  redirect() {
    return { status: AuthenticationResultStatus.Redirect };
  }
}

const authService = new AuthorizationService();

export default authService;
