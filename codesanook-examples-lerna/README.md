## Credit
https://michalzalecki.com/solve-code-sharing-and-setup-project-with-lerna-and-monorepo/


## Install Lerna
$ npm install --save-dev lerna

## Create sample react app
$ cd packages
$ npx create-react-app codesanook --typescript
$ npx create-react-app nullgadget --typescript

## Clean node_modules in each project and install centralize nodes_modules in the root folder
$ cd ..  
$ npx lerna clean -y 
$ npx lerna bootstrap --hoist

## Run a React project in each project folder
$ cd packages/codesanook
$ npm start

## Alternative way to run a React project at root folder
$ npx lerna run start --scope=codesanook