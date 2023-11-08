var tk = JSON.parse(localStorage.getItem("tk"));
let uredK = JSON.parse(localStorage.getItem("urediKnjigu")) || 0;

let ispis = function() {

    if (uredK == 2) {
       let str = "Promjena podataka <br> knjige " + tk; 
       document.getElementById('h2').style.display = "flex";
       document.getElementById('h22').style.display = "none";
       document.getElementById('h2').innerHTML = str;

       document.getElementById('btnTrazi1').style.display = "flex";
       document.getElementById('btnTrazi2').style.display = "none";

    }  else if (uredK == 3) {

       document.getElementById('h2').style.display = "none";
       document.getElementById('h22').style.display = "flex";
       

       document.getElementById('btnTrazi1').style.display = "none";
       document.getElementById('btnTrazi2').style.display = "flex";
    } 
}
ispis();