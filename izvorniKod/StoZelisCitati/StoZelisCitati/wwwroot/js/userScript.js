let head = document.querySelector('.header-container');
let leftSide = document.querySelector('#left');
let rightSide = document.querySelector('#right');
let loadHeader2 = function() {
    fetch('../views/header2.html')
        .then(response => response.text())
        .then(html => {
            head.innerHTML = html;
            //console.log(html);
        })
        .catch(error => {
        console.log("Unable to fetch header2: " + error);
        });
}

let pubsForUser = function() {
    fetch('../views/objava.html')
    .then(response => response.text())
    .then(html => {
        leftSide.innerHTML = html;
    }).catch(error => {
        console.log("Couldnt fetch users publications: " + error);
    });
    
}

let logout = function() {
    localStorage.setItem('sessionToken',null);
    window.location.href('../views/index.html');
}

let isLoggedIn = function() {
    return localStorage.getItem('sessionToken') !== null;
}

loadHeader2();
pubsForUser();


  