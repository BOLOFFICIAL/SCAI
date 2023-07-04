function showPreview(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById('selectedImage').src = e.target.result;
        };

        reader.readAsDataURL(input.files[0]);
    }
}
