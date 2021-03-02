## Create React app with TypeScript template in the current directory
```
yarn create react-app . --template typescript
```

## Debug React app (work both JS and TS project type)

- https://code.visualstudio.com/docs/nodejs/reactjs-tutorial#_debugging-react
- https://medium.com/@auchenberg/live-edit-and-debug-your-react-apps-directly-from-vs-code-without-leaving-the-editor-3da489ed905f

## Deployment
- https://create-react-app.dev/docs/deployment/


## custom configuration

```
yarn add @craco/craco

```

```
touch craco.config.js
```

### Update package.json

```
/* package.json */

"scripts": {
  "start": "craco start",
  "build": "craco build"
  "test": "craco test"
}
```


### Keep logging in terminal


```
  "scripts": {
    "start": "craco start",
    "dev": "concurrently yarn:start",
    "build": "craco build",
    "test": "craco test"
  },

```

## Adding custom variable
https://create-react-app.dev/docs/adding-custom-environment-variables/
