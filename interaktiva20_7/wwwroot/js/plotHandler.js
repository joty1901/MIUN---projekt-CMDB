
const fullPlot = document.getElementById('plot').textContent
let shortPlot = truncate(fullPlot, 200)
document.querySelector('#read-more').style.cursor = "pointer";
let readMore = document.querySelector('#read-more')

if (fullPlot.length > 200) {
    document.querySelector('#read-more').style.display = 'block'
    document.getElementById('plot').textContent = shortPlot + readMore
}

document.querySelector('#read-more').addEventListener('click', function () {
    let visiblePlot = document.getElementById('plot').textContent

    if (visiblePlot.length <= 202) {
        readMore.textContent = ' Hide'
        document.getElementById('plot').textContent = fullPlot
    }
    else {
        readMore.textContent = ' Read more'
        document.getElementById('plot').textContent = shortPlot
    }
})

function truncate(str, n) {
    return (str.length > n) ? str.substr(0, n - 1) + '...' : str;
};