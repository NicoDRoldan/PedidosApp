document.addEventListener('DOMContentLoaded', function () {
    var couponModal = document.getElementById("couponAddedModal");
    var acceptButton = document.getElementById("salir-cupon-modal");

    function showCouponModal() {
        couponModal.style.display = "block";
        couponModal.classList.add('scaleIn');
    }

    function hideCouponModal() {
        couponModal.classList.remove('scaleIn');
        couponModal.classList.add('scaleOut');
        setTimeout(function () {
            couponModal.style.display = "none";
            couponModal.classList.remove('scaleOut');
        }, 0); // Tiempo de la animación scaleOut
    }

    // Agrega evento a cada tarjeta de cupón
    document.querySelectorAll('.card').forEach(function (card) {
        card.addEventListener('click', function () {
            showCouponModal();
        });
    });

    // Agrega evento al botón "Aceptar"
    acceptButton.addEventListener('click', function () {
        hideCouponModal();
    });
});
