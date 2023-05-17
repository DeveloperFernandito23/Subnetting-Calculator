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
var alertPlaceholder = document.getElementById('liveAlert');

export function error() {
    if (alertPlaceholder.innerHTML.length == 0) {
        alert('<svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Danger:"><use xlink:href="#exclamation-triangle-fill" /></svg> ERROR AL INTRODUCIR LOS DATOS.', 'danger');
        var myAlert = document.getElementById('alert-content');

        myAlert.addEventListener('closed.bs.alert', () => {
            var removeElement = document.getElementById("alert");
            alertPlaceholder.removeChild(removeElement);
        });
    }
}

function alert(message, type) {
    var wrapper = document.createElement('div');
    wrapper.setAttribute("id", "alert");
    wrapper.innerHTML = '<div class="fade show alert alert-' + type + ' alert-dismissible" role="alert" id="alert-content">' + message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>';

    alertPlaceholder.append(wrapper);
}

export function drawResult(subnetClassList) {

    var subnetList = JSON.parse(subnetClassList);

    var table = document.getElementsByClassName("table")[0];
    var thead = table.getElementsByClassName("thead-dark")[0];
    var tbody = table.getElementsByClassName("tbody")[0];
    thead.innerHTML = "";
    tbody.innerHTML = "";


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
        subnetRange.innerHTML = "";
        subnetRange.innerHTML = `${subnet.RangeStart.join('.')} - ${subnet.RangeEnd.join('.')}`;

        var subnetBroadcast = document.createElement("td");
        subnetBroadcast.innerHTML = "";
        subnetBroadcast.innerHTML = subnet.Broadcast.join('.');

        var subnetMask = document.createElement("td");
        subnetMask.innerHTML = "";
        subnetMask.innerHTML = subnet.Mask.join('.');

        var subnetCIDR = document.createElement("td");
        subnetCIDR.innerHTML = "";
        subnetCIDR.innerHTML = `/${subnet.CIDR}`;

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
}

export function showMessage() {
    var icon = '<svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Info:"><use xlink:href="#info-fill" /></svg>'
    var headerAlert = '<h4 class="alert-heading">INFORMATION MESSAGE!</h4>';
    var textAlert = '<p>When you press calculate you get an error message?, Check that: First you have filled in all the fields. Then check the syntax that is illustrated below. And finally check if it is possible to do x subnets with those required hosts</p>';
    var br = '<hr/>'
    var sintax = '<p> The IP Base: 192.168.2.0/24 <br/> The host number: 10 <br/> The subnets number: 4 <br/> <strong>This is a posible example</strong></p>'

    if (alertPlaceholder.innerHTML.length == 0) {
        alert(icon + headerAlert + textAlert + br + sintax, 'primary');

        var myAlert = document.getElementById('alert-content');

        myAlert.addEventListener('closed.bs.alert', () => {
            var removeElement = document.getElementById("alert");
            alertPlaceholder.removeChild(removeElement);
        });
    }
}