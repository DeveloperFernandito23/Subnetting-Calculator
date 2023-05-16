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

export function drawResult(subnetClassList) {

    var subnetList = JSON.parse(subnetClassList);

    var table = document.getElementsByClassName("table")[0];
    var thead = table.getElementsByClassName("thead-dark")[0];
    var tbody = table.getElementsByClassName("tbody")[0];
    thead.innerHTML = "";
    tbody.innerHTML = "";


    //lan, host, totalHost, ipBase, maskString, cidr, availableHost, broadCast

    var rowHead = document.createElement("tr");

    var subnetNameTh = document.createElement("th");
    subnetNameTh.setAttribute("scope", "col");
    subnetNameTh.innerHTML = "Subnet Name";

    var subnetRequiredSizeTh = document.createElement("th");
    subnetRequiredSizeTh.setAttribute("scope", "col");
    subnetRequiredSizeTh.innerHTML = "Size Required";

    var subnetTotalSizeTh = document.createElement("th");
    subnetTotalSizeTh.setAttribute("scope", "col");
    subnetTotalSizeTh.innerHTML = "Total Size";

    var subnetIPBaseTh = document.createElement("th");
    subnetIPBaseTh.setAttribute("scope", "col");
    subnetIPBaseTh.innerHTML = "IP Base";

    var subnetRangeTh = document.createElement("th");
    subnetRangeTh.setAttribute("scope", "col");
    subnetRangeTh.innerHTML = "IP Range";

    var subnetBroadcastTh = document.createElement("th");
    subnetBroadcastTh.setAttribute("scope", "col");
    subnetBroadcastTh.innerHTML = "Broadast";

    var subnetMaskTh = document.createElement("th");
    subnetMaskTh.setAttribute("scope", "col");
    subnetMaskTh.innerHTML = "Mask";

    var subnetCIDRTh = document.createElement("th");
    subnetCIDRTh.setAttribute("scope", "col");
    subnetCIDRTh.innerHTML = "CIDR";


    table.appendChild(thead);
    thead.appendChild(rowHead);

    rowHead.appendChild(subnetNameTh);
    rowHead.appendChild(subnetRequiredSizeTh);
    rowHead.appendChild(subnetTotalSizeTh);
    rowHead.appendChild(subnetIPBaseTh);
    rowHead.appendChild(subnetRangeTh);
    rowHead.appendChild(subnetBroadcastTh);
    rowHead.appendChild(subnetMaskTh);
    rowHead.appendChild(subnetCIDRTh);

    subnetList.forEach(subnet => {
        var newRow = document.createElement("tr");

        var subnetName = document.createElement("td");
        subnetName.innerHTML = "";
        subnetName.innerHTML = subnet.Name;

        var subnetRequiredSize = document.createElement("td");
        subnetRequiredSize.innerHTML = "";
        subnetRequiredSize.innerHTML = subnet.Size;

        var subnetTotalSize = document.createElement("td");
        subnetTotalSize.innerHTML = "";
        subnetTotalSize.innerHTML = subnet.TotalSize;

        var subnetIPBase = document.createElement("td");
        subnetIPBase.innerHTML = "";
        subnetIPBase.innerHTML = subnet.IPBase.join('.');

        var subnetRange = document.createElement("td");
        subnetName.innerHTML = "";
        subnetRange.innerHTML = `${subnet.RangeStart.join('.')} - ${subnet.RangeEnd.join('.') }`;

        var subnetBroadcast = document.createElement("td");
        subnetBroadcast.innerHTML = "";
        subnetBroadcast.innerHTML = subnet.Broadcast.join('.');

        var subnetMask = document.createElement("td");
        subnetMask.innerHTML = "";
        subnetMask.innerHTML = subnet.Mask.join('.');

        var subnetCIDR = document.createElement("td");
        subnetCIDR.innerHTML = "";
        subnetCIDR.innerHTML = subnet.CIDR;

        tbody.appendChild(newRow);
        newRow.appendChild(subnetName);
        newRow.appendChild(subnetRequiredSize);
        newRow.appendChild(subnetTotalSize);
        newRow.appendChild(subnetIPBase);
        newRow.appendChild(subnetRange);
        newRow.appendChild(subnetBroadcast);
        newRow.appendChild(subnetMask);
        newRow.appendChild(subnetCIDR);
    });

    table.appendChild(tbody);

    console.log("prueba");
    console.log("llega?");

}