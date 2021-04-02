# codesanook-examples-playwright

## Required packages

```shell
yarn add --dev jest typescript ts-jest @types/jest yarn add playwright
```

## Create jest.config.js

Create jest.config.js file at the root level to use ts-jest preset

```js
module.exports = {
  preset: "ts-jest",
  testEnvironment: "node",
};
```

## Run a test

```shell
yarn test
```
