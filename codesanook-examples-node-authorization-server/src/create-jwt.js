import jwt from 'jsonwebtoken';

const user = {
  id: 1,
  roles: ['editor'],
};
const jwtSecret = 'your_jwt_secret';
const accessToken = jwt.sign(user, jwtSecret, {
  algorithm: 'HS256',
  expiresIn: '12h',
});

console.log(accessToken);
