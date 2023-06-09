import { Router } from "express";
import connectToDB from "../database.js";

const router = Router();

function randint(max) {
    return Math.floor((Math.random() * max)) + 1;
}

router.get('/:username', async (req, res)=>{
    let connection = null;

    try {
        console.log("Hello there")
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            'SELECT * FROM levels INNER JOIN games USING (level_id) INNER JOIN users USING (game_id) WHERE username = ?',
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

router.put('/', async (req, res) => {
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
                [req.body["level_num"], req.body["username"]]);
        
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

export default router;