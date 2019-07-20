## Credit
https://michalzalecki.com/solve-code-sharing-and-setup-project-with-lerna-and-monorepo/

## Install Lerna at the root of a project
npm install --save-dev lerna

## Init the Lerna project
npx lerna init

## Update Lerna package folder
Change value of packages in lerna.json to
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
CD to the root of package folders that defined in lerna.json
CD packages
npx create-react-app codesanook --typescript
npx create-react-app nullgadget --typescript
npx create-react-app codesanook-slide-widget --typescript
npx create-react-app codesanook-common --typescript

## Clean node_modules in each project and install central nodes_modules in the root folder
CD back to root of the Lerna project
CD ..
npx lerna clean -y
npx lerna bootstrap --hoist

## Run a React project in a module folder
cd packages/codesanook
npm start

## Alternative way to run a React project at root folder
npx lerna run start --scope=codesanook
npx lerna run build --scope=codesanook-common
npx lerna run dev --scope=codesanook-theme
npx lerna run dev --scope=codesanook-facebook-connect

## Install new npm package
npx lerna add package-name --dev
npx lerna add @types/es6-promise --dev --scope=codesanook-facebook-connect
npx lerna add babel-polyfill --dev --scope=codesanook-facebook-connect
npx lerna exec -- npm install --scope=codesanook-theme

## Uninstall a package
This package is useful when you have mismatch module.
You can uninstall that module and add it again.
npx lerna exec -- npm uninstall package-name
e.g.
npx lerna exec -- npm uninstall node-sass

## Interesting point, need to learn more
- Add package with lerna not add a version to root package.json
