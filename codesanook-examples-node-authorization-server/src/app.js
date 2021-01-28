import express from 'express';
import passport from 'passport';
import { AddressInfo } from 'net';
import user from './routes/user';
import client from './routes/client';
import bodyParser from 'body-parser';
import cookieParser from 'cookie-parser';
import 'regenerator-runtime/runtime';
import oauth from './routes/oauth';
import auth from './routes/auth';
import './passport';
import session from 'express-session';
import expressValidator from 'express-validator';
const app = express();
const jwtSecret = 'your_jwt_secret';

// https://stackoverflow.com/a/56095662/1872200
// You are not required to use passport.initialize() if you are not using sessions.

// To Extracting POST Data Content-Type: application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: true }));
// support parsing of application/json type post data
app.use(bodyParser.json());
app.use(cookieParser())

// session option https://stackoverflow.com/questions/28839532/node-js-session-error-express-session-deprecated
app.use(session({ secret: 'keyboard cat', resave: true, saveUninitialized: true }));

// Set a static folder for images
app.use(express.static('public'));
app.use(passport.initialize());
app.use(passport.session());

// https://github.com/express-validator/express-validator/issues/735
app.use(expressValidator());

// default folder of HTML template is views folder
app.set('view engine', 'ejs');

app.get('/', (_, res) => {
  res.render('index',
    {
      products:
        [
          { url: 'images/product-a.jpg' },
          { url: 'images/product-b.jpg' },
          { url: 'images/product-c.jpg' },
        ]
    }
  );
});

// Register user
app.use('/user', user);
//app.post('/user-profile', port.autho('jwt'),  user);

// Register client
app.use('/client', client);

// login
app.use('/auth', auth);

// oauth
app.use('/oauth', oauth);

const port = 3000;
const listener = app.listen(port, () => {
  const { port } = (listener.address())
  console.log(`Listening on port ${port}`);
});
