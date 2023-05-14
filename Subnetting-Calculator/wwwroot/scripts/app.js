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

export function drawResult(paramList) {
    var table = document.getElementsByClassName("table")[0];
    var tbody = table.getElementsByClassName("tbody")[0];
    var thead = table.getElementsByClassName("thead-dark")[0];


    //lan, host, totalHost, ipBase, maskString, cidr, availableHost, broadCast

    var rowHead = document.createElement("tr");
    var column1Head = document.createElement("th");
    column1Head.setAttribute("scope", "col");
    column1Head.innerHTML = "Subnet Name";
    var column2Head = document.createElement("th");
    column2Head.setAttribute("scope", "col");
    column2Head.innerHTML = "Size Required";
    var column3Head = document.createElement("th");
    column3Head.setAttribute("scope", "col");
    column3Head.innerHTML = "New Size";
    var column4Head = document.createElement("th");
    column4Head.setAttribute("scope", "col");
    column4Head.innerHTML = "IP Base";
    var column5Head = document.createElement("th");
    column5Head.setAttribute("scope", "col");
    column5Head.innerHTML = "Mask";
    var column6Head = document.createElement("th");
    column6Head.setAttribute("scope", "col");
    column6Head.innerHTML = "CIDR";
    var column7Head = document.createElement("th");
    column7Head.setAttribute("scope", "col");
    column7Head.innerHTML = "IP Range";
    var column8Head = document.createElement("th");
    column8Head.setAttribute("scope", "col");
    column8Head.innerHTML = "BroadCast";

    table.appendChild(thead);
    thead.appendChild(rowHead);
    rowHead.appendChild(column1Head);
    rowHead.appendChild(column2Head);
    rowHead.appendChild(column3Head);
    rowHead.appendChild(column4Head);
    rowHead.appendChild(column5Head);
    rowHead.appendChild(column6Head);
    rowHead.appendChild(column7Head);
    rowHead.appendChild(column8Head);

    for (let i = 0; i < paramList.length; i++) {
        var newRow = document.createElement("tr");
        for (let j = 0; j < paramList[i].length; j++) {
            var newColumn = document.createElement("td");
            newColumn.innerHTML = paramList[i][j];
            j == 0 ? newColumn.setAttribute("scope", "row") : "";

            newRow.appendChild(newColumn);
        }
        tbody.appendChild(newRow);
    }
    table.appendChild(tbody);

    console.log("prueba");
    console.log("llega?");

}