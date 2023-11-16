let container = document.querySelector('.header-container');

let loadHeader = function() {
    fetch('../views/header1.html')
            .then(response => response.text())
            .then(html => {
                container.innerHTML = html;
                console.log(html+"html");
            }).catch(error => {
                console.error("Error loading content:", error);
            });
}

loadHeader();