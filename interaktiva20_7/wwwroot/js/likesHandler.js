

let numberOfLikes = document.querySelector('#likes').textContent
let numberOfDislikes = document.querySelector('#dislikes').textContent

document.querySelector('#thumbs-up').addEventListener('click', function () {
    numberOfLikes++
    document.querySelector('#likes').textContent = numberOfLikes
    document.querySelector('#thumbs-up').disabled = true
    document.querySelector('#thumbs-down').disabled = true
})

document.querySelector('#thumbs-down').addEventListener('click', function () {
    numberOfDislikes++
    document.querySelector('#dislikes').textContent = numberOfDislikes
    document.querySelector('#thumbs-up').disabled = true
    document.querySelector('#thumbs-down').disabled = true
})


