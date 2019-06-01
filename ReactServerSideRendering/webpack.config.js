var path = require('path');
module.exports = {
    entry: {
        server: './scripts/server.js',
        client:'./scripts/client.js'

    },
    output: {
        path: path.resolve(__dirname, 'scripts'),
        filename: '[name].bundle.js',
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    },
    resolve: {
        extensions: ['.js', '.jsx']
    },
    externals: {
        react:'React'
    }
};