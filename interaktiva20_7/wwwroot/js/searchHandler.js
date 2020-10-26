let searchString

document.querySelector('body').addEventListener('keyup', function updateSearchString() {
    searchString = document.getElementById('search').value
})
document.getElementById('searchBtn').addEventListener('click', function search() {
    window.location = "/search?id=" + searchString
})