export function totalHost() {
    var hosts = document.getElementsByClassName("size");
    var values = [];

    hosts.forEach(host => {
        values.push(host.value);
    });

    console.log(values);

    return values;
}