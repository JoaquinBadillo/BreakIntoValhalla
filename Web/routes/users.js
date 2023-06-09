import { Router } from "express";
import connectToDB from "../database.js";

const router = Router();

router.post('/login', async (req, res)=>{
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

router.post("/", async (req, res) => {
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

export default router;