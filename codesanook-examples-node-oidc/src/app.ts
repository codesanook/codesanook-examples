import './passport';
import express from 'express';
import passport from 'passport';

import auth from './routes/auth';
import user from './routes/user';

const app = express();
const port = 3000;

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

app.listen(port, () => {
  console.log(`The app listening at http://localhost:${port}`)
});
