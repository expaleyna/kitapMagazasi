// Site-wide JavaScript functionality

// Update cart badge
function updateCartBadge() {
    // This would typically get the cart count from session/API
    // For demo purposes, we'll use a simple approach
    const cartItems = JSON.parse(sessionStorage.getItem('cartItems') || '[]');
    const badge = document.getElementById('cart-badge');
    if (badge) {
        badge.textContent = cartItems.length;
        badge.style.display = cartItems.length > 0 ? 'inline' : 'none';
    }
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    updateCartBadge();
    
    // Auto-hide alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function(alert) {
        setTimeout(function() {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
    
    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });
    
    // Form validation feedback
    const forms = document.querySelectorAll('form');
    forms.forEach(function(form) {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });
    
    // Auto-submit filter form on category change
    const categorySelect = document.getElementById('categoryId');
    if (categorySelect) {
        categorySelect.addEventListener('change', function() {
            // Add a small delay to show the loading state
            const form = this.closest('form');
            if (form) {
                setTimeout(() => {
                    form.submit();
                }, 100);
            }
        });
    }
    
    // Add loading state to filter form
    const filterForm = document.querySelector('.filter-section form');
    if (filterForm) {
        filterForm.addEventListener('submit', function() {
            const submitBtn = this.querySelector('button[type="submit"]');
            if (submitBtn) {
                const originalText = submitBtn.innerHTML;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Aranıyor...';
                submitBtn.disabled = true;
                
                // Re-enable after a short delay if for some reason the page doesn't redirect
                setTimeout(() => {
                    submitBtn.innerHTML = originalText;
                    submitBtn.disabled = false;
                }, 3000);
            }
        });
    }
    
    // Image lazy loading fallback
    const images = document.querySelectorAll('img');
    images.forEach(function(img) {
        img.addEventListener('error', function() {
            if (!this.src.includes('no-image.jpg')) {
                this.src = '/images/no-image.jpg';
            }
        });
    });
    
    // Quantity input validation
    const quantityInputs = document.querySelectorAll('input[type="number"][name="quantity"]');
    quantityInputs.forEach(function(input) {
        input.addEventListener('change', function() {
            const min = parseInt(this.min) || 1;
            const max = parseInt(this.max) || 999;
            let value = parseInt(this.value);
            
            if (isNaN(value) || value < min) {
                this.value = min;
            } else if (value > max) {
                this.value = max;
            }
        });
    });
});

// Search functionality
function performSearch() {
    const searchInput = document.getElementById('search');
    if (searchInput && searchInput.value.trim()) {
        window.location.href = `/kitaps?search=${encodeURIComponent(searchInput.value.trim())}`;
    }
}

// Price range validation
function validatePriceRange() {
    const minPrice = document.getElementById('minPrice');
    const maxPrice = document.getElementById('maxPrice');
    
    if (minPrice && maxPrice) {
        const min = parseFloat(minPrice.value) || 0;
        const max = parseFloat(maxPrice.value) || 0;
        
        if (min > 0 && max > 0 && min > max) {
            alert('Minimum fiyat, maksimum fiyattan büyük olamaz.');
            return false;
        }
    }
    return true;
}

// Add to favorites with animation
function addToFavorites(kitapId, userId) {
    // Add visual feedback
    const button = event.target;
    const originalText = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
    button.disabled = true;
    
    // Simulate API call (in real app, this would be an AJAX call)
    setTimeout(() => {
        button.innerHTML = '<i class="fas fa-check"></i> Eklendi';
        button.classList.remove('btn-outline-danger');
        button.classList.add('btn-success');
        
        setTimeout(() => {
            button.innerHTML = originalText;
            button.classList.remove('btn-success');
            button.classList.add('btn-outline-danger');
            button.disabled = false;
        }, 2000);
    }, 1000);
}

// Cart functionality
function addToCart(kitapId, quantity = 1) {
    // Add visual feedback
    const button = event.target;
    const originalText = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Ekleniyor...';
    button.disabled = true;
    
    // Simulate adding to cart
    setTimeout(() => {
        // Update cart in session storage (demo purposes)
        const cartItems = JSON.parse(sessionStorage.getItem('cartItems') || '[]');
        const existingItem = cartItems.find(item => item.kitapId === kitapId);
        
        if (existingItem) {
            existingItem.quantity += quantity;
        } else {
            cartItems.push({ kitapId, quantity });
        }
        
        sessionStorage.setItem('cartItems', JSON.stringify(cartItems));
        updateCartBadge();
        
        // Reset button
        button.innerHTML = '<i class="fas fa-check"></i> Eklendi';
        button.classList.remove('btn-primary');
        button.classList.add('btn-success');
        
        setTimeout(() => {
            button.innerHTML = originalText;
            button.classList.remove('btn-success');
            button.classList.add('btn-primary');
            button.disabled = false;
        }, 2000);
    }, 1000);
}

// Utility functions
function formatPrice(price) {
    return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
    }).format(price);
}

function showToast(message, type = 'info') {
    // Create toast element
    const toast = document.createElement('div');
    toast.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    toast.style.top = '20px';
    toast.style.right = '20px';
    toast.style.zIndex = '9999';
    toast.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(toast);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (toast.parentNode) {
            toast.parentNode.removeChild(toast);
        }
    }, 5000);
}
