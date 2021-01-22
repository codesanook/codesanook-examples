//routes/user.js
import { Router } from 'express';

/* GET users listing. */
const router = Router();
router.get('/', function (req, res, next) {
  res.send('respond with a resource');
});

/* GET user profile. */
router.get('/profile', function (req, res, next) {
  res.send(req.user);
});

export default router;
