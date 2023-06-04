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
 * Get level by username
 */
app.get('/api/users/:username/levels', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT level_num, seed ' +
            'FROM valhalla.levels ' +
            'INNER JOIN games USING (level_id) ' +
            'INNER JOIN users USING (game_id) ' +
            'WHERE username = ?', 
                [req.params["username"]]);
        
        console.log(results);

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
 * Utility function for random integers from 1 to max inclusive
 */ 
function randint(max) {
    return Math.floor((Math.random() * max)) + 1;
}

/**
 * Update level number from json and set random seed by user_id
 */
app.put('/api/users/levels', async (req, res)=>{
    let connection = null;

    try {
        let newSeed = randint(80000);
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'UPDATE valhalla.levels ' + 
            'INNER JOIN games USING (level_id) ' +
            'INNER JOIN users USING (game_id) ' +
            `SET level_num = ?, seed = ${newSeed} ` +
            'WHERE username = ?', 
                [req.body["level_num"], 
                 req.body["username"]
                ]);
        
        console.log(`SUCCESS: New seed: ${newSeed}`)
        res.json({'seed': newSeed});
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
 * Get class by its class_id or class name
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

        else if (req?.body?.name !== undefined) {
            const [results, fields] = await connection
                .execute('SELECT * FROM valhalla.classes WHERE name = ?', 
                    [req.body["name"]]);
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
            'JOIN valhalla.classes USING (stats_id) ' +
            'JOIN valhalla.characters USING (class_id) '+ 
            'WHERE character_id = ?', 
                [req.params["character_id"]]);

        if (results.length === 0) {
            throw new Error("Class not found!");
        }

        res.json(results[0]);
    }

    catch(error) {
        if (error.message === "Class not found!") {
            res.status(404);
            res.json(error);
        }
        else {
            res.status(500);
            res.json(error);
        }
        
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

/**
 * Get all the stats of a specific class using the class name
 */
app.get('/api/classes/stats', async (req, res)=>{
    let connection = null;
    try {
        connection = await connectToDB();
        if (req?.body?.name !== undefined) {
            const [results, fields] = await connection.execute(
                'SELECT * FROM valhalla.stats ' +
                'JOIN valhalla.classes USING (stats_id) ' +
                'WHERE classes.name = ?', 
                    [req.body["name"]]);
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

/**
 * Get all player's metrics
 */
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

/** 
 * Read metrics specific to user
 */
app.get('/api/users/:username/metrics', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();

        const [results, fields] = await connection.execute(
            'SELECT kills, wins FROM valhalla.metrics ' + 
            'INNER JOIN valhalla.users USING (metrics_id) ' +
            'WHERE username = ?',
            [req.params["username"]]);
        
        if (results.length === 0) 
            throw new Error("User not found!");

        console.log(`${results.length} rows returned`);
        res.json(results);
    } 
    
    catch (error) {
        if (error.message === "User not found!")
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
 * View leaderboard
 */
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
        if (error.message === "Invalid Leaderboard!") 
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

/** 
 * Update player metrics
 * The JSON contains the amount of kills and wins to be 
 * added to the previous data
 */
app.put('/api/users/metrics', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();

        const [previousData, fields] = await connection.execute(
            'SELECT kills, wins FROM valhalla.metrics ' +
            'INNER JOIN valhalla.users USING (metrics_id) ' +
            'WHERE username = ?',
            [req.body["username"]]);
        
        if (previousData.length === 0) {
            throw new Error("Data not found!");
        }

        const [results, fields2] = await connection.execute(
            'UPDATE valhalla.metrics ' +
            'INNER JOIN valhalla.users USING (metrics_id) ' +
            'SET kills = ?, wins = ? ' +
            'WHERE username = ?',
            [previousData[0]["kills"] + req.body["kills"],  
             previousData[0]["wins"] + req.body["wins"],
             req.body["username"]]);

        console.log(`${results.affectedRows} rows updated`);
        res.json({'message': `Data updated correctly: ${results.affectedRows} rows updated.`});
    }

    catch(error) {
        if (error.message === "Data not found!") {
            res.status(404);
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
        if(!validDeathPlaces.has(req.params["type"])) throw new Error("Invalid Death Data")
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            `SELECT * FROM valhalla.${req.params["type"]}`);
        console.log(`${results.length} rows returned`);
        res.json(results);
    }

    catch (error) {
        if (error.message === "Invalid Death Data")
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

app.post('/api/game', async (req, res)=>{
    let connection = null;

    try {
        console.log("Creating game!");
        connection = await connectToDB();
        let seed = randint(800000);
        console.log(`Seed: ${seed}`);
        console.log("User: " + req.body["username"]);
        console.log("CharId: " + req.body["character_id"]);
        
        const [results, fields] = await connection.execute(
            `CALL valhalla.create_game(?, ?, ${seed})`, 
            [req.body["username"],
             req.body["character_id"]
            ]);

        res.json({'message': "Level initialized."})
    }

    catch(error) {
        if (error.message === "Invalid User!") {
            res.status(404);
        }

        else if (error.message === "Game Already Exists!") {
            res.status(400);
        }
        else {
            console.log(error);
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

// Update game
app.put('/api/game', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();

        const [results, fields] = await connection
        .query('update valhalla.games set game_id = ?, user_id = ?, level_id = ?, class_id = ? where user_id = ?',
            [req.body['game_id'], req.body['user_id'], req.body['level_id'], req.body['class_id'],
            req.body['user_id']]);
        
        res.json({'message': `Data updated correctly: ${results.affectedRows} rows updated.`});
    }

    catch(error) {
        res.status(500);
        res.json(error);
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

app.post('/api/users/login', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT * FROM valhalla.users WHERE username = ? AND password = ?',
            [req.body["username"], req.body["password"]]);

        if (results.length === 0)
            throw new Error("Invalid Credentials!");
        
        res.json(results[0]);
    }

    catch(error) {
        if (error.message === "Invalid Credentials!")
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

app.post("/api/users", async (req, res) => {
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            "CALL create_user(?, ?, ?);", 
            [req.body["username"], 
             req.body["email"],
             req.body["password"]
            ]);

        res.json(results);
    }

    catch(error) {
        if (error.message === "User already exists") {
            res.status(400);
            res.json(error); 
        }
        else {
            res.status(500);
            res.json(error); 
        }
    }

    finally {
        if(connection!==null) {
            connection.end();
            console.log("Connection closed succesfully!");
        }
    }
});

// endpoint to update a user
app.put('/api/users/data', async (req, res)=>{
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

app.get('/api/users/characters/:username', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT * FROM valhalla.characters ' +
            'INNER JOIN valhalla.users USING (character_id) ' +
            'WHERE username = ?', 
                [req.params["username"]]);
        
        if (results.length === 0) 
            throw new Error("User not found!");

        res.json(results);
    }

    catch(error) {
        if (error.message === "User not found!")
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

app.listen(port, () => {
  console.log(`You can now view your app on a browser`);
  console.log(`Local:\t http://localhost:${port}`);
});
