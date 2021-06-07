const webpack = require('webpack');

module.exports = {
  entry: {
    main: './src/main',
  },
  mode: 'development',
  output: {
    path: __dirname, // Output dir must be absolute path
    filename: 'wwwroot/scripts/[name].js'
  },
  resolve: {
    extensions: ['.ts', '.js', '.tsx', '.jsx']
  },
  module: {
    rules: [
      {
        test: /\.(ts|js)x?$/,
        use: [
          'babel-loader',
        ],
        exclude: /node_modules/
      },
    ]
  },
  externals: {
    react: 'React'
  },
  // https://webpack.js.org/configuration/devtool/
  devtool: 'inline-source-map',
};
