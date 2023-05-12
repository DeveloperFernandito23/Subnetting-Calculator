export function totalHost() {
    var hosts = document.getElementsByClassName("size");
    var values = [];

    for (var i = 0; i < hosts.length; i++) {
        values.push(hosts[i].value);
    }

    //hosts.forEach(host => {
    //    values.push(host.value);
    //});

    console.log(values);

    return values;
}