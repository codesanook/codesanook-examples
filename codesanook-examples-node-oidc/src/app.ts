import express from 'express';
import passport from 'passport';
import auth from './routes/auth';
import user from './routes/user';
import bodyParser from 'body-parser';
import cookieParser from 'cookie-parser';

import './passport';
import { Provider } from 'oidc-provider';

const app = express();
const port = 3000;

// To Extracting POST Data Content-Type: application/x-www-form-urlencoded
app.use(bodyParser.urlencoded({ extended: true }));
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

app.use('/auth', auth);
app.use('/user', passport.authenticate('jwt', { session: false }), user);

const configuration = {
  // ... see available options /docs
  clients: [{
    client_id: 'foo',
    client_secret: 'bar',
    redirect_uris: [],
    response_types: [],
    grant_types: ['client_credentials'],
  }],
};

//const oidc = new Provider('http://localhost:3000', configuration);
// express/nodejs style application callback (req, res, next) for use with express apps, see /examples/express.js

//app.use('/oidc', oidc.callback);

app.listen(port, () => {
  console.log(`The app listening at http://localhost:${port}`)
});
