    $(window).load(function () {
        $('#carusel').show();
        $('#gallery').hide();
        $('#verborgen_file').hide();
        $('#hot-offer').hide();
        $('#hot-offer1').hide();
        var offId = $('#offer').val();
        $('#offerId').val(offId);
    });

$(document).ready(function () {
    $('input[type=radio][name=pic]').change(function () {
        if (this.value == 'carusel') {
            $('#carusel').show();
            $('#gallery').hide();
        }
        else if (this.value == 'gallery') {
            $('#carusel').hide();
            $('#gallery').show();
        }
    });

    $('input[type=checkbox][name=shedule]').change(function () {
        var ch = $('input[type=checkbox][name=shedule]');
        if (ch.is(":checked")) {
            $('#shedule').show();
            $('#shedule1').show();
        }
        else if (!ch.is(":checked")) {
            $('#shedule').hide();
            $('#shedule1').hide();
        }
    });

    $('input[type=checkbox][name=hot]').change(function () {
        var ch = $('input[type=checkbox][name=hot]');
        if (ch.is(":checked")) {
            $('#hot-offer').show();
            $('#hot-offer1').show();
        }
        else if (!ch.is(":checked")) {
            $('#hot-offer').hide();
            $('#hot-offer1').hide();
        }
    });      
});


$(document).ready(function () {
    $('input[type=radio][name=fon]').change(function () {
        if (this.value == 'pic') {
            $('#uploadButton').prop('disabled', false);
        }
        else if (this.value == 'empty') {
            $('#uploadButton').prop('disabled', true);
            $('#constr_body').css('background-image', '');
        }
    });
});

$(document).ready(function () {
    $('#uploadButton').on('click', function () {
        $('#verborgen_file').click();
    });
    $('#verborgen_file').change(function () {
        var file = this.files[0];
        var reader = new FileReader();
        reader.onloadend = function () {
            $('#constr_body').css('background-image', 'url("' + reader.result + '")');
            $('#backgr').val("url('" + reader.result + "')");
        }
        if (file) {
            reader.readAsDataURL(file);
        } else {
        }
    });
});
$(document).ready(function () {
    $('#rbar1').starwarsjs({
        stars: 5,
        default_stars: 4,
    });
    $('#rbar2').starwarsjs({
        stars: 5,
        default_stars: 4,
    });
});
