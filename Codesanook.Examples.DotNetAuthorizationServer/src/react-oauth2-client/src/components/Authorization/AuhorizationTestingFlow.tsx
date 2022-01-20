import AuthorizationCodeGrantType from './AuthorizationCodeGrantType';
import UserApiWithTokenInterceptor from '../UserApiWithTokenInterceptor';
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
          <UserApiWithTokenInterceptor />
        </li>
        <li>
          <RefreshToken />
        </li>
      </ul>
    </Layout>
  );
};
