const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const outputFileName = 'codesanook-examples-typescript';

module.exports = {
    // https://webpack.js.org/configuration/entry-context/#entry
    entry: [
        './src/main',
    ],
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: `${outputFileName}.js`,
    },

    resolve: {
        extensions: ['.ts', '.tsx', '.js', 'jsx']
    },

    module: {
        rules: [{
            test: /\.(ts|js)x?$/,
            loader: 'babel-loader',
            exclude: /node_modules/,
        }, {
            test: /\.scss$/,
            use: [{
                loader: MiniCssExtractPlugin.loader,
            }, {
                loader: 'css-loader', // Translates CSS to CommonJS modules
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
                    sourceMap: true,
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
                    outputPath: './../styles',
                }
            }]
        }]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: `./../dist/${outputFileName}.css`,
        })
    ],
    // externals: {
    //     react: 'React',
    // },
    // https://webpack.js.org/configuration/devtool/
    devtool: 'inline-source-map',
};
