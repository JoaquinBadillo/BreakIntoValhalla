import { Router } from "express";
import connectToDB from "../database.js";

const router = Router();

router.get('/stats', async (req, res)=>{
    let connection = null;
    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT * FROM valhalla.stats'
        );

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

router.get('/:username', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT character_id FROM valhalla.users ' +
            'INNER JOIN valhalla.games USING (game_id) ' +
            'INNER JOIN valhalla.characters USING (character_id) ' +
            'WHERE username = ?', 
                [req.params["username"]]);
        
        if (results.length === 0) 
            throw new Error("User not found!");

        res.json(results[0]);
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

router.get('/:character_id/stats', async (req, res)=>{
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

export default router;