

let numberOfLikes = document.querySelector('#likes').textContent
let numberOfDislikes = document.querySelector('#dislikes').textContent
let imdbid = document.getElementById('hidden-imdbid').value
var imdbIdsArray = new Array("tt0076759", "tt0111161")
//TODO: Fixa så att listan sparas med hjälp av sessions

document.querySelector('#thumbs-up').addEventListener('click', async function() {
    const like = document.getElementById('thumbs-up').value
    let result = await sendLike(imdbid, like)
    document.querySelector('#likes').textContent = result
})

document.querySelector('#thumbs-down').addEventListener('click', async function () {
    const dislike = document.getElementById('thumbs-down').value
    let result = await sendLike(imdbid, dislike)
    document.querySelector('#dislikes').textContent = result
})

async function sendLike(imdbid, likeOrDislike) {

    let newResult = numberOfLikes
    let alreadyVoted = checkIfAlreadyVote()
    let url = `https://localhost:44313/api/${imdbid}/${likeOrDislike}`

    if (alreadyVoted == false) {
        let response = await fetch(url)

        if (response.status == 200) {
            newResult++
            imdbIdsArray.push(imdbid)
            document.getElementById('thumbs-up').disabled = true
            document.getElementById('thumbs-down').disabled = true
            createVoteMessage()
            return newResult
        }
    }
    else {
        alert('Whoops! You have already voted on this movie')
    }   
    return newResult
}

function checkIfAlreadyVote() {
    for (var i = 0; i < imdbIdsArray.length; i++) {
        if (imdbIdsArray[i] == imdbid) {
            return true
        }
    }
    return false
}

function createVoteMessage() {
    document.getElementById('messageLike').innerHTML = 'Your vote is registered'
}