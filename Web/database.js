import * as dotenv from 'dotenv';
import mysql from 'mysql2/promise';

export default async function connectToDB() {
    return await mysql.createConnection({
        host: process.env.DB_HOST,
        user: process.env.DB_USER,
        password: process.env.DB_PASS,
        database: process.env.DATABASE
    })
}