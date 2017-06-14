function onselectchange(linkId) {
    console.log(event.target.value);
    $(linkId).attr("href", (i, a) => {
        return a.replace(/(roleName=)[a-z]+/ig, '$1' + event.target.value);
    });
}