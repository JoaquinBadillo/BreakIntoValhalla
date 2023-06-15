const URI = "https://valhallaapi-production.up.railway.app";

fetch(`${URI}/api/characters/stats`).then(res => res.json())
.then(res => {
    res.map((character, index) => {
        for (let attribute in character){
            let selection = document.querySelectorAll(`#${attribute}`);

            if (selection.length !== 0)
                selection[index] = character[attribute];
        }
    })
}).catch(err => {
    console.log(err)});
