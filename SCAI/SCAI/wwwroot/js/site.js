// Метод предпросмотра картинки после загрузки
function showPreview(input) 
{
    if (input.files && input.files[0])
    {
        var reader = new FileReader();
        reader.onload = function (e)
        {
            document.getElementById('selectedImage').src = e.target.result;
        }
        reader.readAsDataURL(input.files[0]);
        hideDropZone();
    }
}

// Скрытие зоны перетаскивания после загрузки
function hideDropZone() {
    var dropZone = document.querySelector('.drop-zone');
    dropZone.style.display = 'none';
}

// Обрабатывает событие перетаскивания файла на зону сброса.
function handleDrop(event)
{
    event.preventDefault();
    var file = event.dataTransfer.files[0];
    document.getElementById('selectedImage').src = URL.createObjectURL(file);
    document.getElementById('imageFile').files = event.dataTransfer.files;
    hideDropZone();
}

// Обрабатывает событие наведения курсора на зону сброса.
function handleDragOver(event)
{
    event.preventDefault();
    event.stopPropagation();
    event.currentTarget.classList.add('dragover');
    //event.currentTarget.style.opacity = '0.7';
}

// Обрабатывает событие покидания курсором зоны сброса.
function handleDragLeave(event)
{
    event.currentTarget.classList.remove('dragover');
    //event.currentTarget.style.opacity = '1';
}

// Вызывает элемент загрузки файла по клику на изображении.
function triggerFileInput()
{
    document.getElementById('imageFile').click();
}

/*function uploadImage()
{
    var selectedImage = document.getElementById('selectedImage');
    if (selectedImage.src && selectedImage.src !== 'about:blank') {
        var file = dataURLtoFile(selectedImage.src, 'uploaded_image.png');
        document.getElementById('imageFile').files = [file];
        document.forms[0].submit();
    }
}*/

// Обрабатывает событие покидания курсором изображения.
function handleImageDragLeave(event)
{
    event.currentTarget.classList.remove('dragover');
    //event.currentTarget.style.opacity = '1';
}