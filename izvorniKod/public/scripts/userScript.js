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

function dropdown() {
    console.log("help")
    let drop = document.getElementById('dropdown-content');
    drop.style.display = 'flex';
    drop.classList.toggle('show');
}

loadHeader2();
pubsForUser();

window.onclick = function(event) {
    if (!event.target.matches('#dropdown-button')) {
      let dropdowns = document.getElementsByClassName('dropdowns');
      for (i = 0; i < dropdowns.length; i++) {
        let openDrop = dropdowns[i];
        if (openDrop.classList.contains('show')) {
          openDrop.classList.remove('show');
        }
      }
    }
}

  