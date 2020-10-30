var menuSearchDiv = document.getElementById('menuSearchDiv');
var menuSearchResult = document.getElementById('menuSearchResult').style;
menuSearchResult.display = 'inline-block';
let searchString = '';

document.querySelector('body').addEventListener('keyup', function updateSearchString1() {
    searchString = document.getElementById('search').value;
    searchString1 = document.getElementById('menuSearchBar').value;
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


document.getElementById('menuSearchBar').addEventListener('keyup', function checkIfEnter(event) {
    let searchString = this.value
    if (event.keyCode == 13) {
        window.location = "/search?id=" + searchString
    }
    console.log(searchString1)
    var list, li, i
    filter = searchString.toUpperCase()
    list = document.getElementById("myList")
    li = list.getElementsByTagName('li')

    for (i = 0; i < li.length; i++) {
        blockquote = li[i].getElementsByTagName('blockquote')[0];
        txtValue = blockquote.textContent
        console.log(txtValue)
        console.log(searchString)
        if (txtValue.toUpperCase().indexOf(filter) > -1 && txtValue.toUpperCase().includes(filter)) {
            li[i].style.display = "block;"
            console.log('ListItem[' + i + '] has been changed to inline-block')
        } else {
            li[i].style.display = "none";
            console.log('ListItem[' + i + '] has been changed to none')
        }
    }




} )



