// Настройки за pagination
const productsPerPage = 9; // Колко продукта на страница
let currentPage = 1;

// Функция за рендиране на продуктите на съответната страница
function renderProducts(page) {
    // Избираме всички продукти (вземаме всички .product-card елементи)
    const allProducts = document.querySelectorAll('.product-card');

    // Изчисляваме индексите на продуктите, които трябва да се покажат
    const start = (page - 1) * productsPerPage;
    const end = page * productsPerPage;

    // Изчистваме видимите продукти
    allProducts.forEach(product => {
        product.style.display = 'none';
    });

    // Показваме само продуктите за текущата страница
    for (let i = start; i < end && i < allProducts.length; i++) {
        allProducts[i].style.display = 'block';
    }

    renderPagination(allProducts.length);
}

// Функция за рендиране на навигацията за страници
function renderPagination(totalProducts) {
    const totalPages = Math.ceil(totalProducts / productsPerPage);
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = '';

    // Предишна страница
    if (currentPage > 1) {
        pagination.innerHTML += `
            <a href="javascript:void(0);" class="btn btn-primary me-2" onclick="goToPage(${currentPage - 1})">Предишна</a>
        `;
    }

    // Страница по страница
    for (let i = 1; i <= totalPages; i++) {
        pagination.innerHTML += `
            <a href="javascript:void(0);" class="btn btn-secondary me-2 ${i === currentPage ? 'active' : ''}" onclick="goToPage(${i})">${i}</a>
        `;
    }

    // Следваща страница
    if (currentPage < totalPages) {
        pagination.innerHTML += `
            <a href="javascript:void(0);" class="btn btn-primary ms-2" onclick="goToPage(${currentPage + 1})">Следваща</a>
        `;
    }
}

// Функция за промяна на страницата
function goToPage(page) {
    if (page < 1 || page > Math.ceil(document.querySelectorAll('.product-card').length / productsPerPage)) return;
    currentPage = page;
    renderProducts(page);
}

// Зареждаме продуктите при начално зареждане на страницата
document.addEventListener('DOMContentLoaded', function () {
    renderProducts(currentPage); // Показваме първата страница при зареждане
});
