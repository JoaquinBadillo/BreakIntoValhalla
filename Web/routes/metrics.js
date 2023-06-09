import { Router } from "express";
import connectToDB from "../database.js";

const router = Router();

router.get('/leaderboards/:type', async (req, res)=>{
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

router.put('/', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();

        const [results, fields] = await connection.execute(
            'UPDATE valhalla.metrics ' +
            'INNER JOIN valhalla.users USING (metrics_id) ' +
            'SET kills = kills + ?, wins = wins + ? ' +
            'WHERE username = ?',
            [req.body["kills"],  
             req.body["wins"],
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

export default router;