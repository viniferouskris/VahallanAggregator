
////possibly not needed
//// photoManagement.js
//document.addEventListener('DOMContentLoaded', function () {
//    const dragArea = document.getElementById('dragArea');
//    const fileInput = document.getElementById('fileInput');
//    const browseBtn = document.getElementById('browseBtn');
//    const photosGrid = document.getElementById('photosGrid');
//    const progressBar = document.querySelector('.progress-bar');
//    const progressContainer = document.querySelector('.upload-progress');
//    const recipeId = document.getElementById('RecipeId').value;

//    // Drag and drop handlers
//    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
//        dragArea.addEventListener(eventName, preventDefaults, false);
//    });

//    function preventDefaults(e) {
//        e.preventDefault();
//        e.stopPropagation();
//    }

//    ['dragenter', 'dragover'].forEach(eventName => {
//        dragArea.addEventListener(eventName, highlight, false);
//    });

//    ['dragleave', 'drop'].forEach(eventName => {
//        dragArea.addEventListener(eventName, unhighlight, false);
//    });

//    function highlight() {
//        dragArea.classList.add('highlight');
//    }

//    function unhighlight() {
//        dragArea.classList.remove('highlight');
//    }

//    // Handle file drop
//    dragArea.addEventListener('drop', handleDrop, false);

//    function handleDrop(e) {
//        const dt = e.dataTransfer;
//        const files = dt.files;
//        handleFiles(files);
//    }

//    // Handle file selection via button
//    browseBtn.addEventListener('click', () => fileInput.click());
//    fileInput.addEventListener('change', () => handleFiles(fileInput.files));

//    async function handleFiles(files) {
//        const validFiles = Array.from(files).filter(file =>
//            file.type.startsWith('image/') && file.size <= 5242880); // 5MB limit

//        if (validFiles.length === 0) {
//            alert('Please select valid image files (max 5MB each)');
//            return;
//        }

//        progressContainer.classList.remove('d-none');

//        for (let file of validFiles) {
//            await uploadFile(file);
//        }

//        progressContainer.classList.add('d-none');
//        progressBar.style.width = '0%';
//    }

//    async function uploadFile(file) {
//        const formData = new FormData();
//        formData.append('photo', file);
//        formData.append('recipeId', recipeId);

//        try {
//            const response = await fetch('/Photo/Upload', {
//                method: 'POST',
//                body: formData,
//                headers: {
//                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//                }
//            });

//            if (!response.ok) throw new Error('Upload failed');

//            const result = await response.json();
//            if (result.success) {
//                addPhotoToGrid(result);
//            }
//        } catch (error) {
//            console.error('Upload error:', error);
//            alert('Failed to upload photo');
//        }
//    }

//    function addPhotoToGrid(photoData) {
//        const photoHtml = `
//            <div class="col-md-4 col-lg-3" data-photo-id="${photoData.photoId}">
//                <div class="card h-100 ${photoData.isMain ? 'border-primary' : ''}">
//                    <img src="${photoData.thumbnailUrl}" class="card-img-top" alt="Recipe photo">
//                    <div class="card-body p-2">
//                        <div class="btn-group w-100">
//                            ${!photoData.isMain ? `
//                                <button class="btn btn-sm btn-outline-primary make-main-btn">
//                                    Make Main
//                                </button>
//                            ` : ''}
//                            <button class="btn btn-sm btn-outline-danger delete-photo-btn">
//                                Delete
//                            </button>
//                        </div>
//                    </div>
//                </div>
//            </div>
//        `;
//        photosGrid.insertAdjacentHTML('beforeend', photoHtml);
//    }

//    // Event delegation for dynamic buttons
//    photosGrid.addEventListener('click', async function (e) {
//        const photoId = e.target.closest('[data-photo-id]')?.dataset.photoId;
//        if (!photoId) return;

//        // Handle Make Main button
//        if (e.target.classList.contains('make-main-btn')) {
//            try {
//                const response = await fetch('/Photo/SetMainPhoto', {
//                    method: 'POST',
//                    headers: {
//                        'Content-Type': 'application/json',
//                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//                    },
//                    body: JSON.stringify({ recipeId, photoId })
//                });

//                if (!response.ok) throw new Error('Failed to set main photo');

//                // Update UI
//                document.querySelectorAll('.card').forEach(card =>
//                    card.classList.remove('border-primary'));
//                e.target.closest('.card').classList.add('border-primary');
//                location.reload(); // Refresh to update all views
//            } catch (error) {
//                console.error('Error setting main photo:', error);
//                alert('Failed to set main photo');
//            }
//        }

//        // Handle Delete button
//        if (e.target.classList.contains('delete-photo-btn')) {
//            if (!confirm('Are you sure you want to delete this photo?')) return;

//            try {
//                const response = await fetch('/Photo/Delete', {
//                    method: 'POST',
//                    headers: {
//                        'Content-Type': 'application/json',
//                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//                    },
//                    body: JSON.stringify({ recipeId, photoId })
//                });

//                if (!response.ok) throw new Error('Failed to delete photo');

//                // Remove photo from grid
//                e.target.closest('[data-photo-id]').remove();
//            } catch (error) {
//                console.error('Error deleting photo:', error);
//                alert('Failed to delete photo');
//            }
//        }
//    });
//});
