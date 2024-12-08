function inputFileContentToControl(host, fileName, controlID) {
    var file = new XMLHttpRequest();
    file.open("GET", host + fileName, true);
    file.onreadystatechange = function () {
        if (file.readyState === 4) {  // Makes sure the document is ready to parse
            if (file.status === 200) {  // Makes sure it's found the file
                text = file.responseText;
                document.getElementById(controlID).innerHTML = text;
            }
        }
    }
}