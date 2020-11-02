
const fullPlot = document.getElementById('plot').textContent
let shortPlot = fullPlot

if (fullPlot.length > 200) {
    document.querySelector('#see-more').style.display = 'block'
    let output = truncate(shortPlot, 200)
    document.getElementById('plot').textContent = output
}

function truncate(str, n) {
    return (str.length > n) ? str.substr(0, n - 1) + '...' : str;
};