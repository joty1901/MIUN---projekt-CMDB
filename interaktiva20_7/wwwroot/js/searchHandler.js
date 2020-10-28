const menuSearchDiv = document.getElementById('menuSearchDiv')
const menuSearchResult = document.getElementById('menuSearchResult')
menuSearchResult.style.display = 'none'
let searchString = ''

document.querySelector('body').addEventListener('keyup', function updateSearchString1() {
    searchString = document.getElementById('search').value;
})

document.getElementById('searchBtn').addEventListener('click', function search() {
    window.location = "/search?id=" + searchString
})

document.getElementById('menuSearchBtn').addEventListener('click', function searchBtnPressed() {
    if (menuSearchDiv.style.maxWidth == '100%') {
        menuSearchDiv.style.maxWidth = '0'
    } else {
        menuSearchDiv.style.maxWidth = '100%'
    }
})


document.getElementById('menuSearchBar').addEventListener('keydown', function checkIfEnter(event) {
    let searchString = this.value;
    if (event.keyCode == 13) {
        window.location = "/search?id=" + searchString
    }
} )



