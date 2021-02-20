//routes/user.js
import db from '../db';
import { Router } from 'express';
import { hash } from 'bcrypt';
import { permissionAuthenticate } from '../passport';

/* GET users listing. */
const router = Router();

router.post('/', permissionAuthenticate('report-user'), (req, res) => {
  console.log(req.user);
  res.json(req.user);
});

/* GET user profile. */
router.get('/profile', (req, res) => {
  res.send(req.user);
});

export function register(req, res) {
  // This requires express-validator
  req.checkBody('username', 'No valid username is given').notEmpty().len(3, 40)
  req.checkBody('email', 'No valid email is given').notEmpty().len(6, 50)
  req.checkBody('password', 'No valid password is given').notEmpty().len(5, 50)

  const errors = req.validationErrors();
  if (errors) {
    res.send(errors, 400);
  } else {
    const email = req.body['email']
    const username = req.body['username']
    const password = req.body['password']

    db.collection('users').findOne({ email: email }, function (err, user) {
      if (user) {
        res.send('Email is already taken', 422);
      } else {
        hash(password, 11, function (err, hash) {
          db.collection('users').save({ username: username, email: email, password: hash }, (err, savedUser) => {
            res.send(savedUser, 201);
          });
        });
      }
    })
  }
}

router.post('/register', register);

export default router;
