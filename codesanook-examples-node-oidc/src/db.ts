import * as mongojs from 'mongojs';

const databaseUrl = 'authorization_server'
const db = mongojs(databaseUrl)
export default db;

