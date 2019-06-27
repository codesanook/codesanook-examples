const path = require('path');

module.exports = {
    entry: {
        server: './src/server',
        client: './src/client'
    },
    output: {
        path: path.resolve(__dirname, 'scripts'),
        filename: '[name].bundle.js',
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', 'jsx']
    },
    module: {
        rules: [{ test: /\.(ts|js)x?$/, loader: 'babel-loader', exclude: /node_modules/ }],
    },
    plugins: [
    ],
    externals: {
        react: 'React'
    }
};
