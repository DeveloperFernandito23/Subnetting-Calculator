using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Subnetting_Calculator;
using Subnetting_Calculator.Shared;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

namespace Subnetting_Calculator.Pages
{
    public partial class Index
    {
        private const int TOTALBITS = 32;

        private IJSInProcessObjectReference _module;

        static private int _subnetNumber = 3;
        static private long? _totalHost;
        static private int _mask = 24;
        static private List<int?> _list = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _module = await JSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./scripts/app.js");
        }

            
        private async Task CheckHostJS() => _list = await _module.InvokeAsync<List<int?>>("totalHost");
        private async Task ErrorJS() => await _module.InvokeVoidAsync("error");


		private async Task<bool> CheckHosts()
        {
            await CheckHostJS();

            bool isNull = _list.All(x =>  x != null); 

            _totalHost = _list.Sum();

            int avaliableHosts = TOTALBITS - _mask;

            double totalHostsAvaliable = Math.Pow(2, avaliableHosts);

            return isNull && totalHostsAvaliable > _totalHost;
        }

        private async Task Calculate()
        {
            
            bool checkHosts = await CheckHosts();
            bool checkIPBase = true;
            bool checkSubnets = true;


			if (checkIPBase && checkSubnets && checkHosts) 
            {
                Console.WriteLine("SI");
            }
            else
            {
                await ErrorJS();
            }
        }
    }
}