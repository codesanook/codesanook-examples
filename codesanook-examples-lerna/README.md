## Credit
https://michalzalecki.com/solve-code-sharing-and-setup-project-with-lerna-and-monorepo/


## Install Lerna at the root of a project
$ npm install --save-dev lerna

## Init the project
$npx lerna init

## Update Lerna package folder
Change value of packages in lerna.json

e.g.
```
{
  "packages": [
    "Orchard.Web/Modules/*",
    "Orchard.Web/Themes/*"
  ],
  "version": "0.0.0"
}
```

## Create two sample react apps with TypeScript project type
$ CD to the root of package folders that defined in lerna.json
$ CD packages
$ npx create-react-app codesanook --typescript
$ npx create-react-app nullgadget --typescript
$ npx create-react-app codesanook-slide-widget --typescript
$ npx create-react-app codesanook-common --typescript

## Clean node_modules in each project and install central nodes_modules in the root folder
CD back to root of the Lerna project
$ CD ..
$ npx lerna clean -y
$ npx lerna bootstrap --hoist

## Run a React project in a specific project folder
$ cd packages/codesanook
$ npm start

## Alternative way to run a React project at root folder
$ npx lerna run start --scope=codesanook
$ npx lerna run build --scope=codesanook-common
$ npx lerna run start --scope=codesanook-common

## Install new npm package
$ npx lerna add package-name --dev

## Uninstall a package
This package is useful when you have mismatch module.
You can uninstall that module and add it again.
$ npx lerna exec -- npm uninstall package-name