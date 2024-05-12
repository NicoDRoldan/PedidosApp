// Simulated data for menu items
const menuItems = [
    { name: "Burger", price: 5.99 },
    { name: "Fries", price: 2.99 },
    { name: "Drink", price: 1.99 }
];

// Function to display menu items
function displayMenuItems() {
    const menuSection = document.getElementById("menu");
    menuItems.forEach(item => {
        const itemElement = document.createElement("div");
        itemElement.classList.add("item");
        itemElement.innerHTML = `
            <img src="burger.jpg" alt="${item.name}">
            <h3>${item.name}</h3>
            <p>$${item.price}</p>
            <button>Add to Cart</button>
        `;
        menuSection.appendChild(itemElement);
    });
}

// Function to initialize the app
function init() {
    displayMenuItems();
}

// Initialize the app
init();
