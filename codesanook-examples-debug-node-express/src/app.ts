// How do I use dotenv with import? https://www.npmjs.com/package/dotenv#how-do-i-use-dotenv-with-import-
import 'dotenv/config';
import express from 'express';
import http from 'http';
import { AddressInfo } from 'net'
import HttpError from './HttpError';
import fetch from 'node-fetch';

const app = express();

const message = 'hello world';
app.get('/', (_, res) => {
  res.send(message);
});

async function getDataAsync() {
  const response = await fetch('https://codequiz.azurewebsites.net/data', { method: 'GET' });
  const data = await response.json();
  return data.data;
}

app.get('/api', async (_, res) => {
  const result = await getDataAsync();
  res.send(result * 10);
});

app.get('/error', () => {
  throw new HttpError('Invalid input');
});

app.use((err, req, res, next) => {
  const { statusCode, message, stack } = err;
  const jsonBody = { status: 'error', statusCode, message };

  res.status(statusCode).json(
    process.env['NODE_ENV'] === 'production'
      ? jsonBody
      : { ...jsonBody, stack }
  );
});


// Create a server
const server = http.createServer(app);
const port = process.env.PORT || 3000;

server.on('listening', onListening);
server.on('error', onError);
server.listen(port);

function onListening() {
  const { port } = server.address() as AddressInfo;
  console.log(`listening on port: ${port}`)
}

function onError(error) {
  console.error('Error starting server', error);
}
