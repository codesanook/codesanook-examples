module.exports = {
  // https://webpack.js.org/configuration/
  webpack: {
    configure: (webpackConfig, { env }) => {
      if (env === 'development') { // production
        return webpackConfig
      };

      webpackConfig.externals = webpackConfig.externals || [];
      webpackConfig.externals.push(
        {
          './config': 'config'
        }
      );
      return webpackConfig;
    }
  },
  /*
  Alternative configuration
    webpack: {
      configure: {
        externals: {
          './config': 'config'
        }
      }
    }
  */
  babel: {
    presets: ['@emotion/babel-preset-css-prop'],
  }
};
