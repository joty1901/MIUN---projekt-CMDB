
const fullPlot = document.getElementById('plot').textContent
document.queryselector('se-more')
let shortPlot = fullPlot


if (fullPlot.length > 200) {
    document.querySelectorAll('plot').textContent = truncate(shortPlot, 200)
}
else {

}

function truncate(str, n) {
    return (str.length > n) ? str.substr(0, n - 1) + '...' : str;
};