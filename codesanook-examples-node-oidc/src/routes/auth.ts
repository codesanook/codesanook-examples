import { Router } from 'express'
import jwt from 'jsonwebtoken';
import passport from 'passport';

const jwtSecret = 'your_jwt_secret';

/* POST login. */

const router = Router();
router.post('/login', function (req, res, next) {

  //get username from request's body, eg. from login form
  const email = req.body.email;
  const password = req.body.password;
  /*
  check username and password correctness here,
  if they matched then:
  */
  //create jwt token
  const token = jwt.sign({ username: 'ponggun', email: email }, jwtSecret)

  //save token in cookie
  // set httpOnly: false for simplifying localhost 
  res.cookie('auth-cookie', token, { maxAge: 900000, httpOnly: false });
  // signed cookie https://stackoverflow.com/a/11898142/1872200
  // res.cookie('name', 'value', {signed: true})

  return res.json({ token });
  // res.redirect('/');

});


// http://www.passportjs.org/docs/authenticate/
router.get('/test-cookie', function (req, res, next) {
  passport.authenticate('cookie', (err, user, info) => {

    // user is return from the storage
    if (err || !user) {
      return res.status(400).json({
        message: 'Something is not right',
        user: user
      });
    }

    req.login(user, { session: false }, (err) => {
      if (err) {
        res.send(err);
      }
      console.log('user already logged in, valid cookie token');
      return res.json(user);
    });

  })(req, res, next);
});

export default router;



/*
  passport.authenticate('local', { session: false }, (err, user, info) => {
    console.log(`error ${JSON.stringify(err, null, 2)}`);
    console.log(`user ${JSON.stringify(user, null, 2)}`);

    // user is return from the storage
    if (err || !user) {
      return res.status(400).json({
        message: 'Something is not right',
        user: user
      });
    }

    req.login(user, { session: false }, (err) => {
      if (err) {
        res.send(err);
      }
      // generate a signed son web token with the contents of user object and return it in the response
      const token = jwt.sign(user, jwtSecret);
      return res.json({ token });
    });
  })(req, res);
*/
