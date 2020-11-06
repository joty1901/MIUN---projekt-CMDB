
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

    if (alreadyVoted == false) {
            imdbIdsArray.push(selectedMovie.accessKey)
            SaveImdb()
            UpdateUI(selectedMovie, likeOrDislike)
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
function UpdateUI(selectedMovie, likeOrDislike) {

    if (likeOrDislike == 'like') {
        let numberOfLikes = selectedMovie.querySelector('a').textContent
        selectedMovie.querySelector('a').textContent = parseInt(numberOfLikes) + 1
    }
    else if (likeOrDislike == 'dislike') {
        let numberOfDislikes = selectedMovie.querySelector('a').textContent
        selectedMovie.querySelector('a').textContent = parseInt(numberOfDislikes) + 1
    }
}

//Funktion för att spara alla imdbId för de filmer som blivit röstade på i sessionen.
function SaveImdb() {
    window.sessionStorage.setItem('savedImdbid', JSON.stringify(imdbIdsArray))
}