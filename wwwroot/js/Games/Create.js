let selectedFiles = [];

function updateFileInput(input) {
    var fileName = input.value.split('\\').pop();
    document.getElementById('main-file-label').textContent = fileName;

    var file = input.files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var newImageContainer = document.createElement("div");
            var oldPlaceholder = document.querySelector('.main-div');
            newImageContainer.className = "photo-frame photo-big main-div";

            var newImage = document.createElement("img");
            newImage.src = e.target.result;
            newImage.alt = "Uploaded Image";

            newImageContainer.appendChild(newImage);
            newImageContainer.addEventListener('click', function () {
                // document.getElementById('main-photo-input').value = "";
                // document.getElementById('main-file-label').textContent = "Wybierz zdjęcie do przesłania.";
                // var placeholderNode = createPlaceholderNode(false)
                // newImageContainer.parentNode.replaceChild(placeholderNode, newImageContainer);
                document.getElementById('main-photo-input').click();
            });

            oldPlaceholder.parentNode.replaceChild(newImageContainer, oldPlaceholder);
        };

        reader.readAsDataURL(file);
    }
}

function createPlaceholderNode(isSmall) {
    var newImageContainer = document.createElement("div");
    newImageContainer.className = "photo-placeholder " + (isSmall ? "photo-small thumbnail-div" : "photo-big main-div");

    var newImage = document.createElement("img");
    newImage.src = "/images/add_photo.png";
    newImage.alt = "Uploaded Image";

    newImageContainer.appendChild(newImage);
    newImageContainer.addEventListener('click', function () {
        document.getElementById(isSmall ? 'thumbnail-photo-input' : 'main-photo-input').click();
    });
    return newImageContainer;
}

function updateThumbnails(input) {
    document.getElementById('thumbnails-file-label').textContent = "";
    
    var allThumbnails= document.querySelectorAll('.thumbnail-div');
    var parent = allThumbnails[0].parentNode;
    

    allThumbnails.forEach(function (thumbnail) {
        thumbnail.remove();
    });

    var placeholderNode = createPlaceholderNode(true);
    parent.appendChild(placeholderNode);

    var files = Array.from(input.files);

    for (const file of files) {
        if (!(file instanceof File)) continue;
        var reader = new FileReader();

        reader.onload = function (e) {
            var newImageContainer = document.createElement("div");

            var allPlaceholders = document.querySelectorAll('.thumbnail-div');
            var oldPlaceholder = allPlaceholders[allPlaceholders.length - 1];

            newImageContainer.className = "photo-frame photo-small thumbnail-div";

            var newImage = document.createElement("img");
            newImage.src = e.target.result;
            newImage.alt = "Uploaded Image";

            newImageContainer.appendChild(newImage);
            newImageContainer.addEventListener('click', function () {

                // var occurrenceIndex = findOccurrenceIndex(newImageContainer, '.thumbnail-div');
            
                // removeFromFileInputAt(occurrenceIndex);

                // var placeholderNode = createPlaceholderNode(true)
                
                // var placeholder = newImageContainer.parentNode.querySelector('.photo-placeholder');

                // if (!(placeholder)) newImageContainer.parentNode.appendChild(placeholderNode);

                // let thumbnail = newImageContainer.parentNode.querySelector('.photo-frame');

                // newImageContainer.parentNode.removeChild(newImageContainer);

                document.getElementById('thumbnail-photo-input').click();  
            });

            if (allPlaceholders.length >= 4) {
                oldPlaceholder.parentNode.replaceChild(newImageContainer, oldPlaceholder);
            } else {
                oldPlaceholder.parentNode.insertBefore(newImageContainer, oldPlaceholder);
            }
        };

        reader.readAsDataURL(file);
    }
}

function findOccurrenceIndex(node, elementClass) {
    var elements = document.getElementsByClassName(elementClass);
    var occurrenceIndex = 0;
    for (var i = 0; i < elements.length; i++) {
        if (elements[i] === node) {
            occurrenceIndex = i + 1;
            break;
        }
    }
    return occurrenceIndex;
}

function removeFromFileInputAt(i) {
    var input = document.getElementById('thumbnail-photo-input');
    var files = Object.entries(input.files);
    if (input.files.length == 1) {
        input.value = "";
    } else {
        Object.entries(files.splice(i - 1, 1));
    }
    console.log(files);
}

document.querySelector('.main-div').addEventListener('click', function () {
    document.getElementById('main-photo-input').click();
});

document.querySelector('.thumbnail-div').addEventListener('click', function () {
    document.getElementById('thumbnail-photo-input').click();
});