document.addEventListener('DOMContentLoaded', function () {
    const dropZone = document.getElementById('dropZone');
    const fileInput = document.getElementById('fileInput');
    const browseButton = document.getElementById('browseButton');
    const previewContainer = document.getElementById('previewContainer');
    const uploadProgress = document.getElementById('uploadProgress');
    const progressBar = uploadProgress.querySelector('.progress-bar');
    const recipeForm = document.getElementById('recipeForm');

    // Track unique files using a Map instead of FormData
    let photoMap = new Map(); // Use Map to track files by preview ID

    // Prevent default drag behaviors
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        dropZone.addEventListener(eventName, preventDefaults, false);
        document.body.addEventListener(eventName, preventDefaults, false);
    });

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    ['dragenter', 'dragover'].forEach(eventName => {
        dropZone.addEventListener(eventName, highlight, false);
    });

    ['dragleave', 'drop'].forEach(eventName => {
        dropZone.addEventListener(eventName, unhighlight, false);
    });

    function highlight(e) { dropZone.classList.add('dragover'); }
    function unhighlight(e) { dropZone.classList.remove('dragover'); }

    // Handle dropped files
    dropZone.addEventListener('drop', handleDrop, false);
    fileInput.addEventListener('change', handleFiles);
    browseButton.addEventListener('click', () => fileInput.click());

    function handleDrop(e) {
        const dt = e.dataTransfer;
        const files = dt.files;
        handleFiles({ target: { files } });
    }

    function handleFiles(e) {
        const files = [...e.target.files];
        files.forEach(previewFile);
    }

    function previewFile(file) {
        if (!file.type.startsWith('image/')) {
            alert('Please upload only image files.');
            return;
        }

        if (file.size > 5242880) {
            alert('File size should not exceed 5MB.');
            return;
        }

        const reader = new FileReader();
        reader.onloadend = function () {
            const previewId = 'preview-' + Date.now();
            const previewHtml = createPreviewHtml(previewId, reader.result);
            previewContainer.insertAdjacentHTML('beforeend', previewHtml);

            // Store file in Map with preview ID as key
            photoMap.set(previewId, file);
        }
        reader.readAsDataURL(file);
    }

    function createPreviewHtml(previewId, imgSrc) {
        const isFirst = previewContainer.children.length === 0;
        return `
            <div class="col-md-3 preview-item" id="${previewId}">
                <div class="position-relative">
                    ${isFirst ? '<span class="main-photo-badge">Main Photo</span>' : ''}
                    <img src="${imgSrc}" class="shadow-sm" alt="Preview">
                    <button type="button" class="btn btn-sm remove-photo" onclick="removePreview('${previewId}')">
                        <i class="bi bi-x"></i>
                    </button>
                </div>
            </div>
        `;
    }

    // Update removePreview to remove from Map
    window.removePreview = function (previewId) {
        const preview = document.getElementById(previewId);
        if (preview) {
            preview.remove();
            photoMap.delete(previewId); // Remove from Map
        }
    }

    // Update form submission to only use files from Map
    recipeForm.addEventListener('submit', async function (e) {
        e.preventDefault();

        const formData = new FormData(recipeForm);

        // Only add files from our tracked Map
        for (let file of photoMap.values()) {
            formData.append('photos', file);
        }

        try {
            const response = await fetch(recipeForm.action, {
                method: 'POST',
                body: formData
            });

            const result = await response.json();
            if (result.success) {
                window.location.href = result.redirectUrl;
            } else {
                alert(result.message || 'Error creating recipe');
            }
        } catch (error) {
            console.error('Form submission error:', error);
            alert('Failed to create recipe. Please try again.');
        }
    });
});