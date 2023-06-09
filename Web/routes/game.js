import { Router } from "express";
import connectToDB from "../database.js";

const router = Router();

function randint(max) {
    return Math.floor((Math.random() * max)) + 1;
}

router.post('/', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const seed = randint(80000);
        const [results, fields] = await connection.execute(
            `CALL create_game(?, ?, ${seed})`,
            [req.body["username"], req.body["character_id"]]);

        res.json({'seed': seed});
    }

    catch (error) {
        if (error.message === "Invalid User!")
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

export default router;