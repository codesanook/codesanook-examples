import oauthService from './OauthService';
import axios from 'axios';


import { LocalStorageConfigs } from "./constant";
const { AccessTokenKey, RefreshTokenKey } = LocalStorageConfigs.Token;


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
     /*
    Return `response.data`
      access_token: "eyJhbGciOiJIUzI1NiIsInR5cCI6ImF0K2p3dCJ9.eyJzdWIiOiJlcmciLCJzb21lIGNsYWltIjoic29tZSB2YWx1ZSIsIm9pX3Byc3QiOiJyZWFjdC1zcGEiLCJvaV9hdV9pZCI6IjJlMDc4Y2I0LWUyM2QtNDIxMi05OGQ4LWQzODZmYWM5NWI4OCIsImNsaWVudF9pZCI6InJlYWN0LXNwYSIsIm9pX3Rrbl9pZCI6Ijk3ZTk3NDZhLWZlZmMtNDIwYy04YzU2LWRmNGE1MjhjZmEwZSIsInNjb3BlIjoiYXBpIG9wZW5pZCBvZmZsaW5lX2FjY2VzcyIsImV4cCI6MTYyMzI5MDcyMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8iLCJpYXQiOjE2MjMyODg5MjB9.C5h3kdncXFOTvFjNUr3sjk07cgEqh3jEsrWXKTCAapM"
      expires_in: 1799
      id_token: "eyJhbGciOiJSUzI1NiIsImtpZCI6IjNJT1hCUFVROThJSzIwMFJQSFBSUEVFMURaVkFYWVQ5RU8tWFVER1MiLCJ0eXAiOiJKV1QifQ.eyJzdWIiOiJlcmciLCJlbWFpbCI6InNvbWVAZW1haWwiLCJvaV9hdV9pZCI6IjJlMDc4Y2I0LWUyM2QtNDIxMi05OGQ4LWQzODZmYWM5NWI4OCIsImF6cCI6InJlYWN0LXNwYSIsImF0X2hhc2giOiJobThhUkRpV2xLR0RuTnlhbTgteDN3Iiwib2lfdGtuX2lkIjoiZmFmMmJiZTEtMDAwNy00YWE4LWFjNDgtYTU2YmI4ZmI4Zjk3IiwiYXVkIjoicmVhY3Qtc3BhIiwiZXhwIjoxNjIzMjkwMTIwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxLyIsImlhdCI6MTYyMzI4ODkyMH0.LEltKUqAmCigD2XhLO2bILoRhd1jeKikK_XaYIvfdYve-HwVJ7KJOxHdxUJotJ_PIoTJACkh4Y8mNh9TPFonZ91qV7PRWQjEvlzYj6YoyM0IOHKi23MOLCxNkKfYvg_SVClzL4xnIcQqDn0SeRVjylSgJQE9zqDfCsnXxQLj_POSg7baQpKoMp7LETgD148DL1TwI-c-lCFW0qKCFpScbm3NRLtfklRXKWCPu7q63jQC9_CBQ6_ujcRMD4qYNYR2UPiNJRQ1l3hNaCw18Z3esk4n5YtmQZH_9hZxpf8f-wkEsHhv2G9PaKhC1V-FbdZWNnEGPeEJg72nGFyzstPLSA"
      refresh_token: "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZDQkMtSFM1MTIiLCJraWQiOiJZTTBDT1pVRzJBQVNKU0RVSzdCQUxTM1BXRVFRM0tVRTFaSF9aR1gyIiwidHlwIjoib2lfcmVmdCtqd3QifQ.Y9vDkHtydcDze95QmkhelabjeWadyb5-Y28F0KkSbvkz4B6UY2fXTVopy1bGdE1FTTyVQeTB_0wcFpPyKyBixCUTDZ6TdGl5V-mfONfipGQAA08OkLC5c1bj8pqrKjZ43sQ0-Y03VA1-YSKity4OWUrwu4iT96uPWG9xmzplG5k0EhcLUwG0w0zGo8d2trtV-LJY7r2Ma-iXKDPpXOJksdhdxBmgJlESlJEgECcH06HDCywGfohcN8SsGmENuw78HEQXoCjYclkXztu5a7_poH1mgooRICTEuT_u_nqgFxr0gna9I_ovv23UNFvAEUdkz1-4fo50D7lIdHV75KezMw.RBCrxPX9yro-j6VrlBUNNQ.Nsjga5qxV9L9kFz8UXRRX9KyBh-6ug9G-DMfkIjvQZt472r4GBLd02i8mV2psmRoueon9l2Cg4ghnOLvR_03tzGd9PA4d9oUBPO0bifOIL_XYIycM7PMiac-P1eA3zbGrShrnTSBFo2Th-jort6OAX_mDuO2XrLiMhFKWKkuKfByGfH5wfCOGPglgQdf8Cxrs1lwnc9PuaNYpLlnJGbtgzdit9EQuR5FPaPZh9f9hN5L36xiQ28bOsEEBWZ9d-rqfNXAdpNXYCeX-QnrV5uGaBQrZjjtJPFm1zSQfcR5fghUNXwIAQZsbTI0Ac8Q_uoZZQ4-ztQU5ipoxU-foGYKQxKqyyWGJ59bnfAU2a6YUnxVvFvEnYc_rgSWp8qp9mv4ruSNjcOUbP-6Eq2Hd7wvW91Ym3ciWFvb0svF5EdXJ-D2X1YdsZLGDmIZymh-3IEBv3zxc2DYHextnlv7C9BMJiY0G0bMXNqIZv9F7JofhMCP5EFLR12X75hPlcsGdT6nRT3Qlw-Bstm6MM-s8zf_TEwhCD74xvEL8qCb6yY1bCmaX5BQH5jV9sQj7DLWIEQPa-btNX-H4427mdvMA49nmj34v6al1XToTR-jpCj9_WUsYK-Vc3rks_f2YpV_GxdDKlM_AZK-uT-YB5ZP2VYqw_fd27xk1Ns5AHzNUsf64FiDJqNMf4v8mF7yfUf4J0CFwcdcrQsGxKfx4emOY20qbIzR9DhV9_UPHVmcxkgjpvsoIbCd_aD353m2DZMjsJDyVqo9BrTtJdza_ADXdmF6T1kMBqz0Q4mqtDLBFUQ5ZYA5IkPvkPQtFkKrI02YjtlRGNLJoZ8dtKS8FgA67u812w1C1eOYJIBFmG9uH-kbZNPrI_KIJ2XtWJkxjA8F4yb1QmUT3eayIHsunw7yllYJvmzU7UgdHhFop9Ig31oUDgSXEZDTmpTFc5jOMt8iaqtAKtYdCJNJ-JwTiSugvra2aEwHpDdu5XKkPpy6llJVRfZgjw0RP2_6Z6tzc7i97yBdnF1QDCBEJ3joE7ImUte7eUMqjJK8mig7cUYMSjCPyqg.c5u0EnFSoD9UA0-EDfajTCszr4p2R4Kddt2-7F7bUtA"
      scope: "api openid offline_access"
      token_type: "Bearer"
    */
    // Saved tokens
    localStorage.setItem(AccessTokenKey, response.data.access_token);
    localStorage.setItem(RefreshTokenKey, response.data.refresh_token);
  }

  getAccessToken(){
    return localStorage.getItem(AccessTokenKey);
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
