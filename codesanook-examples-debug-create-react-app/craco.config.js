module.exports = {
  // https://webpack.js.org/configuration/
  webpack: {
    configure: (webpackConfig, { env }) => {
      if (env === 'development') {
        return webpackConfig
      };

      console.log(`env ${env}`);
      webpackConfig.externals = webpackConfig.externals || [];
      webpackConfig.externals.push(
        {
          './config': 'config'
        }
      );
      return webpackConfig;
    }
  },
};
