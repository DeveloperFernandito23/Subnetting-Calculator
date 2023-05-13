using Microsoft.JSInterop;
using Subnetting_Calculator.Classes;
using System.Text.RegularExpressions;

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
		private string _ipBaseWithMask;
		private string _ipBase;

		private int SubnetNumber
		{
			get => _subnetNumber; set
			{
				if (!(value >= 1))
				{
					_subnetNumber = 1;
				}
				else
				{
					_subnetNumber = value;
				}
			}
		}
		private long? TotalHost { get => _totalHost; set => _totalHost = value; }
		private int Mask { get => _mask; set => _mask = value; }
		private List<int?> List { get => _list; set => _list = value; }
		private bool Vlsm { get => _vlsm; set => _vlsm = value; }
		private string IpBaseWithMask { get => _ipBaseWithMask; set => _ipBaseWithMask = value; }
		private string IpBase { get => _ipBase; set => _ipBase = value; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			_module = await JSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./scripts/app.js");
		}

		private async Task CheckHostJS() => List = await _module.InvokeAsync<List<int?>>("totalHost");
		private async Task CheckIPBaseJS() => IpBaseWithMask = await _module.InvokeAsync<string>("takeIp");
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
				IpBase = IpBaseWithMask.Split('/')[0];
				Mask = int.Parse(IpBaseWithMask.Split('/')[1]);
			}

			return result;
		}

		private async Task<bool> CheckHosts()
		{
			await CheckHostJS();

			bool isNull = List.All(x => x != null);

			TotalHost = List.Sum(x => x + 2);

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
				if (!Vlsm)
				{
					Subnetting subnetting = new Subnetting();
					subnetting.SubnetFlsm(IpBase, Mask, SubnetNumber);
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