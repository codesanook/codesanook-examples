import oauth2orize from 'oauth2orize';
import passport from 'passport';
import jwt from 'jsonwebtoken';
import { uid } from '../utils';
import * as pkce from 'oauth2orize-pkce';
import { createHash } from 'crypto';
import db from '../db';
import { Router } from 'express'

const router = Router();
const jwtSecret = 'your_jwt_secret';
// create OAuth 2.0 server
const server = oauth2orize.createServer();

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

// server.grant(pkce.extensions());

//Register grant (used to issue authorization codes)
server.grant(oauth2orize.grant.code(function (client, redirectURI, user, ares, done) {
  console.log('granting with code');
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
    }, function (err) {
      if (err) return done(err)
      done(null, code)
    });
}));

//Used to exchange authorization codes for access token
server.exchange(oauth2orize.exchange.code(function (client, code, redirectURI, done) {
  console.log(`code ${code}`);
  console.log(`redirectURI ${redirectURI}`);

  var codeHash = createHash("sha1").update(code).digest("hex");
  db.collection("authorizationCodes").findOne(
    { code: codeHash },
    function (err, authCode) {
      console.log(`authCode ${JSON.stringify(authCode, null, 2)}`);

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

      db.collection("authorizationCodes").remove({ code: code }, function (err) {
        if (err) return done(err);
        const accessToken = jwt.sign({ clientId: client.clientId }, jwtSecret, { algorithm: 'HS256', expiresIn: '12h' })
        const refreshToken = jwt.sign({ clientId: client.clientId }, jwtSecret, { algorithm: 'HS256', expiresIn: '14d' })
        done(null, accessToken, refreshToken);
      });
    }
  );
}))

//Refresh Token
server.exchange(oauth2orize.exchange.refreshToken(function (client, refreshToken, scope, done) {
  try {
    const token: any = jwt.verify(refreshToken, jwtSecret);
    console.log(`decoded refresh token ${JSON.stringify(token)}`);

    if (!token) {
      done(null, false);
    }

    console.log(`clent.cliendId ${client.clientId}, token.clientId ${token.clientId}`);
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
