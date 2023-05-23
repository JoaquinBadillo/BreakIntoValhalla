"use strict"

import express from 'express'
import fs from 'fs'

const app = express();
const port = 5000;

app.use(express.json());
app.use(express.static('./public'));

// Send landing page to browser on the root route
app.get('/', (req, res)=> {
    fs.readFile('./public/index.html', 'utf8', 
    (err, html) => {
        if(err) {
            res.status(500).send('Internal Server Error: ' + err);
            return;
        }
        
        res.send(html);

    });
});

// API


app.listen(port, () => {
  console.log(`You can now view your app on a browser`);
  console.log(`Local:\t http://localhost:${port}`);
});