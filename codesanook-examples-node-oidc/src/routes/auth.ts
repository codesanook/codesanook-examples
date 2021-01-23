import { Router } from 'express'
import jwt from 'jsonwebtoken';
import passport from 'passport';

const jwtSecret = 'your_jwt_secret';

/* POST login. */

const router = Router();
// http://www.passportjs.org/docs/authenticate/
router.post('/login', function (req, res, next) {
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
});

export default router;
