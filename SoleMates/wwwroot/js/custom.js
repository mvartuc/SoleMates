const search = document.getElementById("registerLocation");
const matchList = document.getElementById("datalistOptions");


const outPutHtml = matches => {
    if (matches.length > 0) {
        const html = matches.map(match =>
            `<li id="registerValues" value="${match.stateCode}">${match.cityName}, ${match.stateCode}</li>`)
            .join("");
        matchList.innerHTML = html;
    }
}


$(document).ready(function () {
    bsCustomFileInput.init()
})

