const startCoords = document.getElementById("On"),
    stopCoords = document.getElementById("Off");

var array = [];

startCoords.addEventListener('click', async function () {
    webgazer.begin();
});

stopCoords.addEventListener('click', async function () {
    webgazer.pause();
    webgazer.showPredictionPoints(false);
    webgazer.showVideoPreview(false);
    webgazer.end();
    download(JSON.stringify(array), "coordinates.json", "text/plain");
});

window.onload = async function () {
    await webgazer.setRegression('ridge').setTracker('clmtrackr').setGazeListener(function (data, clock) {

        if (data == null) {
            return;
        }

        var xprediction = Math.round(data.x);
        var yprediction = Math.round(data.y);

        xprediction = (xprediction < 0) ? 0 : xprediction;
        yprediction = (yprediction < 0) ? 0 : yprediction;

        array.push({ X: xprediction, Y: yprediction, Time: Math.round(clock) });

    }).saveDataAcrossSessions(true)

    webgazer.showVideoPreview(false).showPredictionPoints(true).applyKalmanFilter(true);
};

function download(data, filename, type) {
    var file = new Blob([data], {type: type});
    if (window.navigator.msSaveOrOpenBlob)
        window.navigator.msSaveOrOpenBlob(file, filename);
    else {
        var a = document.createElement("a"),
                url = URL.createObjectURL(file);
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        setTimeout(function() {
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);  
        }, 0); 
    }
}