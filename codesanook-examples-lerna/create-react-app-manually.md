npm init -y
npm install typescript --save-dev
create tsconfig.json with the following content
```
{ 
  "compilerOptions": { 
    "target": "es5", 
    "module": "es6", 
    "moduleResolution": "node", 
    "lib": ["es6", "dom"],
    "sourceMap": true, 
    "jsx": "react", 
    "strict": true, 
    "noImplicitReturns": true,
    "rootDir": "src",
    "outDir": "dist",
  },
  "include": ["**/*.ts", "**/*.tsx"],
  "exclude": ["node_modules"]
}
```

Add tslint.json with the following content
```
{
  "extends": ["tslint:recommended", "tslint-react", "tslint-config-prettier"],
  "linterOptions": {
    "exclude": ["node_modules/**/*.ts"]
  }
}
```

npm install react react-dom

Add type definition
npm install @types/react @types/react-dom --save-dev

add html and source file

add webpack
npm install webpack webpack-cli --save-dev
npm install webpack-dev-server --save-dev

install TypeScript Webpack plugin
npm install ts-loader --save-dev


create webpack.config.js and add the following content
```
const path = require("path");

module.exports = {
  mode: 'development'
  entry: "./src/index.tsx",
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/
      }
    ]
  },
  resolve: {
    extensions: [".tsx", ".ts", ".js"]
  },
  output: {
    path: path.resolve(__dirname, "dist"),
    filename: "bundle.js"
  },
  devServer: {
    contentBase: path.join(__dirname, "dist"),
    compress: true,
    port: 9000
  }
};
```

Add start and build command to package.json

"start": "webpack-dev-server --env development",
"build": "webpack --env production"