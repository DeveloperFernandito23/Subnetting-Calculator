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

        private int _subnetNumber = 3;
        private long? _totalHost;
        private int _mask;
        private List<int?> _list = new();
        private bool _vlsm;

        private int SubnetNumber { get => _subnetNumber; set
            {
                if (!(value >= 1))
                {
                    _subnetNumber = 1;
                }
                else
                {
                    _subnetNumber = value;
                }
            } }
        private long? TotalHost { get => _totalHost; set => _totalHost = value; }
        private int Mask { get => _mask; set => _mask = value; }
        private List<int?> List { get => _list; set => _list = value; }
        private bool Vlsm { get => _vlsm; set => _vlsm = value; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _module = await JSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./scripts/app.js");
        }

        private async Task CheckHostJS() => List = await _module.InvokeAsync<List<int?>>("totalHost");
        private async Task ErrorJS() => await _module.InvokeVoidAsync("error");

		private async Task<bool> CheckHosts()
        {
            await CheckHostJS();

            bool isNull = List.All(x =>  x != null);

			TotalHost = List.Sum(x => x + 2);

            int avaliableHosts = TOTALBITS - Mask;

            double totalHostsAvaliable = Math.Pow(2, avaliableHosts);

            return isNull && totalHostsAvaliable >= TotalHost;
        }

		private async Task Calculate()
        {
            bool checkHosts = await CheckHosts();
            bool checkIPBase = true;
            bool checkSubnets = SubnetNumber > 0;


			if (checkIPBase && checkSubnets && checkHosts) 
            {
                if (!Vlsm)
                {

                }
                else
                {

                }
            }
            else
            {
                await ErrorJS();
            }
        }
    }
}