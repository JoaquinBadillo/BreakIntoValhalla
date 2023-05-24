"use strict"

// Configure dotenv to manage environment variables
import * as dotenv from 'dotenv'
dotenv.config()

import express from 'express'
import mysql from 'mysql2/promise'
import fs from 'fs'

const app = express();
const port = 5000;

app.use(express.json());
app.use(express.static('./public'));

async function connectToDB() {
    return await mysql.createConnection({
        host: process.env.DB_HOST,
        user: process.env.DB_USER,
        password: process.env.DB_PASS,
        database: process.env.DATABASE
    })
}

// Send landing page to browser on the root route
app.get('/', (req, res)=> {
    fs.readFile('./public/index.html', 'utf8', (err, html) => {
        if(err) {
            res.status(500).send('Internal Server Error: ' + err);
            return;
        }
        
        res.send(html);
    });
});

// API
/**
 * Get level by user_id
 */
app.get('/api/users/levels', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection
            .execute('SELECT level_num, seed FROM valhalla.levels INNER JOIN games USING (level_id) WHERE user_id = ?', 
                [req.body["user_id"]]);
        res.json(results);
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
        }
    }
});

/**
 * Update level number or seed by user_id.
 */
app.put('/api/users/levels', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection
            .execute('UPDATE valhalla.levels INNER JOIN games USING (level_id) SET level_num = ?, seed = ? WHERE user_id = ?', 
                [req.body["level_num"], req.body["seed"], req.body["user_id"]]);
        res.json({'message': 'Seed updated correctly!'});
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
        }
    }
});

/**
 * Get class by its class_id.
 */
app.get('/api/classes', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        if (req?.body?.class_id !== undefined) {
            console.log("yes");
            const [results, fields] = await connection
                .execute('SELECT * FROM valhalla.classes WHERE class_id = ?', 
                    [req.body["class_id"]]); 
            res.json(results);
        }
        else {
            const [results, fields] = await connection
                .execute('SELECT * FROM valhalla.classes');
            res.json(results);
        }
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

/**
 * Update class names
 */
app.put('/api/classes', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection
            .query('UPDATE valhalla.classes SET name = ? WHERE class_id = ?', 
                [req.body["name"], req.body["class_id"]]);
        res.json({'message': 'Data updated correctly!'});
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

/**
 * Get all the stats of a specific class
 */
app.get('/api/classes/stats', async (req, res)=>{
    let connection = null;
    try {
        connection = await connectToDB();
        if (req?.body?.class_id !== undefined) {
            const [results, fields] = await connection
                .execute('SELECT * FROM valhalla.stats WHERE class_id = ?', 
                    [req.body["class_id"]]);
            res.json(results);
        }

        else {
            const [results, fields] = await connection
                .execute('SELECT * FROM valhalla.stats');
            res.json(results);
        }
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

/**
 * Update all stats for a specific class
 */
app.put('/api/classes/stats', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection
            .query('UPDATE valhalla.stats SET hp = ?, attack = ?, attack_speed = ?, defense = ?, speed = ? WHERE class_id = ?', 
                [req.body["hp"], req.body["attack"], req.body["attack_speed"], req.body["defense"], req.body["speed"], req.body["class_id"]]);

        res.json({'message': 'Data updated correctly!'});
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

/**
 * Uncoupled API endpoint to update a specific stat for a particular class
 * -- This endpoint is not meant to be used on Unity --
 * It is designed to facilitate the release of patches
 */ 
app.put('/api/classes/stats/:stat', async (req, res)=>{
    let connection = null;
    const validStats = new Set(["hp", "attack", "attack_speed", "defense", "speed"]);

    try {
        // Safety Check
        if(!validStats.has(req.params.stat)) throw new Error("Invalid stat!");

        connection = await connectToDB();
        const [results, fields] = await connection
            .query(`UPDATE valhalla.stats SET ${req.params.stat} = ? WHERE class_id = ?`, 
                [req.body["value"], req.body["class_id"]]);

        res.json({'message': 'Stat updated correctly!'});
    }

    catch(error) {
        // Notify the API user if the error is on their side or on the server's side
        if (error.message === "Invalid stat!")
            res.status(400);
        else
            res.status(500);

        res.json(error);
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

app.listen(port, () => {
  console.log(`You can now view your app on a browser`);
  console.log(`Local:\t http://localhost:${port}`);
});