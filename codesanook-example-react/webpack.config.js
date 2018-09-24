var path = require("path");

module.exports = {
    entry: './src/index.js'    ,
    output: {
        path: path.resolve(__dirname, "dist/assets"),
        filename: 'bundle.js',
    },
    module: {
        rules : [
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                loader: 'babel-loader',
                query: {
                    presets:['env', 'react']
                }
            },
			{
				test: /\.css$/,
				loader: 'style-loader!css-loader!autoprefixer-loader'
			},
			{
				test: /\.scss$/,
				loader: 'style-loader!css-loader!autoprefixer-loader!sass-loader'
			}
        ]
     }
}