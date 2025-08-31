document.addEventListener('DOMContentLoaded', function () {
    const galleryData = document.getElementById('photoGallery')?.dataset.photos;
    console.log('Gallery data:', galleryData);

    const gallery = document.getElementById('photoGallery');
    const mainPhotoContainer = document.getElementById('mainPhoto');
    const thumbnailContainer = document.getElementById('thumbnailGallery');

    if (!gallery || !mainPhotoContainer || !thumbnailContainer) {
        console.log('Required elements not found:', { gallery, mainPhotoContainer, thumbnailContainer });
        return;
    }

    // Handle thumbnail clicks
    thumbnailContainer.addEventListener('click', function (e) {
        const clickedImg = e.target.closest('img');
        if (!clickedImg) return;

        console.log('Thumbnail clicked:', clickedImg.src);
        console.log('Full image URL:', clickedImg.dataset.fullImage);

        // Update main photo with smooth transition
        const newMainImg = document.createElement('img');
        newMainImg.src = clickedImg.dataset.fullImage;
        newMainImg.className = 'img-fluid rounded';
        newMainImg.alt = clickedImg.alt;
        newMainImg.style.opacity = '0';
        newMainImg.style.transition = 'opacity 0.3s ease-in';

        // Clear and update main photo container
        mainPhotoContainer.innerHTML = '';
        mainPhotoContainer.appendChild(newMainImg);

        // Trigger reflow and fade in
        newMainImg.offsetHeight;
        newMainImg.style.opacity = '1';

        // Update thumbnail active states
        thumbnailContainer.querySelectorAll('img').forEach(img => {
            img.classList.remove('border-primary');
        });
        clickedImg.classList.add('border-primary');
    });

    // Add hover effect to thumbnails
    thumbnailContainer.querySelectorAll('img').forEach(img => {
        img.addEventListener('mouseenter', function () {
            if (!this.classList.contains('border-primary')) {
                this.classList.add('border-secondary');
            }
        });

        img.addEventListener('mouseleave', function () {
            this.classList.remove('border-secondary');
        });
    });


});