// https://www.npmjs.com/package/dotenv#how-do-i-use-dotenv-with-import-
const dotenv = require('dotenv')
dotenv.config();

const path = require('path');
const jsdom = require("jsdom");
const express = require('express');
const config = require('./src/config');
const obfuscator = require('javascript-obfuscator');

const { JSDOM } = jsdom;
const buildPath = path.join(__dirname, 'build');
const app = express();
app.use(express.static(buildPath, { index: false }));

// https://stackoverflow.com/a/46515787/1872200
(async () => {
  try {
    // We need to disable index.html with index:false because it will take precedent over / root path
    await setupRoute();
    createServer();

  } catch (err) {
    console.log(err);
  }
})();

async function setupRoute() {
  const { allConfigsHtmlContent, withoutSecretConfigsHtmlContent } = await getHtmlContent();
  const pattern = /(test|hello|secret)/i;

  app.get('/*', (req, res) => {
    if (pattern.test(req.originalUrl)) {
      res.send(allConfigsHtmlContent);
      return;
    }

    res.send(withoutSecretConfigsHtmlContent);
  });
}

function createServer() {
  const port = process.env.PORT || 3000;
  const server = app.listen(port, () => {
    console.log(`Server is listening on port ${port}`);
  });

  server.on('error', (error) => {
    console.log(error);
  });
}

async function getHtmlContent() {
  const allConfigsHtmlContent = await getHtmlContentWithConfiguration(config);
  const { 'secret': _, ...updatedConfig } = config;
  const withoutSecretConfigsHtmlContent = await getHtmlContentWithConfiguration(updatedConfig);
  return { allConfigsHtmlContent, withoutSecretConfigsHtmlContent };
}

async function getHtmlContentWithConfiguration(config) {
  const indexPath = path.join(buildPath, 'index.html');
  const dom = await JSDOM.fromFile(indexPath, { runScripts: 'outside-only' });
  const { document } = dom.window;

  const scriptElement = document.createElement("script");
  scriptElement.innerHTML = obfuscator.obfuscate(`const config = ${JSON.stringify(config)};`);
  document.getElementsByTagName('head')[0].appendChild(scriptElement);
  return dom.serialize();
}
