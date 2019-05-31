// This example still uses CommonJS syntax because Node hasn't yet shipped support for ES6 module syntax at the time of writing
var path = require('path');

module.exports = {
    entry: {
        server: './scripts/server.js'
    },
    output: {
        filename: './scripts/[name].bundle.js'
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /node_modules/,
                loader: 'babel-loader'
            }
        ]
    },
    resolve: {
        extensions: ['.js', '.jsx']
    }
};