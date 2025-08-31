
//document.addEventListener('DOMContentLoaded', function () {
//    const dropZone = document.getElementById('dropZone');
//    const fileInput = document.getElementById('fileInput');
//    const browseButton = document.getElementById('browseButton');
//    const previewContainer = document.getElementById('previewContainer');
//    const uploadProgress = document.getElementById('uploadProgress');
//    const progressBar = uploadProgress.querySelector('.progress-bar');
//    const form = document.querySelector('form');

//    // Track both existing and new photos
//    let photoMap = new Map(); // For new photos
//    let existingPhotos = new Set(); // For existing photos
//    const existingPhotos = previewContainer.querySelectorAll('[data-photo-id]');
//    existingPhotos.forEach(photo => {
//        const photoId = photo.dataset.photoId;
//        const removeBtn = photo.querySelector('.remove-photo');
//        if (removeBtn) {
//            removeBtn.addEventListener('click', () => {
//                formData.append('RemovedPhotoIds[]', photoId);
//                photo.remove();
//            });
//        }
//    });
//    // Initialize existing photos if any
//    const existingPhotoElements = previewContainer.querySelectorAll('[data-photo-id]');
//    existingPhotoElements.forEach(elem => {
//        existingPhotos.add(elem.dataset.photoId);
//    });

//    // Prevent default drag behaviors
//    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
//        dropZone.addEventListener(eventName, preventDefaults, false);
//        document.body.addEventListener(eventName, preventDefaults, false);
//    });

//    function preventDefaults(e) {
//        e.preventDefault();
//        e.stopPropagation();
//    }

//    ['dragenter', 'dragover'].forEach(eventName => {
//        dropZone.addEventListener(eventName, highlight, false);
//    });

//    ['dragleave', 'drop'].forEach(eventName => {
//        dropZone.addEventListener(eventName, unhighlight, false);
//    });

//    function highlight(e) { dropZone.classList.add('dragover'); }
//    function unhighlight(e) { dropZone.classList.remove('dragover'); }

//    // Handle dropped files
//    dropZone.addEventListener('drop', handleDrop, false);
//    fileInput.addEventListener('change', handleFiles);
//    browseButton.addEventListener('click', () => fileInput.click());

//    function handleDrop(e) {
//        const dt = e.dataTransfer;
//        const files = dt.files;
//        handleFiles({ target: { files } });
//    }

//    function handleFiles(e) {
//        const files = [...e.target.files];
//        files.forEach(previewFile);
//    }

//    function previewFile(file) {
//        if (!file.type.startsWith('image/')) {
//            alert('Please upload only image files.');
//            return;
//        }

//        if (file.size > 5242880) { // 5MB
//            alert('File size should not exceed 5MB.');
//            return;
//        }

//        const reader = new FileReader();
//        reader.onloadend = function () {
//            const previewId = 'preview-' + Date.now();
//            const previewHtml = createPreviewHtml(previewId, reader.result, false);
//            previewContainer.insertAdjacentHTML('beforeend', previewHtml);

//            // Add click handler for remove button
//            const removeBtn = previewContainer.querySelector(`#${previewId} .remove-photo`);
//            removeBtn.addEventListener('click', () => removePreview(previewId));

//            // Store file in Map
//            photoMap.set(previewId, file);
//        }
//        reader.readAsDataURL(file);
//    }

//    function createPreviewHtml(previewId, imgSrc, isExisting = false) {
//        return `
//            <div class="col-md-3 mb-3" id="${previewId}" ${isExisting ? `data-photo-id="${previewId}"` : ''}>
//                <div class="position-relative">
//                    <img src="${imgSrc}" class="img-thumbnail" alt="Preview">
//                    <button type="button" class="btn btn-sm btn-danger remove-photo position-absolute top-0 end-0 m-1">
//                        <i class="bi bi-x"></i>
//                    </button>
//                </div>
//            </div>
//        `;
//    }

//    // Handle photo removal (both new and existing)
//    window.removePreview = function (previewId) {
//        const preview = document.getElementById(previewId);
//        if (preview) {
//            // If it's an existing photo, track its ID for deletion
//            const photoId = preview.dataset.photoId;
//            if (photoId) {
//                // Add a hidden input to track removed photos
//                const input = document.createElement('input');
//                input.type = 'hidden';
//                input.name = 'RemovedPhotoIds';
//                input.value = photoId;
//                form.appendChild(input);
//            } else {
//                // If it's a new photo, remove it from the photoMap
//                photoMap.delete(previewId);
//            }
//            preview.remove();
//        }
//    }
//    // Handle form submission
//    form.addEventListener('submit', function (e) {
//        e.preventDefault();

//        const formData = new FormData(form);

//        // Add new photos
//        for (let [previewId, file] of photoMap.entries()) {
//            formData.append('Photos', file);
//        }

//        // Submit the form
//        fetch(form.action, {
//            method: 'POST',
//            body: formData
//        })
//            .then(response => response.json())
//            .then(result => {
//                if (result.success) {
//                    window.location.href = result.redirectUrl;
//                } else {
//                    alert(result.message || 'Error saving changes');
//                }
//            })
//            .catch(error => {
//                console.error('Form submission error:', error);
//                alert('Failed to save changes. Please try again.');
//            });
//    });
//});