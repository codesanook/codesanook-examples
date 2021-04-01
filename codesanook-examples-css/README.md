# codesanook-examples-css

## How to run a project
- Clone the project 
```
git clone git@github.com:codesanook/Codesanook.Examples.git
```

- CD to the CSS project
```
cd Codesanook.Examples/codesanook-examples-css
```

- Install Node modules
```
yarn install
``` 

- Run the project, a browser will launch automatically on http://localhost:9999
```
yarn serve
```   
- Edit `src/scss/site.scss` or `src/index.html` and you will find hot reload work. 
- When you run serve, you won't find any HTML, CSS or JS files in dist folder because all files are serve from memory by Webpack dev server. 

## Required modules

```shell
yarn add --dev \
  @babel/core \
  @babel/preset-env \
  babel-loader \
  editorconfig \
  file-loader \
  html-loader@0.5.5 \
  extract-loader@2.0.1 \
  style-loader \
  css-loader \
  sass-loader \
  node-sass \
  webpack \
  webpack-cli \
  webpack-dev-server
```

## Hot reload
- !!! HTML work only "html-loader": "0.5.5" and "extract-loader": "2.0.1"

## Credit
- Runyasak Chaengnaimuang
