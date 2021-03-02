// https://github.com/gsoft-inc/craco/blob/master/packages/craco/README.md#webpack-api
const { createWebpackDevConfig, createWebpackProdConfig } = require("@craco/craco");
const cracoConfig = require("./craco.config.js");
const webpackConfig = createWebpackProdConfig(cracoConfig);

module.exports = webpackConfig;
