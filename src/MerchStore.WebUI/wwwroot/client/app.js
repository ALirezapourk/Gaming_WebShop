// Configuration
const API_CONFIG = {
    baseUrl: 'http://localhost:5004',  // Update with your API's URL
    endpoints: {
        products: '/api/basic/products'
    },
    headers: {
        'X-API-Key': 'API_KEY',  // Update with your API key
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
};

// DOM Elements
const apiStatus = document.getElementById('api-status');
const productsGrid = document.getElementById('products-grid');
const productModal = document.getElementById('product-modal');
const productDetails = document.getElementById('product-details');
const closeButton = document.querySelector('.close-button');
const productCardTemplate = document.getElementById('product-card-template');

// Event Listeners
document.addEventListener('DOMContentLoaded', initialize);
closeButton.addEventListener('click', closeModal);
window.addEventListener('click', (event) => {
    if (event.target === productModal) {
        closeModal();
    }
});

/**
 * Initialize the application
 */
function initialize() {
    // Always render the UI first, regardless of API connectivity
    renderWelcomeMessage();

    // Then attempt to connect to the API
    fetchProducts()
        .then(products => {
            updateApiStatus('connected', 'Connected to API');
            renderProductGrid(products);
        })
        .catch(error => {
            console.error('Error connecting to API:', error);
            updateApiStatus('error', `API Connection Error: ${error.message}`);
            renderErrorMessage('Could not load products from the API. Please try again later.');
        });
}

/**
 * Render a welcome message in the products grid
 */
function renderWelcomeMessage() {
    productsGrid.innerHTML = `
        <div class="welcome-message">
            <h3>Welcome to the MerchStore Client Demo!</h3>
            <p>This application demonstrates how to consume the MerchStore API from a JavaScript client.</p>
            <p>Loading products from the API...</p>
        </div>
    `;
}

/**
 * Render an error message in the products grid
 */
function renderErrorMessage(message) {
    productsGrid.innerHTML = `
        <div class="error-message">
            <h3>Oops! Something went wrong</h3>
            <p>${message}</p>
            <button onclick="initialize()" class="retry-button">Retry</button>
        </div>
    `;
}

/**
 * Update the API status indicator
 */
function updateApiStatus(status, message) {
    apiStatus.className = `status-indicator ${status}`;
    apiStatus.textContent = message;
}

/**
 * Fetch all products from the API
 */
async function fetchProducts() {
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.products}`, {
        method: 'GET',
        headers: API_CONFIG.headers
    });

    if (!response.ok) {
        throw new Error(`API returned status: ${response.status}`);
    }

    return await response.json();
}

/**
 * Fetch a specific product by ID
 */
async function fetchProductById(productId) {
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.products}/${productId}`, {
        method: 'GET',
        headers: API_CONFIG.headers
    });

    if (!response.ok) {
        throw new Error(`API returned status: ${response.status}`);
    }

    return await response.json();
}

/**
 * Render the products grid
 */
function renderProductGrid(products) {
    // Clear the loader
    productsGrid.innerHTML = '';

    // Handle empty products array
    if (!products || products.length === 0) {
        productsGrid.innerHTML = '<p>No products available.</p>';
        return;
    }

    // Create a document fragment to improve performance
    const fragment = document.createDocumentFragment();

    // Create product cards
    products.forEach(product => {
        const productCard = createProductCard(product);
        fragment.appendChild(productCard);
    });

    // Append all products to the grid
    productsGrid.appendChild(fragment);
}

/**
 * Create a product card from the template
 */
function createProductCard(product) {
    // Clone the template
    const productCard = productCardTemplate.content.cloneNode(true);

    // Set product data
    const image = productCard.querySelector('.product-image img');
    if (product.image_url) {
        image.src = product.image_url;
        image.alt = product.name;
    } else {
        image.src = 'https://via.placeholder.com/300x200?text=No+Image';
        image.alt = 'No image available';
    }

    productCard.querySelector('.product-name').textContent = product.name;
    productCard.querySelector('.product-price').textContent = `${product.price} ${product.currency}`;

    const stockElement = productCard.querySelector('.product-stock');
    if (product.in_stock) {
        stockElement.textContent = `In Stock (${product.stock_quantity})`;
        stockElement.classList.add('in-stock');
    } else {
        stockElement.textContent = 'Out of Stock';
        stockElement.classList.add('out-of-stock');
    }

    // Add event listener to view details button
    const viewDetailsButton = productCard.querySelector('.view-details-button');
    viewDetailsButton.addEventListener('click', () => openProductDetails(product.id));

    return productCard;
}

/**
 * Open the product details modal
 */
async function openProductDetails(productId) {
    try {
        // Show loading state
        productDetails.innerHTML = '<div class="loader"></div>';
        productModal.style.display = 'block';

        // Fetch the product details
        const product = await fetchProductById(productId);

        // Render the product details
        productDetails.innerHTML = `
            <h2>${product.name}</h2>
            ${product.image_url ?
                `<img src="${product.image_url}" alt="${product.name}" class="product-details-image">` :
                '<div class="no-image">No image available</div>'}
            <p class="product-description">${product.description}</p>
            <div class="product-meta">
                <div class="product-price-details">
                    <strong>Price:</strong> ${product.price} ${product.currency}
                </div>
                <div class="product-stock-details">
                    <strong>Stock:</strong>
                    <span class="${product.in_stock ? 'in-stock' : 'out-of-stock'}">
                        ${product.in_stock ? `In Stock (${product.stock_quantity})` : 'Out of Stock'}
                    </span>
                </div>
            </div>
        `;
    } catch (error) {
        console.error('Error fetching product details:', error);
        productDetails.innerHTML = `
            <div class="error-message">
                <h3>Error Loading Product</h3>
                <p>Could not load product details. Please try again later.</p>
            </div>
        `;
    }
}

/**
 * Close the product details modal
 */
function closeModal() {
    productModal.style.display = 'none';
}