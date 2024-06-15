document.addEventListener('DOMContentLoaded', function () {
    const menuList = document.getElementById('menuList');
    const toggleMenuButton = document.getElementById('toggleMenu');
    const toggleButtonContainer = document.querySelector('.toggle-button');

    let isCollapsed = true;

    function updateMenuVisibility() {
        if (window.innerWidth >= 300 && window.innerWidth <= 500) {
            menuList.classList.add('collapsed');
            toggleButtonContainer.classList.remove('hidden');
        } else {
            menuList.classList.remove('collapsed', 'expanded');
            toggleButtonContainer.classList.add('hidden');
        }
    }

    toggleMenuButton.addEventListener('click', function () {
        if (isCollapsed) {
            menuList.classList.remove('collapsed');
            menuList.classList.add('expanded');
            toggleMenuButton.textContent = 'Mostrar menos';
        } else {
            menuList.classList.remove('expanded');
            menuList.classList.add('collapsed');
            toggleMenuButton.textContent = 'Mostrar más';
        }
        isCollapsed = !isCollapsed;
    });

    // Debounce function to limit the rate of the resize event handler execution
    function debounce(func, wait) {
        let timeout;
        return function () {
            const context = this, args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(context, args), wait);
        };
    }

    // Initial check
    updateMenuVisibility();
});