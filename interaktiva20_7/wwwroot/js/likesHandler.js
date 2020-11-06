
let result
let imdbid
let numberOfLikes
let numberOfDislikes
var imdbIdsArray = new Array()
if (sessionStorage.getItem('savedImdbid') != null) {
    imdbIdsArray = JSON.parse(sessionStorage.getItem('savedImdbid'))
}

//Eventlistener som lyssnar på vilken knapp användare tryckt på.
document.querySelectorAll('#thumbs-up').forEach(selectedMovie => {
    selectedMovie.addEventListener('click', async function () {
        const like = 'like'
        imdbId = selectedMovie.accessKey
        result = await SendVote(selectedMovie, imdbId, like)
        UpdateUI(selectedMovie, result, like)
    })
})

document.querySelectorAll('#thumbs-down').forEach(selectedMovie => {
    selectedMovie.addEventListener('click', async function () {
        const dislike = 'dislike'
        imdbId = selectedMovie.accessKey
        result = await SendVote(selectedMovie, imdbId, dislike)
        UpdateUI(selectedMovie, result, dislike)
    })
})

//Funktion för att göra själva API-anropet och skicka like/dislike
async function SendVote(selectedMovie, imdbid, likeOrDislike) {

    let alreadyVoted = CheckIfAlreadyVoted(imdbid)
    let url = `https://localhost:44313/api/${imdbid}/${likeOrDislike}`

    if (alreadyVoted == false) {
        let response = await fetch(url)

        if (response.status == 200) {
            imdbIdsArray.push(imdbid)
            SaveImdb()
            return true
        }
        else {
            alert('Something went wrong')
        }
    }
    else {
        alert('Whoops! You have already voted on this movie')
    }   
    return false   
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

function UpdateUI(selectedMovie, bool, likeOrDislike) {

    if (bool) {
        if (likeOrDislike == 'like') {
            numberOfLikes = selectedMovie.querySelector('a').textContent
            selectedMovie.querySelector('a').textContent = parseInt(numberOfLikes) + 1
        }
        else if (likeOrDislike == 'dislike') {
            numberOfDislikes = selectedMovie.querySelector('a').textContent
            selectedMovie.querySelector('a').textContent = parseInt(numberOfDislikes) + 1
        }
    }
}

//Funktion för att spara alla imdbId för de filmer som blivit röstade på i sessionen.
function SaveImdb() {
    window.sessionStorage.setItem('savedImdbid', JSON.stringify(imdbIdsArray))
}