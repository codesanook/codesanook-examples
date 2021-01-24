import passport from 'passport';
import jwt from 'jsonwebtoken';

import { Strategy } from 'passport-cookie'
// import { Strategy } from 'passport-local';
const CookieStrategy = Strategy;

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
