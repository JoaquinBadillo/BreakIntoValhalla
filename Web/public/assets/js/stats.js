const URI = "https://valhallaapi-production.up.railway.app";

isFloat = (n) => {
    return Number(n) === n && n % 1 !== 0;
}

fetch(`${URI}/api/characters/stats`).then(res => res.json())
.then(res => {
    res.map((character, index) => {
        for (let attribute in character){
            let selection = document.querySelectorAll(`#${attribute}`);
            
            let value = character[attribute];
            if (selection.length !== 0)           
                selection[index].innerHTML = isFloat(value) ? value.toFixed(2) : value;
        }
    })
}).catch(err => {
    console.log(err)});
