import passport from 'passport';
import jwt from 'jsonwebtoken';
import { BasicStrategy } from 'passport-http';
import { Strategy as ClientPasswordStrategy } from 'passport-oauth2-client-password';
import { Strategy as LocalStrategy } from 'passport-local';
import { Strategy as CookieStrategy } from 'passport-cookie'

import db from './db';
import { compare } from 'bcrypt';

import { Strategy as JwtStrategy, ExtractJwt } from "passport-jwt";
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

/**
 * These strategies are used to authenticate registered OAuth clients.
 * The authentication data may be delivered using the basic authentication scheme (recommended)
 * or the client strategy, which means that the authentication data is in the body of the request.
 */
// header base64 client_id:client_secret
// 'Basic Ym9iOnNlY3JldA=='
passport.use(
  'clientBasic',
  new BasicStrategy(function (clientId, clientSecret, done) {
    console.log('checking with client basic')
    db.collection('clients').findOne({ clientId: clientId }, function (err, client) {
      console.log('getting client');
      if (err) return done(err);
      if (!client) {
        return done(null, false);
      }

      if (client.clientSecret == clientSecret) {

        console.log('Return client');
        return done(null, client);
      }
      else {
        return done(null, false);
      }
    });
  })
);


// client_id
// client_secret
passport.use("clientPassword", new ClientPasswordStrategy(
  function (clientId, clientSecret, done) {
    console.log('client password');
    return done(null, { clientId: clientId });
  }
));

// https://github.com/reneweb/oauth2orize_client_credentials_example/blob/master/auth.js
/**
* LocalStrategy
*/
passport.use(new LocalStrategy(
  {
    usernameField: 'email',
    passwordField: 'password',
    session: false
  },
  function (email, password, done) {
    db.collection('users').findOne({ email: email }, function (err, user) {
      if (err) {
        return done(err);
      }

      if (!user) {
        return done(null, false);
      }

      compare(password, user.password, function (err, res) {
        if (!res) return done(null, false)
        return done(null, user);
      });

    });
  }
));

passport.use(new JwtStrategy({
  jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
  secretOrKey: jwtSecret,
},
  (jwtPayload, done) => {
    console.log(`jwtPayload ${JSON.stringify(jwtPayload)}`);

    //find the user in db if needed. This functionality may be omitted if you store everything you'll need in JWT payload.
    const user = { id: jwtPayload.id, roles: jwtPayload.roles };
    return done(null, user);
  }
));

const permissionAuthenticate = (permission: string) => (req, res, next) => {
  if (!permission) {
    res.status(500).json({ errorMessage: 'You need to specify a permission when using permissionAuthenticate' });
    return;
  }

  passport.authenticate('jwt', { session: false }, (error, user) => {
    if (error || !user) {
      res.status(401).json(error);
      return;
    }

    console.log(`Checking if user has ${permission} permission`);
    const rolePermission = [
      {
        name: 'admin',
        permissions: [
          'list-user',
          'read-user',
          'edit-user',
          'delete-user',
        ],
      },
      {
        name: 'editor',
        permissions: [
          'list-user',
          'read-user',
          'report-user',
        ],
      },
    ];

    const requiredPermission = permission.toUpperCase();
    const rolesHavRequiredPermission = rolePermission
      .filter(rp => rp.permissions.some(p => p.toUpperCase() === requiredPermission))
      .map(rp => rp.name.toUpperCase());

    console.log(JSON.stringify(rolesHavRequiredPermission, null, 2));
    const userRoles = user.roles.map(r => r.toUpperCase());

    const isUserHasPermission = rolesHavRequiredPermission.some(r => userRoles.includes(r));
    console.log(`isUserHasPermission ${isUserHasPermission}`);
    if(!isUserHasPermission){
      const errorMessage = `A user with role '${user.roles.join(', ')}' does not have '${permission}' permission.`;
      res.status(401).json({errorMessage});
      return;
    }

    // We need to login user manually when we handle authorize by ourselves
    req.login(user, { session: false }, err => {
      if (err) {
        res.status(401).send(error);
      }
      console.log('user already logged in');
      next();
    });

  })(req, res, next);
};

export { permissionAuthenticate };
