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

namespace Subnetting_Calculator.Pages
{
    public partial class Index
    {
        private const int TOTALBITS = 32;

        static private int subnetNumber = 3;
		static private int? size;
        static private long? totalHost;
		static private int mask = 23;


		private bool CheckHosts()
        {
			totalHost = size != null ? (size * subnetNumber) + (2 * subnetNumber) : null;

			Console.WriteLine(totalHost);

            int avaliableHosts = TOTALBITS - mask;

            double totalHostsAvaliable = Math.Pow(2, avaliableHosts);

            return totalHostsAvaliable > totalHost;
        }

        private void Calculate()
        {

        }
    }
}