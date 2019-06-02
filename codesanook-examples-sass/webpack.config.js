const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
/*
Useful webpack setup link
Setup webpack with Bootstrap
https://getbootstrap.com/docs/4.0/getting-started/webpack/
*/
module.exports = {
    entry: "./index.js",
    output: {
        path: path.resolve(__dirname, "dist"),
        filename: "bundle.js",
    },
    resolve: {
        extensions: [".js"]
    },
    module: {
        rules: [{
            test: /\.scss$/,
            use: [{
                loader: MiniCssExtractPlugin.loader
            }, {
                loader: "css-loader" // translates CSS into CommonJS modules
            }, {
                loader: "sass-loader", // compiles Sass to CSS, using Node Sass by default
            }],
            exclude: /node_modules/
        }]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "style.css"
        })
    ],
    devServer: {
        contentBase: path.join(__dirname, "dist"),
        compress: true,
        port: 9000
    }
};