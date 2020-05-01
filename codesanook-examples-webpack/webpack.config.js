const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');

const siteFile = [__dirname, 'src', 'site'];
const outputPath = [__dirname, 'dist'];

module.exports = {
    entry: {
        site: path.resolve(...siteFile),
    },
    output: {
        path: path.resolve(...outputPath),
    },
    resolve: {
        // https://github.com/webpack/webpack-dev-server/issues/720#issuecomment-268470989
        // https://github.com/webpack/webpack-dev-server/issues/2393
        extensions: ['.js', '.scss'],
    },
    devtool: 'source-map',
    module: {
        rules: [
            {
                test: /\.scss$/,
                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                        options: {
                            // https://github.com/webpack-contrib/mini-css-extract-plugin#hot-module-reloading-hmr
                            hmr: true,
                        },
                    },
                    {
                        loader: 'css-loader', // Translates CSS into CommonJS modules
                        options: {
                            sourceMap: true,
                        },
                    },
                    {
                        loader: 'postcss-loader', // Run post css actions
                        options: {
                            plugins: () => {
                                // post css plugins, can be exported to postcss.config.js
                                return [
                                    require('precss'),
                                    require('autoprefixer'),
                                ];
                            },
                            sourceMap: true,
                        },
                    },
                    {
                        loader: 'resolve-url-loader',
                    },
                    {
                        loader: 'sass-loader', // Compiles Sass to CSS, using node-sass by default
                        options: {
                            sourceMap: true,
                        },
                    },
                ],
                exclude: /node_modules/,
            },
            {
                test: /\.(png|jpe?g|gif|svg|eot|ttf|woff2?)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            name: '[name].[ext]',
                            outputPath: '.', //relative to output
                        },
                    },
                ],
            },
        ],
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: './[name].css', // relative to output
        }),
        new HtmlWebpackPlugin({
            template: './src/index.html',
        }),
    ],

    devServer: {
        contentBase: path.join(__dirname, 'dist'),
        port: 8080,
        open: true,
    },
};
