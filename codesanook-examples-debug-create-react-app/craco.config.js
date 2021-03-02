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
  alternatively
    webpack: {
      configure: {
        externals: {
          './config': 'config'
        }
      }
    }
  */
};
