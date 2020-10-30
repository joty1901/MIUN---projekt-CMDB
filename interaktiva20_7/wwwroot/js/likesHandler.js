
let numberOfLikes = document.querySelector('#likes').textContent
let numberOfDislikes = document.querySelector('#dislikes').textContent
let imdbid = document.getElementById('hidden-imdbid').value
var imdbIdsArray = new Array()
//TODO: Fixa så att listan sparas med hjälp av sessions

document.querySelector('#thumbs-up').addEventListener('click', async function() {
    const like = 'like'
    let result = await sendLike(imdbid, like)
    document.querySelector('#likes').textContent = (parseInt(numberOfLikes) + result)
})

document.querySelector('#thumbs-down').addEventListener('click', async function () {
    const dislike = 'dislike'
    let result = await sendLike(imdbid, dislike)
    document.querySelector('#dislikes').textContent = (parseInt(numberOfDislikes) + result)
})

async function sendLike(imdbid, likeOrDislike) {

    let result = 0
    let alreadyVoted = checkIfAlreadyVote()
    let url = `https://localhost:44313/api/${imdbid}/${likeOrDislike}`

    if (alreadyVoted == false) {
        let response = await fetch(url)

        if (response.status == 200) {
            result++
            imdbIdsArray.push(imdbid)
            document.getElementById('thumbs-up').disabled = true
            document.getElementById('thumbs-down').disabled = true
            document.getElementById('messageLike').innerHTML = 'Your vote is registered'
            return result
        }
        else {
            alert('Something went wrong')
        }
    }
    else {
        document.getElementById('messageLike').innerHTML = 'Whoops! You have already voted on this movie'
    }   
    return result   
}

function checkIfAlreadyVote() {
    for (var i = 0; i < imdbIdsArray.length; i++) {
        if (imdbIdsArray[i] == imdbid) {
            return true
        }
    }
    return false
}