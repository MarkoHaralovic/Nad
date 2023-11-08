let rezZanr = 0;
//let rezTop = 0;
let logIn = JSON.parse(localStorage.getItem("logIn"));
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

let plusLog = document.getElementById("plus");
let logoLog = document.getElementById("logIn");

let container = document.querySelector('.header-container');

let loadHeader = function() {
    fetch('../views/header1.html')
            .then(response => response.text())
            .then(html => {
                container.innerHTML = html;
                searchButton();
            }).catch(error => {
                console.error("Error loading content:", error);
            });
}

let searchButton = function() {
    searchImg = document.getElementById('povecalo');
    searchImg.addEventListener('click', () => {
        fetch('../views/trazi.html')
        .then(data => data.text())
        .then(html => {
            //center.innerHTML = '';
            centerTop.innerHTML = html;
        });
    });
}

let log = function() {

    if (logIn == null) {
    
        logIn = 0;
    }

    if (logIn == 0) {
        
        plusLog.style.display = "none";
        logoLog.style.display = "flex";

    } else if (logIn == 1) {
    
        plusLog.style.display = "flex";
        logoLog.style.display = "none";
    }

    console.log(logIn + " logIn");
}

let login = function() {

    logIn = 1;
    localStorage.setItem("logIn", JSON.stringify(logIn));
    log();
    window.location.href = "../views/index.html";
}

let logout = function() {

    logIn = 0;
    localStorage.setItem("logIn", JSON.stringify(logIn));
    log();
    window.location.href = "/Nad/Nad/izvorniKod/views/index.html";
}
let showZanr = function() {

    if (rezZanr == 0) {
    
        mapa.style.display = "none";

        fetch('../views/genreList.html')
        .then(response => response.text())
        .then(html => {
            listaZ.innerHTML = html;
            listaZ.style.display = "flex";
            maptxt.style.display = 'none';
            zanrButtonListener();
        }).catch(error => {
            console.log("Error loading content:"+error);
        });
        
        rezZanr = 1;

    } else if (rezZanr == 1) {
        mapa.style.display = "flex";
        listaZ.style.display = "none";
        maptxt.style.display = 'grid';
        rezZanr = 0;
    }
};
/*let showTop = function() {

    if (rezTop == 0) {
    
        mapa.style.display = "none";
        listaZ.style.display = "none"
        listaT.style.display = "flex"
        rezZanr = 0;
        rezTop = 1;

    } else if (rezTop == 1) {
    
        mapa.style.display = "flex";
        listaZ.style.display = "none";
        listaT.style.display = "none"
        rezZanr = 0;
        rezTop = 0;
    }
};*/

let brisiKnjigu = function() {

    uredKn = 1;
    localStorage.setItem("urediKnjigu", JSON.stringify(uredKn));

    window.location.href = "/Nad/Nad/izvorniKod/views/urediKnjigu.html";
}

let urediKnjigu = function() {

    uredKn = 2;
    localStorage.setItem("urediKnjigu", JSON.stringify(uredKn));
    window.location.href = "/Nad/Nad/izvorniKod/views/urediKnjigu.html";
}

let dodajKnjigu = function() {

    uredKn = 3;
    localStorage.setItem("urediKnjigu", JSON.stringify(uredKn));
    window.location.href = "/Nad/Nad/izvorniKod/views/promjenaKnjige.html";
}

let zanrButtonListener = function() {
    btnZanrs = document.querySelectorAll('.btnZanr');
    //console.log("test"); 
    console.log(btnZanrs);
    btnZanrs.forEach((element,index) => {
        //console.log("foreach");
        element.addEventListener('click', () => {
            //console.log("clicked "+index);
            
            //TODO: ovisno o kliknutom gumbu treba izbacit tu kategoriju knjiga. Trebamo popricat sa backendom o ovom
            //za sad ce ovdje biti demonstracija kako ce izgledati output
            fetch('../views/objava.html')
            .then(response => response.text())
            .then(html => {
                centerTop.style.display = 'none';
                center.style.alignItems = 'stretch';
                center.style.justifyContent = 'left';
                center.style.paddingLeft = '15px';
                center.style.flexDirection = 'row';
                center.style.justifyContent = 'space-between';
                leftSide.innerHTML = html;
                console.log(leftSide.innerHTML + "RAZMAK");

                publicationEventListener();
            })
            .catch(error => {
                console.log("Failed to load category: "+ error);
            });
        });
    });
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

loadHeader();
log();


