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

let container; //header container
let hrUnreg = document.createElement('div');
hrUnreg.innerHTML = '<a href="login.html" style="text-decoration: none; color: white; font-size: 25px;"><p>log in</p></a>'

let hrReg = document.createElement('div');
hrReg.innerHTML = '<p id="myAcc" onclick="logout()">Moj profil</p>';

let loadHeader = function() {
    fetch('../views/header.html')
            .then(response => response.text())
            .then(html => {
                container = document.querySelector('.header-container');
                console.log(container);
                container.innerHTML = html;
                let hr = document.getElementById('header-right');
                if(logIn) {
                    hr.appendChild(hrReg);
                } else {
                    hr.appendChild(hrUnreg);
                }
                //if(logIn) plusLog = document.getElementById('plus');
                //else logoLog = document.getElementById('logIn');
                //if(logIn) console.log(plusLog.innerHTML);
                //else console.log(logoLog.innerHTML);
                searchButton();               
            })
            .then(() => {
                log();
            }).catch(error => {
                console.error("Error loading content:", error);
            });
}

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

let login = function() { //placeholder za simuliranje logina i registracije, koristit cemo loginReq za pravu funkcionalnost
    logIn = 1;
    localStorage.setItem("logIn", JSON.stringify(logIn)); //na backend se salje login request
    
    console.log("login u login je:" + logIn);
    if(logIn) {window.location.href = "../views/userProfile.html";}
}

let regReq = function() {
    const name = document.getElementById('name').value;
    const lastname = document.getElementById('last-name').value;
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const email = document.getElementById('email').value;
    const number = document.getElementById('number').value;
    const cat = document.querySelector('.categories').value;
    const marker = window.marker;
    console.log(marker.getLatLng() + ": marker");

    const data = {
        name : name,
        lastname : lastname,
        username : username,
        passwrod : password,
        email : email,
        number : number,
        cat : cat,
        marker : marker
    }

    //marker.getLanLng()

    if(!(name && lastname && username && password && email && number && cat && marker)) {
        /*const errMsg = document.createElement('p');
        const parent = document.querySelector('.center-box');
        parent.appendChild(errMsg)
        //uredit u css
        errMsg.textContent = 'Popunite sva polja';*/
        alert("Jedno ili više polja je prazno");
        
        return; 
    }
    url = 'neki url'
    fetch(url, {
        method: "POST",
        headers: {

        },
        body: JSON.stringify(data)
    })
    .then(response => response.json())
    .then(data => {
        //podaci sa backenda
    })
    .catch(error => { 
        console.log("Error: ",error);
    })

}

let loginReq = function() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    if(!username || !password) {
        /*const errMsg = document.createElement('p');
        errMsg.textContent = 'Popunite sva polja';
        const parent = document.querySelector('.center-box');
        parent.appendChild(errMsg);*/
        alert("Jedno ili više polja je prazno");
        return; 
    }

    fetch('', {
        method: "POST",
        headers: {

        },
        body: JSON.stringify(data)
    })
    .then(response => response.json())
    .then(data => {
        //podaci sa backenda
    })
    .catch(error => { 
        console.log("Error: ",error);
    })

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



