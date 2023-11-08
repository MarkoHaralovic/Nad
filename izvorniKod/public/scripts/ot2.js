let uredKni = JSON.parse(localStorage.getItem("urediKnjigu")) || 0;
var kos = JSON.parse(localStorage.getItem("obrisani")) || [];

var tk = JSON.parse(localStorage.getItem("obrisani"))

let render = function() {

    if (uredKni == 1) {
    
       const ix = document.querySelectorAll('.extra');
       const iupi =  document.querySelectorAll('.extra2');
       
       ix.forEach(function(xi) {
       

        xi.style.display = "flex";
       });

       iupi.forEach(function(iup) {
       

        iup.style.display = "none";
       });

    } else if (uredKni == 2) {
    
       const ix = document.querySelectorAll('.extra');
       const iupi =  document.querySelectorAll('.extra2');
       
       ix.forEach(function(xi) {
       

        xi.style.display = "none";
       });

        iupi.forEach(function(iup) {
       

            iup.style.display = "flex";
        });
    }

    for(let i = 0; i < kos.length; i++) {

        document.getElementById(kos[i]).style.display = "none";
    }
};

let remove = function(id) {

    let ide = id.substring(1, 3); 

    document.getElementById(ide).style.display = "none";

    console.log(ide);
    kos.push(ide);
    localStorage.setItem("obrisani", JSON.stringify(kos));
}
let updateKn = function(id) {

    let ide = id.substring(1, 3);

    console.log(ide);
    localStorage.setItem("tk", JSON.stringify(ide));

    window.location.href = "/Nad/Nad/izvorniKod/views/promjenaKnjige.html";
}

render();