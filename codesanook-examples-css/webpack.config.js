const path = require('path');

const outputDir = path.join(__dirname, 'dist');
module.exports = {
  entry: {
    main: './src/main',
  },
  mode: 'development',
  output: {
    path: outputDir,
    filename: '[name]-bundle.js',
  },
  resolve: {
    extensions: ['.js'],
  },
  module: {
    rules: [
      {
        test: /\.js?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
      },
      {
        test: /\.scss$/,
        use: [
          // Creates `style` nodes from JS strings
          "style-loader",
          // Translates CSS into CommonJS
          "css-loader",
          // Compiles Sass to CSS
          "sass-loader",
        ],
        exclude: /node_modules/,
      },
      {
        test: /\.html$/,
        use: [
          {
            loader: 'file-loader',
            options: {
              name: 'index.html',
            },
          },
          'extract-loader',
          'html-loader',
        ],
      },
    ],
  },
  // https://webpack.js.org/configuration/devtool/
  devtool: 'inline-source-map',
  devServer: {
    contentBase: outputDir,
    compress: false,
    port: 9999,
    overlay: true,
    open: true // Launch a browser automatically 
  },
};
