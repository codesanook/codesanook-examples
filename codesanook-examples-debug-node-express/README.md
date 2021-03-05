# codesanook-examples-debug-node-express

## How to run the project locally.

- Clone the project.
```
$ git clone git@github.com:codesanook/Codesanook.Examples.git
```
- CD to this example folder.
```
$ cd Codesanook.Examples/codesanook-examples-debug-node-express
```

- Install required modules.
```
yarn
```
- Open the project with VS Code.
```
$ code .
```
- Put a breakpoint in `src/app.ts` file on the line that contains `res.send(message);`
- Start debugging the project by pressing F5 or clicking `Start Debugging` ▶ button.
- The project will start and pause at line that we put the breakpoint.
- On the left hand side of VS Code, you will find some information in  VARIABLES, WATCH, CALL STACK windows.

## Other useful information

### Required packages

```
$ yarn add express dotenv

$ yarn add --dev nodemon @types/express @babel/cli @babel/core @babel/preset-env @babel/plugin-transform-runtime @babel/preset-typescript @babel/register
```
### Configure nodemon.json
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
