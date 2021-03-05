## Required packages
- express
- dotenv
- nodemon
- typescript
- @types/express
- @babel/cli
- @babel/core
- @babel/plugin-transform-runtime
- @babel/preset-env
- @babel/preset-typescript
- @babel/register

// nodemon.json
```
{
  "verbose": true,
  "watch": [
    "src"
  ],
  "ext": "js, ts",
  "env": {
    "NODE_ENV": "development"
  }
}

```

yarn add express dotenv
yarn add --dev nodemon typescript @types/express @babel/cli @babel/core @babel/preset-env @babel/plugin-transform-runtime @babel/preset-typescript @babel/register
