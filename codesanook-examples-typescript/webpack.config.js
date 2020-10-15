const path = require('path');

module.exports = {
    // https://webpack.js.org/configuration/entry-context/#entry
    entry: [
        './src/main',
    ],
    mode: 'development', // check if we need it for hot relead 
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: '[name]-bundle.js',
    },

    resolve: {
        extensions: ['.ts', '.tsx', '.js', 'jsx']
    },

    module: {
        rules: [
            {
                test: /\.(ts|js)x?$/,
                loader: 'babel-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.scss$/,
                use: [
                    // Creates `style` nodes from JS strings
                    'style-loader',
                    // Translates CSS into CommonJS
                    'css-loader',
                    // Compiles Sass to CSS
                    'sass-loader',
                ],
                exclude: /node_modules/
            },
        ]
    },
    // https://webpack.js.org/configuration/devtool/
    devtool: 'inline-source-map',
    devServer: {
        contentBase: path.join(__dirname, 'dist'),
        compress: false,
        port: 9999,
        overlay: true,
    }
};


/*
Hot reload
JS work, it link by included JS file and it will inject to the HTML that has that JS file.
SCSS work, with sass-loader, css-loader, style-loader
HTML not reload
*/