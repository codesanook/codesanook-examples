const path = require('path');

module.exports = {
    entry: {
        main: './src/main',
        counter: './src/counter',
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: '[name]-bundle.js',
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', 'jsx'],
    },
    module: {
        rules: [
            {
                test: /\.(ts|js)x?$/,
                loader: 'babel-loader',
                exclude: /node_modules/,
            },
        ],
    },
};
