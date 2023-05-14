export function totalHost() {
    var hosts = document.getElementsByClassName("size");
    var values = [];

    for (var i = 0; i < hosts.length; i++) {
        values.push(parseInt(hosts[i].value));
    }

    return values;
}

export function takeIp() {
    var ip = document.getElementById("base-ip-value");


    return ip.value;
}

export function error() {
    alert("ERROR AL INTRODUCIR LOS DATOS");
}

export function drawResult() {
    var table = document.getElementById("result-container");

    var rowHead = document.createElement("tr");
    var column1Head = document.createElement("th");
    column1Head.innerHTML = "Subnet Name";
    var column2Head = document.createElement("th");
    column2Head.innerHTML = "IP Base";
    var column3Head = document.createElement("th");
    column3Head.innerHTML = "Mask";
    var column4Head = document.createElement("th");
    column4Head.innerHTML = "Range";
    var column5Head = document.createElement("th");
    column5Head.innerHTML = "Broadcast";
    var column6Head = document.createElement("th");
    column6Head.innerHTML = "Size Required";
    var column7Head = document.createElement("th");
    column6Head.innerHTML = "New Size";

    table.appendChild(rowHead);
    rowHead.appendChild(column1Head);
    rowHead.appendChild(column2Head);
    rowHead.appendChild(column3Head);
    rowHead.appendChild(column4Head);
    rowHead.appendChild(column5Head);
    rowHead.appendChild(column6Head);
    rowHead.appendChild(column7Head);

    console.log("funciona");

}