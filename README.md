# BreakIntoValhalla

Break Into Valhalla is an action-based rougelite where the player takes the role of a once worthy norse warrior that was sent to Helheim and who will fight against frightening creatures to prove their worth.

Grab a weapon, and smite powerful enemies to gain loot, progress through dungeons and show your might! 

## Using the project

The game connects to a database through a web REST API. 
Since the game is not deployed yet, there are some steps that must be taken for the game to work as intended if you are cloning this repo.

### Setting up the database

Execute the SQL scripts in order in a MySQL server: schema, views, data. 
Trying to execute the data script without a schema will result in a failure.

Afterwards create a new user in your MySQL server; give this user a password and access to the 4 CRUD operations.

### Setting up the server

Now that you have the database in localhost, you can set your environment variables in a .env file.
Go to the Web directory and create a file called `.env`. In this file you will create 4 environment variables.
If you have the database in your computer, you can use the following as your `.env` file contents, where dollar sign ($) represents values you must change according to the steps taken in the previous step.

```env
  DB_HOST=localhost
  DB_USER=$MySQLUser
  DB_PASS=$MySQLPassword
  DATABASE=valhalla
```
### Running the game

The game is expected to be played on a computer in fullscreen. Open the project in Unity and once it loads build it for your OS and run it. 
You will be presented with the title screen.

## This project as a learning resource

For people learning to code with this project we warn you that this project has not been designed with state of the art security practices, mainly due to the time we had available for development. We encourage all of you to learn about common attacks a web service could suffer, SQL injections could be a potential threat. You might also want to look into "sessions" and try to create different functionality for administrators and normal users of your API.
