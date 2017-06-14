let counter = 1;
$(function () {
    // We can attach the `fileselect` event to all file inputs on the page
    $(document).on('change', ':file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });

    // We can watch for our custom `fileselect` event like this
    $(document).ready(function () {
        $(':file').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }
        });
    });

});

function handleFileSelectMulti(evt) {
    var files = evt.target.files; // FileList object
    document.getElementById('images_row').innerHTML = "";
    for (var i = 0, f; f = files[i]; i++) {
        // Only process image files.
        if (!f.type.match('image.*')) {
            alert("Только изображения....");
        }
        var reader = new FileReader();
        // Closure to capture the file information.
        reader.onload = (function (theFile) {
            return function (e) {
                var imagesRow = document.getElementById('images_row');
                imagesRow.innerHTML += "<img id='img" + counter + "' class='img-thumbnail create-tumbnail' src='" + e.target.result + "'/>";
                counter++;
            };
        })(f);
        // Read in the image file as a data URL.
        reader.readAsDataURL(f);
    }
}
document.getElementById('file').addEventListener('change', handleFileSelectMulti, false);

$('#form').submit(function (event) {
    var imgVal = $('#file').val();
    var catImgVal = $('#CatalogImg').val();
    if ((imgVal != '') ) {
        if (catImgVal != '')
        {
            return;
        }
        else 
            $('#catImg').css('display', 'inline-block');
    }
    else{
        $('#imgs').css('display', 'inline-block');
    }   
    event.preventDefault();
});


$('#images_row').click((event) => {
    var images = [];
    if (event.target.tagName == 'IMG') {
        images = document.querySelectorAll('#images_row img');
        images.forEach((it, i, images) => { it.className = 'img-thumbnail create-tumbnail' })
        var item = document.getElementById(event.target.id);
        var input = document.getElementById('CatalogImg');
        item.className = 'img-thumbnail selected-tumbnail';
        input.value = item.src.split('base64,')[1];
    }
})

$('#CatalogImg').change(() => {
    $('#catImg').css('display', 'none');
})

$('#file').change(() => {
    $('#imgs').css('display', 'none');
})

jQuery(function ($) {
    $("#Contacts").mask("+375(99) 999-99-99");
    $("#WorkingHours").mask("С 99.99 до 99.99");
    
});