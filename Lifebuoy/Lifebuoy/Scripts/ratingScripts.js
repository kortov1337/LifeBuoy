function rbarInit(rbarid, initval, offerid, resultid) {
    $('#' + rbarid).starwarsjs({
        stars: 5,
        default_stars: initval,
        on_select: function (data) {
            sendRating(offerid, data, resultid);
        }
    });
}
function sendRating(offerid, rating, resultid) {
    $.ajax({
        url: "/Catalog/SendRating/" + offerid + "?rating=" + rating,
        type: "POST",
        success: function (response) {
            if (response != null && response.success) {
                var label = $('#' + resultid);
                label.html(response.responseText);
            } else {
                var label = $('#' + resultid);
                label.html = response.responseText;
            }
        },
        error: function (response) {
            alert("error!");  // 
        }
    })
}