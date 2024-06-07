/*Leer más*/

document.addEventListener('DOMContentLoaded', function () {
    const descriptions = document.querySelectorAll('.mcd-store-menu-category-item__title');

    descriptions.forEach(desc => {
        const length = desc.getAttribute('data-length');
        const id = desc.id.split('-')[1];
        const readMore = document.getElementById(`read-more-${id}`);

        if (length > 30) {
            readMore.style.display = 'inline';
        } else {
            readMore.style.display = 'none';
        }
    });
});

function toggleDescription(id) {
    const card = document.getElementById(`card-${id}`);
    const desc = document.getElementById(`desc-${id}`);
    const readMore = document.getElementById(`read-more-${id}`);
    const readLess = document.getElementById(`read-less-${id}`);

    if (card.classList.contains('expanded')) {
        // Iniciar animación de repliegue
        desc.classList.remove('expanding');
        desc.classList.add('collapsing');

        // Esperar el fin de la animación para ajustar el contenido
        desc.addEventListener('animationend', function () {
            if (!card.classList.contains('expanded')) {
                desc.style.webkitLineClamp = '1';
                desc.classList.remove('collapsing');
                readMore.style.display = 'inline';
                readLess.style.display = 'none';
            }
        }, { once: true });

        card.classList.remove('expanded');
    } else {
        // Iniciar animación de despliegue
        desc.classList.remove('collapsing');
        desc.classList.add('expanding');

        // Ajustar el contenido al inicio de la animación
        desc.style.webkitLineClamp = 'unset';
        readMore.style.display = 'none';
        readLess.style.display = 'inline';

        card.classList.add('expanded');
    }
}
