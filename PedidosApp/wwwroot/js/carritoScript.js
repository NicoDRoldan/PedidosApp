var cartButton = document.getElementById('cartButton');
var cartModal = document.getElementById('cartModal');
var closeModal = document.getElementById('closeModal');

cartButton.onclick = function () {
    displayCart();
    cartModal.style.display = 'block';
}

closeModal.onclick = function () {
    cartModal.style.display = 'none';
}

window.onclick = function (event) {
    if (event.target == cartModal) {
        cartModal.style.display = 'none';
    }
}

updateCartCount();

function addToCart(rubro_categoria, id, name, price) {

    var selector = `#card-${id}-${rubro_categoria} .image-art`;
    var imageUrl = document.querySelector(selector).getAttribute('data-url');
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let existingItem = cart.find(item => item.id === id);

    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({ id, name, price, imageUrl, quantity: 1 });
    }

    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();
}

function displayCart() {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let cartItemsContainer = document.getElementById('cartItems');
    cartItemsContainer.innerHTML = '';
    let total = 0;

    cart.forEach(item => {
        let itemTotal = item.quantity * item.price;
        total += itemTotal;
        cartItemsContainer.innerHTML += `
        <div class="card-cart">
            <div class="card-cart-content">
                <div class="card-cart-text">
                    <div>
                        <h3>${item.name}</h3>
                    </div>
                    <div>
                        <p>Precio: $${item.price.toFixed(2)}</p>
                        <p>Total: $${itemTotal.toFixed(2)}</p>
                    </div>
                </div>
                <div class="card-cart-image">
                    <img src="${item.imageUrl}" alt="${item.name}" class="cart-item-image">
                    <div class="quantity-controls">
                        ${item.quantity === 1 ? `<button class="quantity-button" onclick="removeFromCart(${item.id})"><i class="fas fa-trash-alt"></i></button>` : `<button class="quantity-button" onclick="updateItemQuantity(${item.id}, ${item.quantity - 1})">-</button>`}
                        <span class="quantity-display">${item.quantity}</span>
                        <button class="quantity-button" onclick="updateItemQuantity(${item.id}, ${item.quantity + 1})">+</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    });

    totalAmount.innerText = `Total: $${total.toFixed(2)}`;
}

function updateItemQuantity(id, quantity) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let item = cart.find(item => item.id === id);

    if (item) {
        item.quantity = quantity;
        if (item.quantity <= 0) {
            cart = cart.filter(item => item.id !== id);
        }
        localStorage.setItem('cart', JSON.stringify(cart));
        displayCart();
    }
    updateCartCount()
}

function removeFromCart(id) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    cart = cart.filter(item => item.id !== id);
    localStorage.setItem('cart', JSON.stringify(cart));
    displayCart();
    updateCartCount()
}

function updateCartCount() {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let count = cart.reduce((sum, item) => sum + item.quantity, 0);
    document.getElementById('cartCount').innerText = count;
}