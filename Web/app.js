"use strict"

// Configure dotenv to manage environment variables
import * as dotenv from 'dotenv'
dotenv.config()

import express from 'express'
import mysql from 'mysql2/promise'
import fs from 'fs'

const app = express();
const port = process.env.DB_PORT;

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
 * Endpoint to initialize player data and tables
 */
app.post("/api/users", async (req, res) => {
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.query(
            "CALL init_user(?, ?, ?);", 
            [req.body["username"], 
             req.body["email"],
             req.body["password"]
            ]);

        res.json({'message': "User initialized."})
    }

    catch(error) {
        if (error.message === "User already exists") {
            res.status(400);
            res.json(error); 
        }

        res.status(500);
        res.json(error); 
    }
});

/**
 * Get level by user_id
 */
app.get('/api/users/:id/levels', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT level_num, seed ' +
            'FROM valhalla.levels ' +
            'INNER JOIN games USING (level_id) ' +
            'INNER JOIN users USING (game_id) ' +
            'WHERE user_id = ?', 
                [req.params["id"]]);
        
        if (results.length === 0) 
            throw new Error("Level not found!");
        
        res.json(results[0]);
    }

    catch(error) {
        if (error.message === "Level not found!")
            res.status(404);
        else 
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
        const [results, fields] = await connection.execute(
            'UPDATE valhalla.levels ' + 
            'INNER JOIN games USING (level_id) ' +
            'INNER JOIN users USING (user_id) ' +
            'SET level_num = ?, seed = ? ' +
            'WHERE user_id = ?', 
                [req.body["level_num"], 
                 req.body["seed"], 
                 req.body["user_id"]
                ]);

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
 * Get all the stats of a specific class through the URI
 */
app.get('/api/classes/:class_id/stats', async (req, res)=>{
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection
            .execute('SELECT * FROM valhalla.stats WHERE class_id = ?', 
                [req.params["class_id"]]);
        
        if (results.length === 0) 
            throw new Error("Class not found!");

        res.json(results[0]);
    }

    catch(error) {
        if (error.message === "Class not found!")
            res.status(404);
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

/**
 * Get all the stats of a specific class through the character id
 */
app.get('/api/characters/:character_id/stats', async (req, res)=>{
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT * FROM valhalla.stats ' +
            'INNER JOIN valhalla.classes USING (class_id) ' +
            'INNER JOIN valhalla.characters USING (class_id) ' +
            'WHERE character_id = ?', 
                [req.params["character_id"]]);
        
        if (results.length === 0) 
            throw new Error("Class not found!");

        res.json(results[0]);
    }

    catch(error) {
        if (error.message === "Class not found!")
            res.status(404);
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
        const [results, fields] = await connection.query(
            "UPDATE valhalla.stats SET " +
             "hp = ?, " +
             "primary_attack = ?, " + 
             "secondary_attack = ?, " +
             "primary_lag = ?, " +
             "secondary_lag = ?, " +
             "defense = ?, " +
             "speed = ? " +
            "WHERE class_id = ?", 
            [req.body["hp"],
             req.body["primary_attack"], 
             req.body["secondary_attack"],
             req.body["primary_lag"],
             req.body["secondary_lag"],
             req.body["defense"], 
             req.body["speed"], 
             req.body["class_id"]]
        );

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
app.put('/api/classes/:class_id/stats/:stat', async (req, res)=>{
    let connection = null;
    const validStats = new Set(
        ["hp",
         "primary_attack",
         "secondary_attack",
         "primary_lag",
         "secondary_lag",
         "defense",
         "speed"]
    );

    try {
        // Safety Check
        if(!validStats.has(req.params.stat)) throw new Error("Invalid stat!");

        connection = await connectToDB();
        const [results, fields] = await connection.query(
            `UPDATE valhalla.stats ` +
            'INNER JOIN valhalla.classes USING (stats_id) ' +
            `SET ${req.params["stat"]} = ? ` +
            'WHERE class_id = ?',
                [req.body["value"], req.params["class_id"]]);
        
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

        const [results, fields] = await connection.execute(
            'SELECT kills, wins FROM valhalla.metrics ' + 
            'INNER JOIN valhalla.users USING (metrics_id) ' +
            'WHERE user_id = ?',
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
app.get('/api/metrics/leaderboards/:type', async (req, res)=>{
    let connection = null;
    const validLeaderboards = new Set(["top_kills", "top_weekly_elims"])

    try {
        if (!validLeaderboards.has(req.params["type"])) throw new Error("Invalid Leaderboard!")
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            `SELECT * FROM valhalla.${req.params["type"]}`);
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

//View death place and death cause
app.get('/api/deaths/:type', async (req, res)=>{
    let connection = null;
    const validDeathPlaces = new Set(["death_place", "death_cause"])

    try {
        if(!validDeathPlaces.has(req.params["type"])) throw new Error("Invalid Death Place!")
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            `SELECT * FROM valhalla.${req.params["type"]}`);
        console.log(`${results.length} rows returned`);
        res.json(results);
    }

    catch (error) {
        if (error.message === "Invalid Death Place!") {
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
        .query('update valhalla.metrics set kills = ?, wins = ? where user_id = ?',
            [req.body['kills'], req.body['wins'], req.body['user_id']]);
        
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

    try {
        connection = await connectToDB();

        const [results, fields] = await connection
        .query('update valhalla.games set game_id = ?, user_id = ?, level_id = ?, class_id = ? where user_id = ?',
            [req.body['game_id'], req.body['user_id'], req.body['level_id'], req.body['class_id'],
            req.body['user_id']]);
        
        console.log(`${results.affectedRows} rows updated`);
        res.json({'message': `Data updated correctly: ${results.affectedRows} rows updated.`});
    }

    catch(error) {
        res.status(500);
        res.json(error);
        console.log(error);
    }

    finally {
        if(connection!==null) 
        {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

// endpoint to get all users
app.get('/api/users', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute('SELECT * FROM valhalla.users');

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

// endpoint to get user by id or username or email
app.get('/api/users/:id', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute('select * from valhalla.users where user_id = ? or username = ? or email = ?', [req.params.id, req.params.id, req.params.id]);

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


// endpoint to update a user
app.put('/api/users', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.query('update valhalla.users set username = ?, email = ?, password = ? where user_id = ?', [req.body["username"], req.body["email"], req.body["password"], req.body["user_id"]]);

        console.log(`${results.affectedRows} rows updated`)
        res.json({'message': `User data updated correctly: ${results.affectedRows} rows updated.`})
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

// endpoint to delete a user by id or username or email
app.delete('/api/users/:id', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const[results, fields] = await connection.query('delete from valhalla.users where user_id = ?', [req.body["user_id"]]);

        console.log(`${results.affectedRows} rows updated`)
        res.json({'message': `User deleted correctly: ${results.affectedRows} rows updated.`})
    }

    catch(error) {
        res.status(500);
        res.json(error);
    }

    finally {
        if(connection !==null){
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }

});

app.listen(port, () => {
  console.log(`You can now view your app on a browser`);
  console.log(`Local:\t http://localhost:${port}`);
});