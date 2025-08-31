// recipeDetails.js
document.addEventListener('DOMContentLoaded', function () {
    // Get necessary DOM elements
    const photoGallery = document.getElementById('photoGallery');
    const mainPhotoContainer = document.getElementById('mainPhoto');
    const thumbnailContainer = document.getElementById('thumbnailGallery');
    console.log('Photo Gallery Data:', document.getElementById('photoGallery')?.dataset.photos);
    // If we have photo gallery functionality enabled
    if (photoGallery) {
        // Initialize photo gallery
       // initPhotoGallery();

        // Handle thumbnail clicks
        thumbnailContainer?.addEventListener('click', function (e) {
            const thumbnail = e.target.closest('.thumbnail-item');
            if (thumbnail) {
                updateMainPhoto(thumbnail.dataset.photoUrl, thumbnail.dataset.photoId);
            }
        });
    }

    //function initPhotoGallery() {
    //    // Get all photos from the data attribute
    //    const recipePhotos = JSON.parse(photoGallery.dataset.photos || '[]');

    //    if (recipePhotos.length > 0) {
    //        // Find main photo or use first photo
    //        const mainPhoto = recipePhotos.find(p => p.isMain) || recipePhotos[0];

    //        // Render main photo
    //        renderMainPhoto(mainPhoto);

    //        // Render thumbnails
    //      //  renderThumbnails(recipePhotos);
    //    } else {
    //        // Show placeholder if no photos
    //        renderPlaceholder();
    //    }
    //}

    function renderMainPhoto(photo) {
        if (!mainPhotoContainer) return;

        mainPhotoContainer.innerHTML = `
            <img src="${photo.storageUrl}" 
                 alt="Recipe main photo" 
                 class="img-fluid rounded shadow"
                 id="currentMainPhoto"
                 data-photo-id="${photo.id}">
        `;
    }

    //function renderThumbnails(photos) {
    //    if (!thumbnailContainer) return;

    //    const thumbnailsHtml = photos.map(photo => `
    //        <div class="thumbnail-item col-3 mb-2" 
    //             data-photo-id="${photo.id}"
    //             data-photo-url="${photo.storageUrl}">
    //            <img src="${photo.thumbnailUrl}" 
    //                 alt="Recipe thumbnail"
    //                 class="img-thumbnail ${photo.isMain ? 'border-primary' : ''}"
    //                 style="width: 100%; height: 100px; object-fit: cover; cursor: pointer;">
    //        </div>
    //    `).join('');

    //    thumbnailContainer.innerHTML = thumbnailsHtml;
    //}

    function renderPlaceholder() {
        if (!mainPhotoContainer) return;

        mainPhotoContainer.innerHTML = `
            <div class="placeholder-image bg-light d-flex align-items-center justify-content-center rounded"
                 style="height: 300px;">
                <span class="text-muted">No photos available</span>
            </div>
        `;
    }

    function updateMainPhoto(photoUrl, photoId) {
        if (mainPhotoContainer) {
            mainPhotoContainer.innerHTML = `
                <img src="${photoUrl}" 
                     alt="Recipe main photo" 
                     class="img-fluid rounded shadow">
            `;
        }
    }
});