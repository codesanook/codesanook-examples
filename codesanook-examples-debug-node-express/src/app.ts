import dotenv from 'dotenv';
dotenv.config();

import express from 'express';
import http from 'http';
import { AddressInfo } from 'net'
const app = express();

const message = 'hello world';
app.get('/', (_, res) => {
  res.send(message);
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
