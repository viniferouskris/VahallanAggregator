//namespace Vahallan_Ingredient_Aggregator.wwwroot.js
//{
//    public class externalRecipes
//    {
//    }
//}
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('recipe-search');
    const searchButton = document.getElementById('search-button');
    const randomButton = document.getElementById('load-random');
    const resultsContainer = document.getElementById('search-results');
    const loadingSpinner = document.getElementById('loading-spinner');
    const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

    // Load random recipes when the page loads
    loadRandomRecipes();

    // Event listeners
    searchButton.addEventListener('click', handleSearch);
    searchInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') handleSearch();
    });
    randomButton.addEventListener('click', loadRandomRecipes);

    function showLoading() {
        loadingSpinner.classList.remove('d-none');
    }

    function hideLoading() {
        loadingSpinner.classList.add('d-none');
    }

    async function handleSearch() {
        const searchTerm = searchInput.value.trim();
        if (!searchTerm) return;

        showLoading();
        try {
            const response = await fetch(`/Recipe/SearchExternal?searchTerm=${encodeURIComponent(searchTerm)}`);
            const data = await response.json();
            displayResults(data.results);
        } catch (error) {
            console.error('Search failed:', error);
            resultsContainer.innerHTML = `
                <div class="col-12 alert alert-danger">
                    Failed to search recipes. Please try again.
                </div>`;
        } finally {
            hideLoading();
        }
    }

    async function loadRandomRecipes() {
        showLoading();
        try {
            const response = await fetch('/Recipe/GetRandomRecipes');
            const data = await response.json();
            displayResults(data.results);
        } catch (error) {
            console.error('Failed to load random recipes:', error);
            resultsContainer.innerHTML = `
                <div class="col-12 alert alert-danger">
                    Failed to load recipes. Please try again.
                </div>`;
        } finally {
            hideLoading();
        }
    }

    async function importRecipe(externalId) {
        try {
            const response = await fetch('/Recipe/ImportExternalRecipe', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': antiForgeryToken
                },
                body: JSON.stringify({ id: externalId })
            });

            if (!response.ok) throw new Error('Import failed');

            // Disable the import button and update UI
            const button = document.querySelector(`button[data-recipe-id="${externalId}"]`);
            if (button) {
                button.disabled = true;
                button.textContent = 'Recipe Imported';
                button.classList.replace('btn-primary', 'btn-success');
            }
        } catch (error) {
            console.error('Import failed:', error);
            alert('Failed to import recipe. Please try again.');
        }
    }

    function displayResults(recipes) {
        if (!recipes || recipes.length === 0) {
            resultsContainer.innerHTML = `
            <div class="col-12 text-center">
                <p>No recipes found. Try a different search term.</p>
            </div>`;
            return;
        }

        resultsContainer.innerHTML = recipes.map(recipe => `
        <div class="col-md-4 mb-4">
            <div class="card h-100">
                ${recipe.thumbnailUrl ?
                `<img src="${recipe.thumbnailUrl}" class="card-img-top" alt="${recipe.name}" 
                          style="height: 200px; object-fit: cover;">` :
                ''}
                <div class="card-body">
                    <h5 class="card-title">${recipe.name}</h5>
                    <p class="card-text">
                        <small class="text-muted">
                            ${recipe.category || ''} ${recipe.area ? `• ${recipe.area}` : ''}
                        </small>
                    </p>
                    <p class="card-text">
                        <small>${recipe.description}</small>
                    </p>
                    <button 
                        class="btn ${recipe.isImported ? 'btn-success' : 'btn-primary'} w-100"
                        onclick="importRecipe('${recipe.externalId}')"
                        data-recipe-id="${recipe.externalId}"
                        ${recipe.isImported ? 'disabled' : ''}>
                        ${recipe.isImported ? 'Recipe Imported' : 'Import Recipe'}
                    </button>
                </div>
            </div>
        </div>
    `).join('');
    }
    // Expose importRecipe to global scope for onclick handler
    window.importRecipe = importRecipe;
});