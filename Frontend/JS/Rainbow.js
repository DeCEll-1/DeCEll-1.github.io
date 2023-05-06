// How to use:
// <script scr"filepath"></script> //type at the bottom
// Write class="Rainbowbg" for background rainbow
// Write class="Rainbowtxt" for text rainbow 
// <label class="Rainbow"></label>

if (document.getElementsByClassName("Rainbowbg")[0] != undefined) {
    document.getElementsByClassName("Rainbowbg")[0].style.backgroundColor = "red";
}

if (document.getElementsByClassName("Rainbowtxt")[0] != undefined) {
    document.getElementsByClassName("Rainbowtxt")[0].style.color = "red";
}

if (nodeListbg = document.getElementsByClassName("Rainbowborder")) {
    document.getElementsByClassName("Rainbowborder")[0].style.borderColor = "red";

}

window.onload = setInterval(Rainbow, 20)

var colorR = 255;

var colorG = 0;

var colorB = 0;

function Rainbow() {
    var a = 0;

    var b = 1;

    if (colorR == 255 && !(colorG >= 255) && colorB == 0) {
        colorG = colorG + b;
        a = 1;
    }
    else if (colorR <= 255 && colorR != 0 && colorG == 255 && colorB == 0) {
        colorR = colorR - b;
        a = 2;
    }
    else if (colorR <= 255 && colorG == 255 && colorB >= 0 && colorB < 255) {
        colorB = colorB + b;
        a = 3;
    }
    if (colorR == 0 && colorG <= 255 && colorG > 0 && colorB == 255) {
        colorG = colorG - b;
        a = 4;
    }
    else if (colorR < 255 && colorG == 0 && colorB == 255) {
        colorR = colorR + b;
        a = 5;
    }
    else if (colorR == 255 && colorG == 0 && colorB > 0) {
        colorB = colorB - b;
        a = 6;
    }

    if (nodeListtxt = document.getElementsByClassName("Rainbowtxt") != null) {
        var nodeListtxt = document.getElementsByClassName("Rainbowtxt");
        for (let i = 0; i < nodeListtxt.length; i++) {
            nodeListtxt[i].style.color = "rgb(" + colorR + "," + colorG + "," + colorB + ")";
        }
    }

    if (nodeListbg = document.getElementsByClassName("Rainbowbg")) {
        var nodeListbg = document.getElementsByClassName("Rainbowbg");
        for (let i = 0; i < nodeListbg.length; i++) {
            nodeListbg[i].style.backgroundColor = "rgb(" + colorR + "," + colorG + "," + colorB + ")";
        }
    }

    if (nodeListbg = document.getElementsByClassName("Rainbowborder")) {
        var nodeListbg = document.getElementsByClassName("Rainbowborder");
        for (let i = 0; i < nodeListbg.length; i++) {
            nodeListbg[i].style.borderColor = "rgb(" + colorR + "," + colorG + "," + colorB + ")";
        }
    }

    console.log("Renkler: " + colorR + ", " + colorG + ", " + colorB + "\n Hedef rgb: rgb(" + colorR + ", " + colorG + ", " + colorB + ")" + "\n Şuanki if " + a);
}