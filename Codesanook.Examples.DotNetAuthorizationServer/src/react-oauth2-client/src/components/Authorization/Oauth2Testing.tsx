import React from "react";
import ClientRegister from "./ClientRegistration";
import AuthorizationCodeGrantType from "./AuthorizationCodeGrantType";
import UserProfile from "./UserProfile";
import { Layout } from "../Layout";
const apiRoot = "";

export default function Oauth2Testing() {
  return (
    <Layout>
      <h2>1</h2>
      <ClientRegister />
      <br />
      <h2>2</h2>
      <AuthorizationCodeGrantType />
      <br />
      <br />
      <h2>3</h2>
      <UserProfile />
    </Layout>
  );
};
