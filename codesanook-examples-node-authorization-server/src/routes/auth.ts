import { Router } from 'express';
import jwt from 'jsonwebtoken';
import passport from 'passport';

const jwtSecret = 'your_jwt_secret';

/* POST login. */
const router = Router();

// wrap passport.authenticate call in a middleware function
const localAuth = function (req, res, next) {
  console.log('login');
  passport.authenticate('local', function (error, user, info) {
    if (error) {
      res.status(401).send(error);
    } else if (!user) {
      res.status(401).send(info);
    } else {
      req.login(user, { session: false }, (err) => {
        if (err) {
          res.status(401).send(error);
        }
        console.log('user has already logged in');
        next();
      });
    }
  })(req, res, next);
};

router.post('/login', localAuth, function (req, res) {
  //get username from request's body, eg. from login form
  const user = req.user;
  /*
  check username and password correctness here,
  if they matched then:
  */
  //create jwt token
  const token = jwt.sign({ username: user.username, email: user.email }, jwtSecret)
  //save token in cookie
  // set httpOnly: false for simplifying localhost 
  res.cookie('auth-cookie', token, { maxAge: 900000, httpOnly: false });
  // What is signed cookie https://stackoverflow.com/a/11898142/1872200
  // res.cookie('name', 'value', {signed: true})
  return res.json({ token });
});


// wrap passport.authenticate call in a middleware function
const cookieAuth = function (req, res, next) {
  passport.authenticate('cookie', function (error, user, info) {
    // this will execute in any case, even if a passport strategy will find an error
    // log everything to console
    console.log(error);
    console.log(user);
    console.log(info);

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

// http://www.passportjs.org/docs/authenticate/
router.get('/test-cookie', cookieAuth, (req, res) => {
  console.log('About to return JSON to client');
  res.json(req.user);
});

export default router;




