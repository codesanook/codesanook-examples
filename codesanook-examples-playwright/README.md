# codesanook-examples-playwright

# To run the project locally

- Clone the project

```shell
git clone https://github.com/codesanook/Codesanook.Examples.git
```

- CD to the project

```
cd Codesanook.Examples/codesanook-examples-playwright
```

- Install packages

```shell
yarn install
```

- Run a test

```shell
yarn test
```

- You will find test result in a console

## Required packages

```shell
yarn add --dev jest typescript ts-jest @types/jest yarn add playwright
```

## jest.config.js

We need to have jest.config.js file at the root level for using ts-jest preset

```js
module.exports = {
  preset: "ts-jest",
  testEnvironment: "node",
};
```
