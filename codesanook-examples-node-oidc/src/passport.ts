import passport from 'passport';
import jwt from 'jsonwebtoken';
import { BasicStrategy } from 'passport-http';
import { Strategy as ClientPasswordStrategy } from 'passport-oauth2-client-password'

import { Strategy as CookieStrategy } from 'passport-cookie'
// import { Strategy } from 'passport-local';

import passportJWT from "passport-jwt";
const jwtSecret = 'your_jwt_secret';

// check user in cookie
passport.use(new CookieStrategy({
  cookieName: 'auth-cookie',
  signed: false,
  passReqToCallback: true
},
  function (req, token, done) {
    console.log(`verifying with cookie token ${token}`);
    //this one is typically a DB call. Assume that the returned user object is pre-formatted and ready for storing in JWT
    // login call this;
    // invalid token - synchronous
    try {
      // use a defined interface
      const decoded: any = jwt.verify(token, jwtSecret);
      console.log(`decoded token ${JSON.stringify(decoded)}`);
      const user = {
        email: decoded.email,
        username: decoded.username,
      };
      return done(null, user, { message: 'Logged In Successfully' });
      // return done(null, false, {message: 'Incorrect email or password.'});

    } catch (err) {
      // err
      console.error(err);
      done(err);
    }
  }
));

const JWTStrategy = passportJWT.Strategy;
const ExtractJWT = passportJWT.ExtractJwt;

passport.use(new JWTStrategy({
  jwtFromRequest: ExtractJWT.fromAuthHeaderAsBearerToken(),
  secretOrKey: jwtSecret,
},
  function (jwtPayload, done) {

    //find the user in db if needed. This functionality may be omitted if you store everything you'll need in JWT payload.
    const user = { email: jwtPayload.email, username: jwtPayload.username }
    return done(null, user);
  }
));


/**
 * These strategies are used to authenticate registered OAuth clients.
 * The authentication data may be delivered using the basic authentication scheme (recommended)
 * or the client strategy, which means that the authentication data is in the body of the request.
 */
// id:secret base64 encode
passport.use("clientBasic", new BasicStrategy(
  function (clientId, clientSecret, done) {
    // register to database
    console.log('client basic');
    return done(null, { clientId: clientId });
  }
));

// client_id
// client_secret
passport.use("clientPassword", new ClientPasswordStrategy(
  function (clientId, clientSecret, done) {
    console.log('client password');
    return done(null, { clientId: clientId });
  }
));

// https://github.com/reneweb/oauth2orize_client_credentials_example/blob/master/auth.js
