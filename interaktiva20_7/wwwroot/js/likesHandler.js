
let likeOrDislike
var imdbIdsArray = new Array()
if (sessionStorage.getItem('savedImdbid') != null) {
    imdbIdsArray = JSON.parse(sessionStorage.getItem('savedImdbid'))
}

//Eventlistener som lyssnar på vilken knapp användare tryckt på.
document.querySelectorAll('#thumbs-up').forEach(selectedMovie => {
    selectedMovie.addEventListener('click', async function () {
        likeOrDislike = 'like'
        await SendVote(selectedMovie, likeOrDislike)
    })
})

document.querySelectorAll('#thumbs-down').forEach(selectedMovie => {
    selectedMovie.addEventListener('click', async function () {
        likeOrDislike = 'dislike'
        await SendVote(selectedMovie, likeOrDislike)
    })
})

//Funktion för att göra själva API-anropet och skicka like/dislike
async function SendVote(selectedMovie, likeOrDislike) {

    let alreadyVoted = CheckIfAlreadyVoted(selectedMovie.accessKey)
    let url = `https://localhost:44313/api/${selectedMovie.accessKey}/${likeOrDislike}`

    if (alreadyVoted == false) {
        let response = await fetch(url)
        let data = await response.json()

        if (response.status == 200) {
            imdbIdsArray.push(selectedMovie.accessKey)
            SaveImdb()
            UpdateUI(data, selectedMovie, likeOrDislike)
        }
        else {
            alert('Something went wrong')
        }
    }
    else {
        alert('Whoops! You have already voted on this movie')
    }   
}

//Funktion för att kontrollera om en röst på filmen redan gjorts eller inte.
function CheckIfAlreadyVoted(imdbid) {

    for (var i = 0; i < imdbIdsArray.length; i++) {
        if (imdbIdsArray[i] == imdbid) {
            return true
        }
    }
    return false
}

//Funktion för att uppdatera vyn med nya siffror
function UpdateUI(data, selectedMovie, likeOrDislike) {

    if (likeOrDislike == 'like') {
        selectedMovie.querySelector('a').textContent = data.numberOfLikes
    }
    else if (likeOrDislike == 'dislike') {
        selectedMovie.querySelector('a').textContent = data.numberOfDislikes
    }

}

//Funktion för att spara alla imdbId för de filmer som blivit röstade på i sessionen.
function SaveImdb() {
    window.sessionStorage.setItem('savedImdbid', JSON.stringify(imdbIdsArray))
}