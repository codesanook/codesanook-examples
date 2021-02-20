/* eslint-disable @typescript-eslint/no-var-requires */
const path = require('path');

const outputDir = path.resolve(__dirname, 'dist');
const nodeExternals = require('webpack-node-externals');

module.exports = {
  target: 'node',
  mode: 'development',
  devtool: 'source-map',
  entry: {
    main: './src/app',
    pkce: './src/create-code-challenge',
    jwt: './src/create-jwt',
  },
  output: {
    path: outputDir,
    filename: '[name]-bundle.js', // Relative to main output.path
  },
  resolve: {
    extensions: ['.ts', '.js'],
  },
  module: {
    rules: [
      {
        test: /\.(t|j)s$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
      },
    ],
  },
  externals: [nodeExternals()],
};
