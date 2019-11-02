const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    entry: {
        'bundle': './src/main',
    },
    output: {
        path: path.resolve(__dirname, 'scripts'),
        filename: '[name].js',
    },

    resolve: {
        extensions: ['.ts', '.tsx', '.js', 'jsx']
    },
    module: {
        rules: [{
            test: /\.(ts|js)x?$/,
            loader: 'babel-loader',
            exclude: /node_modules/
        }, {
            test: /\.scss$/,
            use: [{
                loader: MiniCssExtractPlugin.loader,
            }, {
                loader: 'css-loader', // translates CSS into CommonJS modules
            }, {
                loader: 'postcss-loader', // Run post css actions
                options: {
                    plugins: function () { // post css plugins, can be exported to postcss.config.js
                        return [
                            require('precss'),
                            require('autoprefixer'),
                        ];
                    }
                }
            }, {
                loader: 'resolve-url-loader',
            }, {
                loader: 'sass-loader', // compiles Sass to CSS, using Node Sass by default
                options: {
                    sourceMap: true
                }
            }],
            exclude: /node_modules/
        },
        {
            test: /\.(png|jpe?g|gif|svg|eot|ttf|woff2?)$/,
            use: [{
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]',
                    outputPath: './../content',
                }
            }]
        }]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: './../content/style.css',
        }),
    ],
    externals: {
        react: 'React', //module => Variable in external file
        jquery: '$',
    },
    //https://webpack.js.org/configuration/devtool/
    devtool: 'inline-source-map',
};
