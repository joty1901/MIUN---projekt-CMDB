
let result
let imdbid
let numberOfLikes
let numberOfDislikes
var imdbIdsArray = new Array()

document.querySelectorAll('#thumbs-up').forEach(selectedMovie => {
    selectedMovie.addEventListener('click', async function () {
        const like = 'like'
        imdbId = selectedMovie.accessKey
        result = await sendLike(selectedMovie, imdbId, like)
        UpdateUI(selectedMovie, result, like)
    })
})

document.querySelectorAll('#thumbs-down').forEach(selectedMovie => {
    selectedMovie.addEventListener('click', async function () {
        const dislike = 'dislike'
        imdbId = selectedMovie.accessKey
        result = await sendLike(selectedMovie, imdbId, dislike)
        UpdateUI(selectedMovie, result, dislike)
    })
})

async function sendLike(selectedMovie, imdbid, likeOrDislike) {

    let alreadyVoted = checkIfAlreadyVote(imdbid)
    let url = `https://localhost:44313/api/${imdbid}/${likeOrDislike}`

    if (alreadyVoted == false) {
        let response = await fetch(url)

        if (response.status == 200) {
            result++
            imdbIdsArray.push(imdbid)
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

function checkIfAlreadyVote(imdbid) {
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