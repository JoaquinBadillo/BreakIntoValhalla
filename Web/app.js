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
app.get('/api/classes', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute('select * from valhalla.classes');

        console.log(`${results.length} rows returned`);
        res.json(results);
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


// Read metrics
app.get('/api/metrics', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();

        const [results, fields] = await connection.execute('select * from valhalla.metrics');

        console.log(`${results.length} rows returned`);
        res.json(results);
    } 
    
    catch (error) {
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

// Read metrics specific to user
app.get('/api/users/metrics', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();

        const [results, fields] = await connection
        .execute('select * from valhalla.metrics where user_id = ?', 
            [req.body["user_id"]]);

        console.log(`${results.length} rows returned`);
        res.json(results);
    } 
    
    catch (error) {
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

// View leaderboard
app.get('/api/metrics/leaderboards', async (req, res)=>{
    let connection = null;
    const validLeaderboards = new Set(["top_kills", "top_weekly_elims"])

    try {
        if (!validLeaderboards.has(req.body["type"])) throw new Error("Invalid Leaderboard!")
        connection = await connectToDB();
        const [results, fields] = await connection.execute(`select * from valhalla.${req.body["type"]}`);
        console.log(`${results.length} rows returned`);
        res.json(results);
    } 
    
    catch (error) {
        if (error.message === "Invalid Leaderboard!") {
            res.status(400);
        } 
        else {
            res.status(500);
        }
        res.json(error);
    }
    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

// Update metrics
app.put('/api/metrics', async (req, res)=>{
    let connection = null;

    try{
        connection = await connectToDB();

        const [results, fields] = await connection
        .query('update valhalla.metrics set kills = ?, num_deaths = ?, wins = ? where metrics_id = ?',
            [req.body['kills'], req.body['num_deaths'], req.body['wins'],
            req.body['metrics_id']]);
        
        console.log(`${results.affectedRows} rows updated`);
        res.json({'message': `Data updated correctly: ${results.affectedRows} rows updated.`});
    }
    catch(error)
    {
        res.status(500);
        res.json(error);
        console.log(error);
    }
    finally
    {
        if(connection!==null) 
        {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

// Update game
app.put('/api/game', async (req, res)=>{
    let connection = null;

    try{
        connection = await connectToDB();

        const [results, fields] = await connection
        .query('update valhalla.games set game_id = ?, user_id = ?, level_id = ?, class_id = ? where user_id = ?',
            [req.body['game_id'], req.body['user_id'], req.body['level_id'], req.body['class_id'],
            req.body['user_id']]);
        
        console.log(`${results.affectedRows} rows updated`);
        res.json({'message': `Data updated correctly: ${results.affectedRows} rows updated.`});
    }
    catch(error)
    {
        res.status(500);
        res.json(error);
        console.log(error);
    }
    finally
    {
        if(connection!==null) 
        {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});


app.listen(port, () => {
  console.log(`You can now view your app on a browser`);
  console.log(`Local:\t http://localhost:${port}`);
});