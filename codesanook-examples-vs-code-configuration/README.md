# VS Code configuration example

### Create Node.js project with a default configuration
```
yarn init -y
```

## Add a required React packages
```
yarn add with the follwing packages
```
- react 
- react-dom 

```
yarn add -- dev with the follwing packages
```

- typescript @types/react 
- @types/react-dom 
- @types/node @babel/core 
- @babel/plugin-proposal-class-properties 
- @babel/plugin-proposal-object-rest-spread 
- @babel/preset-env 
- @babel/preset-react 
- @babel/preset-typescript 
- webpack 
- webpack-cli x
- webpack-dev-server

### Add .babelrc and the following contents.

```
{
    "presets": [
        "@babel/env",
        "@babel/typescript",
        "@babel/react"
    ],
    "plugins": [
        "@babel/proposal-class-properties",
        "@babel/proposal-object-rest-spread"
    ]
}
```

### Add tsconfig.json with the following contents.

```
{
    "compilerOptions": {
        "allowSyntheticDefaultImports": true,
        "esModuleInterop": true,
        "noFallthroughCasesInSwitch": true,
        "noUnusedParameters": true,
        "noImplicitReturns": true,
        "moduleResolution": "node",
        "noUnusedLocals": true,
        "noImplicitAny": true,
        "target": "es2015",
        "module": "es2015",
        "strict": true,
        "jsx": "react",
        "strictPropertyInitialization": false,
        "strictNullChecks": false
    },
    "include": [
        "src/**/*.ts",
    ],
    "exclude": [
        "node_modules"
    ]
}
```

### Add webpack.config.js with the following contents;
```
const path = require('path');

module.exports = {
    entry: [
        './src/main',
    ],
    mode: 'development',
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
        ]
    },
    devServer: {
        contentBase: path.join(__dirname, 'dist'),
        port: 8080,
    }
};
```

## Configure ESLint

```
yarn add --dev the following package
```

- @typescript-eslint/parser
- @typescript-eslint/eslint-plugin
- babel-loader
- eslint
- eslint-config-airbnb
- eslint-plugin-import
- eslint-plugin-jsx-a11y
- eslint-plugin-react
- eslint-plugin-react-hooks

### Create .eslintrc.yml with the following content
```
env:
  browser: true
  es2021: true
extends:
  - airbnb
  - airbnb/hooks
  - 'plugin:@typescript-eslint/recommended'
  - 'plugin:import/typescript'
parser: '@typescript-eslint/parser'
parserOptions:
  ecmaFeatures:
    jsx: true
  ecmaVersion: 12
  sourceType: module
plugins:
  - react
  - '@typescript-eslint'
  - import
rules:
  no-console: 'off'
  semi:
    - error
    - always
  quotes:
    - error
    - single
  react/jsx-filename-extension:
    - error
    - extensions:
        - .jsx
        - .tsx
  no-use-before-define: 'off'
  '@typescript-eslint/no-use-before-define':
    - error
  '@typescript-eslint/explicit-module-boundary-types': 'off'
  import/extensions:
    - error
    - always
    - tsx: never

```

To have ESLint format your code
- Install ESLint Visual Studio plugin https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint
- Add settings .vscode/settings.json with the following contents.
```
{
    "editor.codeActionsOnSave": {
        "source.fixAll.eslint": true
    },
    "eslint.validate": [
        "javascript",
        "typescript",
        "typescriptreact"
    ],
    "editor.fontSize": 16
}
```
- Restart VS code
- There are example code in src folder
- Make some invalid code format and check if you have an error from ESLint.
- Edit code. Then save and see if code auto format. 

# Useful references
- https://stackoverflow.com/a/64024916/1872200

# Credit 
- [Runyasak Chaengnaimuang](https://github.com/runyasak)
- [Wittawat Karpkrikaew](https://github.com/ponggun)
