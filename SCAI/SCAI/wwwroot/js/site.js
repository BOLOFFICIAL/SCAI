function handleDragOver(event) {
    event.preventDefault();
    event.stopPropagation();
}

function handleDragLeave(event) {
    event.preventDefault();
    event.stopPropagation();
}

function handleDrop(event) {
    event.preventDefault();
    event.stopPropagation();
    var file = event.dataTransfer.files[0];
    var imageFileInput = document.getElementById('imageFile');
    imageFileInput.files = event.dataTransfer.files;

    showPreview(imageFileInput);
    submitForm();
}

function showPreview(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var selectedImage = document.getElementById('selectedImage');
            selectedImage.src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function triggerFileInput() {
    var imageFileInput = document.getElementById('imageFile');
    imageFileInput.click();
}

function submitForm() {
    var form = document.getElementById('myForm');
    form.submit();
}

function hideDropZone() {
    var dropZone = document.getElementById('dropZone');
    dropZone.style.display = 'none';
}