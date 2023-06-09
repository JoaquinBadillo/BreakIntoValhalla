import { Router } from "express";
import connectToDB from "../database.js";

const router = Router();

router.get('/:type', async (req, res)=>{
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

router.post('/', async (req, res)=>{
    let connection = null;

    try {
        connection = await connectToDB();
        const [results, fields] = await connection.execute(
            `CALL valhalla.add_death(?, ?, ?)`,
            [req.body["username"], req.body["room"], req.body["killer"]]);
        console.log(`${results.affectedRows} rows inserted`);
        res.json({'message': 'Death added successfully!'});
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
