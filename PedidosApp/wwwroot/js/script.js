// Agrega un evento de clic al botón del carrito para abrir el modal
document.getElementById("cart-btn").addEventListener("click", function () {
    document.getElementById("cart-modal").style.display = "block";
});

// Agrega un evento de clic para cerrar el modal cuando se hace clic en la "X"
document.getElementsByClassName("close")[0].addEventListener("click", function () {
    document.getElementById("cart-modal").style.display = "none";
});
