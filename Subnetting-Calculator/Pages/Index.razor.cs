using Microsoft.JSInterop;
using Subnetting_Calculator.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Subnetting_Calculator.Pages
{
	public partial class Index
	{
		public static readonly int TOTALBITS = 32;

		private IJSInProcessObjectReference _module;

		private int _subnetNumber = 3;
		private long? _totalHost;
		private int _mask;
		private List<int?> _hostList = new();
		private bool _vlsm;
		private string _ipBaseWithMask;
		private List<int> _ipBase = new();
		private List<Subnet> _subnetList = new();

		private int SubnetNumber { get => _subnetNumber; set => _subnetNumber = !(value >= 1) ? 1 : value; }
		private long? TotalHost { get => _totalHost; set => _totalHost = value; }
		private int Mask { get => _mask; set => _mask = value; }
		private List<int?> HostList { get => _hostList; set => _hostList = value; }
		private bool Vlsm { get => _vlsm; set => _vlsm = value; }
		private string IpBaseWithMask { get => _ipBaseWithMask; set => _ipBaseWithMask = value; }
		private List<int> IpBase { get => _ipBase; set => _ipBase = value; }
		public List<Subnet> SubnetList { get => _subnetList; set => _subnetList = value; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			_module = await JSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./scripts/app.js");
		}

		private async Task CheckHostJS() => HostList = await _module.InvokeAsync<List<int?>>("totalHost");
		private async Task CheckIPBaseJS() => IpBaseWithMask = await _module.InvokeAsync<string>("takeIp");
		private async Task DrawResultJS() => await _module.InvokeVoidAsync("drawResult", JsonSerializer.Serialize(SubnetList));
		private async Task ErrorJS() => await _module.InvokeVoidAsync("error");

		private async Task<bool> CheckIp()
		{
			await CheckIPBaseJS();

			bool result = false;

			string pattern = @"\b(1[0-9][0-9]|2[0-5][0-5]|[0-9]|[0-9][0-9]).(1[0-9][0-9]|2[0-5][0-5]|[0-9]|[0-9][0-9]).(1[0-9][0-9]|2[0-5][0-5]|[0-9]|[0-9][0-9]).(25[0-5]|[01]|[0-9][0-9])[/](1[0-9]|2[0-9]|3[0-2])\b";

			Regex checkIp = new Regex(pattern);

			if (checkIp.IsMatch(IpBaseWithMask))
			{
				result = true;
				Mask = int.Parse(IpBaseWithMask.Split('/')[1]);

				IpBase = new();

				foreach (var item in IpBaseWithMask.Split('/')[0].Split('.'))
				{
					int e = int.Parse(item.ToString());
					IpBase.Add(e);
				}
			}

			return result;
		}

		private async Task<bool> CheckHosts()
		{
			await CheckHostJS();

			bool isNull = HostList.All(x => x.HasValue);

			int total = 0;

			if (!Vlsm)
			{
				TotalHost = HostList.Max() * HostList.Count;
			}
			else
			{
				foreach (var host in HostList)
				{
					total += (int)Math.Pow(2, FindHostBits(host.Value));
				}

				TotalHost = total;
			}

			int avaliableHosts = TOTALBITS - Mask;

			double totalHostsAvaliable = Math.Pow(2, avaliableHosts);

			return isNull && totalHostsAvaliable >= TotalHost;
		}

		private async Task Calculate()
		{
			bool checkIPBase = await CheckIp();
			bool checkHosts = await CheckHosts();
			bool checkSubnets = SubnetNumber > 0;


			if (checkIPBase && checkSubnets && checkHosts)
			{
				List<int> list = new List<int>();

				HostList.ForEach(item => list.Add(item.Value));

				Subnetting subnetting = new Subnetting();

				if (!Vlsm)
				{
					subnetting.SubnetFlsm(list, IpBase, Mask, SubnetNumber);

					SubnetList = subnetting.SubnetsList;

					await DrawResultJS();
				}
				else
				{
					subnetting.SubnetVlsm(list, IpBase, Mask, SubnetNumber);

					SubnetList = subnetting.SubnetsList;

					await DrawResultJS();
				}
			}
			else
			{
				await ErrorJS();
			}
		}
		public static int FindHostBits(int host)
		{
			int number = 0;

			while (!(Math.Pow(2, number) - 2 >= host)) number++;

			return number;
		}
	}
}