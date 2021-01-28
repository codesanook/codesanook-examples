import oauth2orize from 'oauth2orize';
import passport from 'passport';
import jwt from 'jsonwebtoken';
import { hashCodeVerify, uid } from '../utils';
import * as pkce from 'oauth2orize-pkce';
import { createHash } from 'crypto';
import db from '../db';
import { Router } from 'express'

const router = Router();
const jwtSecret = 'your_jwt_secret';
// create OAuth 2.0 server
const server = oauth2orize.createServer();
server.grant(pkce.extensions());

//(De-)Serialization for clients
server.serializeClient(function (client, done) {
  return done(null, client.clientId)
})

server.deserializeClient(function (id, done) {
  db.collection('clients').find({ clientId: id }, function (err, client) {
    if (err) return done(err)
    return done(null, client)
  })
})

//Register grant (used to issue authorization codes)
// 


// source code https://github.com/jaredhanson/oauth2orize/blob/master/lib/grant/code.js
// `ares` is any additional * parameters parsed from the user's decision, including scope, duration of * access, etc.
// export type IssueGrantCodeFunction = (client: any, redirectUri: string, user: any, res: any, issued: (err: Error | null, code?: string) => void) => void;

server.grant(oauth2orize.grant.code(function (client, redirectURI, user, ares, res, done) {

  console.log(`res ${JSON.stringify(res)}`)
  console.log(`ares ${JSON.stringify(ares)}`)
  console.log('granting with code');
  console.log(`codeChallenge ${res.codeChallenge}`);
  console.log(`codeMethod ${res.codeChallengeMethod}`);

  const code = uid(16);
  console.log(`code ${code}`);
  const codeHash = createHash('sha1').update(code).digest('hex')
  console.log(`client ${JSON.stringify(client, null, 2)}`);
  console.log(`user ${JSON.stringify(user, null, 2)}`);

  db.collection('authorizationCodes').save(
    {
      code: codeHash,
      clientId: client.clientId, // client is from server.authorization done
      redirectURI: redirectURI,
      userId: user.username,
      codeChallenge: res.codeChallenge,
      codeChallengeMethod: res.codeChallengeMethod
    }, function (err) {
      if (err) return done(err);
      console.log('new authorizationCodes save');
      done(null, code);
    });
}));

//Used to exchange authorization codes for access token
server.exchange(oauth2orize.exchange.code(function (client, code, redirectURI, res, done) {
  console.log(`code ${code}`);
  console.log(`redirectURI ${redirectURI}`);
  console.log(`requestBody ${JSON.stringify(res, null, 2)}`);

  const codeVerifier = res.code_verifier;
  if (!codeVerifier) {
    return done(null, false);
  }

  var codeHash = createHash("sha1").update(code).digest("hex");
  db.collection("authorizationCodes").findOne(
    { code: codeHash },
    function (err, authCode) {

      if (!authCode) {
        return done(null, false);
      }

      console.log(`authCode ${JSON.stringify(authCode, null, 2)}`);
      const codeChallengeFromCodeVerify = hashCodeVerify(codeVerifier, authCode.codeChallengeMethod);

      console.log(`authCode.codeChallenge ${authCode.codeChallenge}`);
      console.log(`codeChallengeFromCodeVerify ${codeChallengeFromCodeVerify}`);
      if (authCode.codeChallenge !== codeChallengeFromCodeVerify) {
        console.log('invalid codeVerifier');
        return done(null, false);
      }

      if (err) {
        done(err);
      }

      if (!authCode) {
        return done(null, false);
      }

      console.log(`client ${JSON.stringify(client, null, 2)}`);
      if (client.clientId !== authCode.clientId) {
        return done(null, false);
      }
      if (redirectURI !== authCode.redirectURI) {
        return done(null, false);
      }

      db.collection("authorizationCodes").remove({ code: codeHash }, function (err) {
        if (err) return done(err);
        const accessToken = jwt.sign({ clientId: client.clientId }, jwtSecret, { algorithm: 'HS256', expiresIn: '12h' })
        const refreshToken = jwt.sign({ clientId: client.clientId }, jwtSecret, { algorithm: 'HS256', expiresIn: '14d' })
        done(null, accessToken, refreshToken);
      });
    }
  );
}));

//Refresh Token
server.exchange(oauth2orize.exchange.refreshToken(function (client, refreshToken, scope, done) {
  try {

    console.log(`getting access token`);
    const token: any = jwt.verify(refreshToken, jwtSecret);
    console.log(`decoded refresh token ${JSON.stringify(token)}`);

    if (!token) {
      done(null, false);
    }

    console.log(`clent.clientId ${client.clientId}, token.clientId ${token.clientId}`);
    if (client.clientId !== token.clientId) {
      done(null, false);
    }

    const newAccessToken = jwt.sign({ clientId: client.clientId }, jwtSecret, { algorithm: 'HS256', expiresIn: '2h' })
    const newRefreshToken = jwt.sign({ clientId: client.clientId }, jwtSecret, { algorithm: 'HS256', expiresIn: '14d' })
    done(null, newAccessToken, newRefreshToken);
  } catch (err) {
    done(err);
  }
}));

//Client Credentials
// grant_type: client_credentials
server.exchange(
  oauth2orize.exchange.clientCredentials(function (client, scope, done) {

    const token = jwt.sign({ username: 'ponggun', email: 'admin@codesanook.com' }, jwtSecret, { algorithm: 'HS256', expiresIn: '2h' })
    return done(null, token);
  })
);

// All endpoints
const cookieAuth = function (req, res, next) {
  passport.authenticate('cookie', { session: false }, function (error, user, info) {
    console.log('checking cookie')

    if (error) {
      res.status(401).send(error);
    } else if (!user) {
      res.status(401).send(info);
    } else {

      req.login(user, { session: false }, (err) => {
        if (err) {
          res.status(401).send(error);
        }
        console.log('user already logged in, valid cookie token');
        next();
      });
    }
  })(req, res, next);
};

// user authorization endpoint
const authorization = [
  cookieAuth,
  server.authorization(function (clientId, redirectURI, done) {
    console.log(`finding client with id ${clientId}`);
    db.collection('clients').findOne({ clientId: clientId }, function (err, client) {
      if (err) return done(err)
      // WARNING: For security purposes, it is highly advisable to check that
      // redirectURI provided by the client matches one registered with
      // the server. For simplicity, this example does not. 
      // You have been warned.
      return done(null, client, redirectURI)
    })
  }),
  function (req, res, next) {
    // res.render("decision", {
    //   transactionID: req.oauth2.transactionID,
    //   user: req.user,
    //   client: req.oauth2.client,
    // });
    // It only requires transaction_id in body to accept decision 
    req.body.transaction_id = req.oauth2.transactionID;
    console.log(`query ${JSON.stringify(req.query, null, 2)}`);
    next();
  },
  server.decision(), // Call oauth2orize.grant.code to get a code in URL
];

// token endpoint
const token = [
  passport.authenticate(['clientBasic'], { session: false }),
  server.token(),
  server.errorHandler(),
];

// // user decision endpoint
// const decision = [
//   function (req, res, next) {
//     if (req.user) next()
//     else res.redirect('/oauth/authorization')
//   },
//   server.decision()
// ];

// authorization?client_id=RpBa1XAw&redirect_uri=http://localhost:3000&response_type=code
router.post('/authorization', authorization);
router.post('/token', token);

export default router;

