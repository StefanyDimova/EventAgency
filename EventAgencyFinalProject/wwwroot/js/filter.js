document.addEventListener("DOMContentLoaded", function () {
    let minPriceInput = document.getElementById("minPrice");
    let maxPriceInput = document.getElementById("maxPrice");

    function filterProducts() {
        let minPrice = parseFloat(minPriceInput.value) || 0;
        let maxPrice = parseFloat(maxPriceInput.value) || Infinity;
        let productCards = document.querySelectorAll(".product-card");

        productCards.forEach(product => {
            let productPrice = parseFloat(product.querySelector(".product-price").textContent.replace(/[^\d.-]/g, ''));

            // Проверка дали цената попада в диапазона
            let matchesPrice = productPrice >= minPrice && productPrice <= maxPrice;

            if (matchesPrice) {
                product.style.display = "block";
            } else {
                product.style.display = "none";
            }
        });
    }

    minPriceInput.addEventListener("input", filterProducts);  // Променяме на всяко въвеждане
    maxPriceInput.addEventListener("input", filterProducts);  // Променяме на всяко въвеждане
});
