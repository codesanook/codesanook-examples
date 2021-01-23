import passport from 'passport';
import { Strategy } from 'passport-local';
const LocalStrategy = Strategy;

import passportJWT from "passport-jwt";
const JWTStrategy = passportJWT.Strategy;
const ExtractJWT = passportJWT.ExtractJwt;

const jwtSecret = 'your_jwt_secret';

// check user in storage
passport.use(new LocalStrategy({
  usernameField: 'email',
  passwordField: 'password'
},
  function (email, password, done) {
    console.log(`logged in with email ${email}, password ${password}`);
    //this one is typically a DB call. Assume that the returned user object is pre-formatted and ready for storing in JWT
    // login call this;
    const user = {
      email: 'admin@codesanook.com',
      username: 'ponggun',
    }
    return done(null, user, { message: 'Logged In Successfully' });
  }
));

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
