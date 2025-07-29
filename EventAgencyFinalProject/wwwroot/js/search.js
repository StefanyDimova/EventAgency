document.addEventListener("DOMContentLoaded", function () {
    let searchBar = document.getElementById("searchBar");

    function filterProducts() {
        let searchValue = searchBar.value.toLowerCase();
        let productCards = document.querySelectorAll(".product-card");

        productCards.forEach(product => {
            let productName = product.querySelector(".card-title").textContent.toLowerCase();

            let matchesSearch = searchValue === "" || productName.includes(searchValue);

            if (matchesSearch) {
                product.style.display = "block";
            } else {
                product.style.display = "none";
            }
        });
    }

    searchBar.addEventListener("keyup", filterProducts);
});
