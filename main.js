function inputFileContentToControl(host, fileName, controlID) {
    let file = new XMLHttpRequest();
    let link = host + fileName;
    file.open("GET", link, true);
    file.onreadystatechange = function () {
        if (file.readyState === 4) {  // Makes sure the document is ready to parse
            if (file.status === 200) {  // Makes sure it's found the file
                text = file.responseText;
                document.getElementById(controlID).innerHTML = text;
            }
        }
    }
    file.send();
}