export function totalHost() {
    var hosts = document.getElementsByClassName("size");
    var values = [];

    for (var i = 0; i < hosts.length; i++) {
        values.push(parseInt(hosts[i].value));
    }

    return values;
}

export function error() {
    alert("ERROR AL INTRODUCIR LOS DATOS");
}