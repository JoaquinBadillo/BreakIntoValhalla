import * as dotenv from 'dotenv';
import express from 'express';

import fs from 'fs';
import cors from 'cors';

dotenv.config();

const app = express();
app.use(express.json());
app.use(express.static('./public'));
app.use(cors());

const port = process.env.DB_PORT || 5000;

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

import metricsRouter from './routes/metrics.js';
import levelsRouter from './routes/levels.js';
import gameRouter from './routes/game.js';
import charactersRouter from './routes/characters.js';
import usersRouter from './routes/users.js';
import deathsRouter from './routes/deaths.js';

// Setting up the routes:
app.use('/api/users/metrics', metricsRouter);
app.use('/api/levels', levelsRouter);
app.use('/api/game', gameRouter);
app.use('/api/characters', charactersRouter);
app.use('/api/users', usersRouter);
app.use('/api/deaths', deathsRouter);

app.listen(port, () => {
  console.log(`You can now view your app on a browser`);
  console.log(`Local:\t http://localhost:${port}`);
});
