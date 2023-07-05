function showPreview(input) {
    var selectedImageFile; // Переменная для хранения выбранного файла
    if (input.files && input.files[0]) {
        selectedImageFile = input.files[0]; // Сохраняем выбранный файл в переменную
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('selectedImage').src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
        hideDropZone();
        showAnalysisButton();
    }
}

function cancelSelection() {
    // Сбросить значение элемента input
    document.getElementById('imageFile').value = '';

    // Очистить предварительный просмотр картинки
    document.getElementById('selectedImage').src = '';

    // Показать зону перетаскивания
    document.querySelector('.drop-zone').style.display = 'block';

    // Очистить переменную с выбранным файлом
    selectedImageFile = null;
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
