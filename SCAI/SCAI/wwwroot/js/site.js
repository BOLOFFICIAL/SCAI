function showPreview(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('selectedImage').src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
        hideDropZone();
        showAnalysisButton();
    }
}

function hideDropZone() {
    var dropZone = document.querySelector('.drop-zone');
    dropZone.style.display = 'none';
}

function showAnalysisButton() {
    var analysisButton = document.getElementById('analysisButton');
    analysisButton.style.display = 'inline-block';
}

function handleDrop(event) {
    event.preventDefault();
    var file = event.dataTransfer.files[0];
    document.getElementById('selectedImage').src = URL.createObjectURL(file);
    document.getElementById('imageFile').files = event.dataTransfer.files;
    hideDropZone();
    showAnalysisButton();
}

function handleDragOver(event) {
    event.preventDefault();
    event.stopPropagation();
    event.currentTarget.classList.add('dragover');
}

function handleDragLeave(event) {
    event.currentTarget.classList.remove('dragover');
}

function triggerFileInput() {
    document.getElementById('imageFile').click();
}

function handleImageDragLeave(event) {
    event.currentTarget.classList.remove('dragover');
}
