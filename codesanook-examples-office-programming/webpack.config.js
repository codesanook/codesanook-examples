// Useful for building unit test
const path = require('path');
module.exports = {
  entry: {
    main: './src/index',
  },
  output: {
    path: path.resolve(__dirname, 'scripts'),
    filename: '[name].bundle.js',
  },
  resolve: {
    extensions: ['.ts', '.js']
  },
  module: {
    rules: [
      {
        test: /\.(ts|js)x?$/,
        loader: 'babel-loader',
        exclude: /node_modules/
      }
    ],
  }
};
