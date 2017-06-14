
function addFilter(filterid) {
    var filtersRow = document.getElementById("filters");
    var select = document.getElementById(filterid);

    if (select.options.selectedIndex == 0)
        filtersRow.removeChild(document.getElementById(filterid + "_"));
    else {
        if (document.getElementById(filterid + "_") != null)
            filtersRow.removeChild(document.getElementById(filterid + "_"));

        var value = select.options[select.selectedIndex].value;
        var clear = document.createElement('img');
        clear.className = "sidebar-clear-icon";
        clear.src = "/Content/images/clear-icon.png";
        clear.setAttribute("onclick", "onClearIco('" + filterid + "_')");
        var filter = document.createElement('div');
        filter.className = "sidebar-row";
        filter.id = filterid + "_";
        filter.innerText = value;
        filter.appendChild(clear);
        filtersRow.appendChild(filter);
    }

}

function onClearIco(filterid) {

    var filtersRow = document.getElementById("filters");
    var filter = document.getElementById(filterid);
    var select = document.getElementById(filterid.replace(/_/, ""));
    select.options[0].selected = true;
    filtersRow.removeChild(filter);
}