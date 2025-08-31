////possibly not in use

//document.addEventListener('DOMContentLoaded', function () {
//    const gallery = document.getElementById('photoGallery');
//    if (!gallery) return;

//    const mainPhotoContainer = document.getElementById('mainPhoto');
//    const thumbnailContainer = document.getElementById('thumbnailGallery');

//    let photos = [];
//    try {
//        // Get photos from the data attribute
//        const photosData = gallery.dataset.photos;
//        if (photosData) {
//            photos = JSON.parse(photosData);
//        }
//    } catch (error) {
//        console.error('Error parsing photos data:', error);
//        return;
//    }

//    function updateMainPhoto(photoUrl) {
//        if (!mainPhotoContainer) return;
//        mainPhotoContainer.innerHTML = `
//            <img src="${photoUrl}" class="img-fluid rounded" alt="Recipe photo">
//        `;
//    }

//    //function renderThumbnails() {
//    //    if (!thumbnailContainer || !photos.length) return;

//    //    thumbnailContainer.innerHTML = photos.map((photo, index) => `
//    //        <div class="col-4 col-md-3 mb-3">
//    //            <img 
//    //                src="${photo.thumbnailUrl || photo.filePath}" 
//    //                class="img-thumbnail cursor-pointer" 
//    //                alt="Recipe thumbnail"
//    //                onclick="updateMainPhoto('${photo.filePath}')"
//    //            >
//    //        </div>
//    //    `).join('');
//    //}

//    // Initialize the gallery
//    if (photos.length > 0) {
//        // Set the first photo as main photo
//        updateMainPhoto(photos[0].filePath);
//        // Render thumbnails
//       // renderThumbnails();
//    }

//    // Make updateMainPhoto available globally
//    window.updateMainPhoto = updateMainPhoto;
//});