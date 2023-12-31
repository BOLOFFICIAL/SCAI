﻿var spinner = document.getElementById('spinner');

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

function previewImage(input) {
    var imgPreview = document.getElementById('imagePreview');
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            imgPreview.src = e.target.result;
            imgPreview.style.display = 'block';
        };
        reader.readAsDataURL(input.files[0]);
    } else {
        imgPreview.src = '#';
        imgPreview.style.display = 'none';
    }
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
    // Show the spinner before submitting the form
    spinner.style.display = 'block';

    var form = document.getElementById('myForm');
    form.submit();
}

function hideDropZone() {
    var dropZone = document.getElementById('dropZone');
    dropZone.style.display = 'none';
}

function goBack() {
    window.history.back();
}