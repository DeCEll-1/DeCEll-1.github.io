function AdDegisimi(a) {
    if (a.value.length < 4) {
        a.style.borderColor = "red";
        document.getElementById("lbl_AdError").innerHTML = "E-posta en az 4 karakter olmalıdır";
        document.getElementById("lbl_AdError").className = "error";
    }
    else {
        if (a.value.includes("@") && a.value.includes(".com")) {
            a.style.borderColor = "green";
            document.getElementById("lbl_AdError").innerHTML = "";
            document.getElementById("lbl_AdError").className = "";
        }
        else {
            a.style.borderColor = "yellow";
            document.getElementById("lbl_AdError").innerHTML = "E-posta '@' ve '.com' içermelidi";
            document.getElementById("lbl_AdError").className = "error";
        }
    }
}

function SifreDegisimi(a) {
    if (a.value.length < 8) {
        a.style.borderColor = "red";
        document.getElementById("lbl_SifreError").innerHTML = "Şifre en az 8 karakter olmalıdır";
        document.getElementById("lbl_SifreError").className = "error";
    }
    else {
        a.style.borderColor = "green";
        document.getElementById("lbl_SifreError").innerHTML = "";
        document.getElementById("lbl_SifreError").className = "";
    }
}

var a = false;

function eyeclick() {

    if (a) {
        document.getElementById("passwordbutton").innerHTML = "<i class='fa-solid fa-eye-slash'></i>";
        document.getElementById("sifre").type = "password"

        a = false;
    }
    else {
        document.getElementById("passwordbutton").innerHTML = "<i class='fa-solid fa-eye'></i>";
        document.getElementById("sifre").type = "text"
        a = true;
    }


}