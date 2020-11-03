
let result
let imdbid
let numberOfLikes
let numberOfDislikes
var imdbIdsArray = new Array()

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

async function SendVote(selectedMovie, imdbid, likeOrDislike) {

    let alreadyVoted = CheckIfAlreadyVoted(imdbid)
    
    if (alreadyVoted == false) {

        imdbIdsArray.push(imdbid)
        SaveImdb()
        return true
    }
    else {
        alert('Whoops! You have already voted on this movie')
    }
    return false
}

function CheckIfAlreadyVoted(imdbid) {

    let savedVotes = JSON.parse(sessionStorage.getItem('savedImdbid'))
    //savedVotes = getSavedVotes

    if (savedVotes !== null) {
        for (var i = 0; i < savedVotes.length; i++) {
            if (savedVotes[i] == imdbid) {
                return true
            }
        }
    }
    else {
        for (var i = 0; i < imdbIdsArray.length; i++) {
            if (imdbIdsArray[i] == imdbid) {
                return true
            }
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

function SaveImdb() {
    sessionStorage.setItem('savedImdbid', JSON.stringify(imdbIdsArray))
}