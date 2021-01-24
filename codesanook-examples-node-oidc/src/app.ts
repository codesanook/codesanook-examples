import express from 'express';
import passport from 'passport';
import user from './routes/user';
import bodyParser from 'body-parser';
import cookieParser from 'cookie-parser';
import 'regenerator-runtime/runtime';
import { token } from './oauth-client-credential';
import './passport';

const app = express();
const port = 3000;
const jwtSecret = 'your_jwt_secret';


// https://stackoverflow.com/a/56095662/1872200
// You are not required to use passport.initialize() if you are not using sessions.


// To Extracting POST Data Content-Type: application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: true }));
// support parsing of application/json type post data
app.use(bodyParser.json());
app.use(cookieParser())

// Set a static folder for images
app.use(express.static('public'));

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

app.post('/oauth/token', token)
app.use('/user', passport.authenticate('jwt', { session: false }), user);

app.listen(port, () => {
  console.log('oidc-provider listening on port 3000, check http://localhost:3000/.well-known/openid-configuration');
});
