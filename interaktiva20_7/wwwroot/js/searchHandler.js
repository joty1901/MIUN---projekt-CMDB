var menuSearchDiv = document.getElementById('menuSearchDiv');
var menuSearchResult = document.getElementById('menuSearchResult').style;
menuSearchResult.display = 'inline-block';
let searchString = '';


document.querySelector('body').addEventListener('keyup', function updateSearchString1() {
    searchString = document.getElementById('search').value;
})

function clickMovie(value) {
    window.location = "/details?id=" + value
}

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

    var list, li, i
    list = document.getElementById('myList')
li = list.getElementsByClassName('listItemsInSearch')

document.getElementById('menuSearchBar').addEventListener('keyup', function checkIfEnter(event) {
    searchString = this.value
    let filter = searchString.toUpperCase()
    let counter = 0
    //searchString1 = document.getElementById('menuSearchBar').value
    if (event.keyCode == 13) {
        window.location = "/search?id=" + searchString
    }
    for (i = 0; i < li.length; i++) {
        blockquote = li[i].getElementsByClassName('movieTitle_test')[0].childNodes[0].nodeValue;



        if (blockquote.toUpperCase().indexOf(filter) > -1 && counter < 5) {
            li[i].style.display = "block !important;"
            li[i].style.cssText = "display:block;"
            counter++;
        } else {
            li[i].style.display = "none";
        }
    }
    list.style.display = 'block'



} )



