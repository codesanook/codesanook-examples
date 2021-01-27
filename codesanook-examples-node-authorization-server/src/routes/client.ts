//routes/client
import db from '../db';
import { Router } from 'express';
import { uid } from '../utils';

const router = Router();

export function register(req, res) {
  req.checkBody('name', 'No valid name is given').notEmpty().len(3, 40)

  const errors = req.validationErrors()
  if (errors) {
    res.send(errors, 400)
  } else {
    const name = req.body['name']
    const clientId = uid(8)
    const clientSecret = uid(20)

    db.collection('clients').findOne({ name: name }, function (err, client) {
      if (client) {
        res.send('client name has already taken', 422)
      } else {
        db.collection('clients').save({ name: name, clientId: clientId, clientSecret: clientSecret }, function (err) {
          res.send({ name: name, clientId: clientId, clientSecret: clientSecret }, 201)
        });
      }
    })
  }
}

router.post('/register', register);
export default router;
