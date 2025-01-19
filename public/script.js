const timerId = setInterval(changeColor, 20);
let red = 255;
let blue = 0;
let green = 0;
let ccolor = "red"
var checking = false

function changeColor() {

    if(ccolor == "lastRed"){
    red = red + Math.floor(Math.random() * -10);
    }
    else if(ccolor == "lastGreen"){
    green = green + Math.floor(Math.random() * -10);
    }
    else if(ccolor == "red"){
    red = red + Math.floor(Math.random() * 10);
    }
    else if(ccolor == "green"){
    green = green + Math.floor(Math.random() * 10);
    }
    
    if (red >= 255 && ccolor == "red") {
        ccolor = "lastRed"
    }
    else if (green >= 220 && ccolor == "green") {
        ccolor = "lastGreen"
    }
    else if (green <= 90 && ccolor == "lastGreen") {
        ccolor = "red"
        green = 0
        red = 90
    }
    else if (red <= 90 && ccolor == "lastRed") {
        ccolor = "green"
        red = 0
        blue = 90
    }

    document.getElementById("h1").style.color = "rgb(" + red + ", " + green + ", " + blue + ")";
    document.getElementById("h2").style.color = "rgb(" + red + ", " + green + ", " + blue + ")";
    document.getElementById("h3").style.color = "rgb(" + red + ", " + green + ", " + blue + ")";
    document.getElementById("h4").style.color = "rgb(" + red + ", " + green + ", " + blue + ")";
    document.getElementById("h5").style.color = "rgb(" + red + ", " + green + ", " + blue + ")";
}
const button = document.querySelector('button');
button.addEventListener('click', function() {
  window.location.href = 'public/quest.html';
});
