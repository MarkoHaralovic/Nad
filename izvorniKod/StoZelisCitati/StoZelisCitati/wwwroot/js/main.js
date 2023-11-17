let rezZanr = 0;
//let rezTop = 0;
//let logIn = JSON.parse(localStorage.getItem("logIn")); //za sad neka bude zakomentirano, simulirat cemo logiranje samo kroz gumbe na login i register
let logIn = 0;
let uredKn = JSON.parse(localStorage.getItem("urediKnjigu"));
var kos = JSON.parse(localStorage.getItem("obrisani"));

let btnZanr = document.getElementById('zanr');
let btnZanrs;
let searchImg;
let publications;
let mapa = document.getElementById('map');
let logBtn = document.getElementById('logInBtn');
let maptxt = document.getElementById('map-text');
let leftSide = document.getElementById('left');
let centerTop = document.getElementById('center-top');
let rightSide = document.getElementById('right');
let center = document.getElementById('center');

let listaZ = document.getElementById('listaZanr');
//let listaT = document.getElementById('listaTop');

let plusLog //= document.getElementById("plus"); //null
let logoLog //= document.getElementById("logIn");

let searchButton = function() {
    //console.log("in searchButton");
    searchImg = document.getElementById('povecalo');
    searchImg.addEventListener('click', () => {
        searchButtonClicked();
    });
}

let searchButtonClicked = function() {
    console.log("in searchButtonClicked");
    fetch('../views/trazi.html')
        .then(data => data.text())
        .then(html => {
            //center.innerHTML = '';
            leftSide.innerHTML = '';
            rightSide.innerHTML = '';
            centerTop.innerHTML = html;
            centerTop.style.display = 'flex';
        })
        .catch(error => {
            console.log("Error while loading element: "+error);
        });
}

let log = function() {
    console.log("login u log je:" + logIn);

    if (logIn == 0) {
        //logoLog = document.getElementById('logIn');
        //plusLog.style.display = "none";
        //logoLog.style.display = "flex";

    } else {
        console.log("else");
        //plusLog = document.getElementById('plus');
        //plusLog.style.display = "flex";
        //logoLog.style.display = "none";
    }
    console.log("tu sam");
    console.log(logIn + " logIn");
}

let publicationEventListener = function() {
    publications = document.querySelectorAll('.objava');
    //console.log(publications[0].innerHTML + " html objavi");
    publications.forEach(element => {
        //console.log("ovdje sam");
        element.addEventListener('click', () => {
            fetch('../views/profilKnjige.html')
            .then(response => response.text())
            .then(html => {
                //console.log(html + "ovdje");
                rightSide.innerHTML = html;
                rightSide.style.paddingTop = '50px';
            })
            .catch(error => {
                console.log("Unable to fetch publication: "+error);
            })
        });
    });
}


