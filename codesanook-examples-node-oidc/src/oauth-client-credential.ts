import oauth2orize from 'oauth2orize';
import passport from 'passport';
import jwt from 'jsonwebtoken';

const jwtSecret = 'your_jwt_secret';
// create OAuth 2.0 server
const server = oauth2orize.createServer();

//Client Credentials
// grant_type: client_credentials
server.exchange(
  oauth2orize.exchange.clientCredentials(function (client, scope, done) {

    const token = jwt.sign({ username: 'ponggun', email: 'admin@codesanook.com' }, jwtSecret, { algorithm: 'HS256', expiresIn: '2h' })
    return done(null, token);
  })
);


const token = [
  passport.authenticate(['clientPassword'], {
    session: false,
  }),
  server.token(),
  server.errorHandler(),
]

// token endpoint
export { token };
