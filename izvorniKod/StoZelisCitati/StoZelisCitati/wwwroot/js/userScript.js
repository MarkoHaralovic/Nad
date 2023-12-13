let head = document.querySelector('.header-container');
let leftSide = document.querySelector('#left');
let rightSide = document.querySelector('#right');
let loadHeader2 = function() {
    fetch('../views/header.html')
        .then(response => response.text())
        .then(html => {
            head.innerHTML = html;
            //console.log(html);
        })
        .catch(error => {
        console.log("Unable to fetch header: " + error);
        });
}

let pubsForUser = function() {
    //dohvati svaku objavu korisnika i izlistaj
}

let logout = function() {
    localStorage.setItem('sessionToken',null);
    window.location.href('./home/index.html');
}

let isLoggedIn = function() {
    return localStorage.getItem('sessionToken') !== null;
}

loadHeader2();
pubsForUser();


  