// https://www.npmjs.com/package/dotenv#how-do-i-use-dotenv-with-import-
const dotenv = require('dotenv')
dotenv.config();
const config = require('./src/config');

const express = require('express');
const app = express();

const path = require('path');
const jsdom = require("jsdom");
const { JSDOM } = jsdom;
const buildPath = path.join(__dirname, 'build');

// https://stackoverflow.com/a/46515787/1872200
(async () => {

  try {
    // We need to disable index.html with index:false because it will take precedent over / root path
    app.use(express.static(buildPath, { index: false }));
    const { allConfigsHtmlContent, withoutSecretConfigsHtmlContent } = await getHtmlContent();

    const regex = /(test|hello|secret)/i;
    app.get('/*', function (req, res) {
      const isMatch = regex.test(req.originalUrl);
      console.log(isMatch);
      if (isMatch) {
        res.send(allConfigsHtmlContent);
        return;
      }

      res.send(withoutSecretConfigsHtmlContent);
    });

    app.listen(3000);
  } catch (err) {
    console.log(err);
  }
})();

async function getHtmlContent() {
  // eslint-disable-next-line no-useless-computed-key
  const allConfigsHtmlContent = await getHtmlContentWithConfiguration(config);
  const { 'secret': remove, ...updatedConfig } = config;
  const withoutSecretConfigsHtmlContent = await getHtmlContentWithConfiguration(updatedConfig);
  return { allConfigsHtmlContent, withoutSecretConfigsHtmlContent };
}

async function getHtmlContentWithConfiguration(config) {
  const indexPath = path.join(buildPath, 'index.html');
  const dom = await JSDOM.fromFile(indexPath, { runScripts: 'outside-only' });
  const { document } = dom.window;

  const script = document.createElement("script");
  script.type = "text/javascript";
  script.innerHTML = `const config = ${JSON.stringify(config)};`;
  document.getElementsByTagName('head')[0].appendChild(script);
  const defaultHtmlContent = dom.serialize();

  return defaultHtmlContent;
}

