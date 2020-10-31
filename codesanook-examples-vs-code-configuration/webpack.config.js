const path = require('path');

module.exports = {
  entry: [
    './src/main',
  ],
  mode: 'development',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: '[name]-bundle.js',
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', 'jsx'],
  },
  module: {
    rules: [
      {
        test: /\.(ts|js)x?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
      },
    ],
  },
  devServer: {
    contentBase: path.join(__dirname, 'dist'),
    port: 8080,
  },
};
