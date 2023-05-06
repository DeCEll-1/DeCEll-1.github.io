function inputvalidate(a) {
    var text = a.value;

    if (text.length < 4) {
        a.style.borderColor = "red";
    }
    else {
        if (text.includes("@")) {
            a.style.borderColor = "green";
        }
        else {
            a.style.borderColor = "yellow";
        }
    }
}
