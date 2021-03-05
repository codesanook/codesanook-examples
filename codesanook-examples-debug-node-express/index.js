if (process.env.NODE_ENV === 'development') {
  // https://babeljs.io/docs/en/babel-register
  require('@babel/register')({
    extensions: ['.js', '.ts'],
  });
  require('./src/app');
} else {
  require('./lib/app');
}
