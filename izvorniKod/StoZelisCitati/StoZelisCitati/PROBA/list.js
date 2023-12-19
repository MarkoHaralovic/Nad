let polje = document.getElementById("right");


let lis = function(broj){

    
    broj = broj - 1;
    let poc = broj * 5;
    let kraj = poc + 5;
    let pom = "";

    const br = document.querySelectorAll('.mb');

    for (let i = poc; i < kraj; i++) {

        pom += `<div id="o${i}" class="objava">
                <div class="fot">
                   <img width="100%" height="100%" src="/Nad/Nad/izvorniKod/StoZelisCitati/StoZelisCitati/wwwroot/images/traziLogo.png" alt="">
                </div>

                <div class="inf">
                   <div class="left">

                      <div class="miniDiv"> naziv: ${i} </div>
                
                      <div class="miniDiv"> autor: pisac </div>
                
                   </div>


                   <div class="rig">

                      <div class="miniDiv"> godina: 2002 </div>
                
                      <div class="miniDiv"> izdavac: dučan </div>
                   </div>
                </div> 

               </div>`

    
    }
    polje.innerHTML = pom;
    
    broj = broj + 1;

    if (broj > 4) {
       
      var j = -3;

      br.forEach(function(gumb) {
      
         gumb.id = broj + j;
         gumb.innerHTML = broj + j;
         j++;
      });
    } else {
    
       var j = 1;

        br.forEach(function(gumb) {
      
            gumb.id = j;
            gumb.innerHTML = j;
            j++;
         });
    }
}
lis(1);