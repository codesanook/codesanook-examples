import AuthorizationCodeGrantType from './AuthorizationCodeGrantType';
import UserProfile from '../UserApi';
import Layout from '../Layout';
import RefreshToken from './RefreshToken';

export default function AuthorizationTestingFlow() {
  return (
    <Layout>
      <ul className='block-list'>
        <li>
          <AuthorizationCodeGrantType />
        </li>
        <li>
          <UserProfile />
        </li>
        <li>
          <RefreshToken />
        </li>
      </ul>
    </Layout>
  );
};
