using Microsoft.JSInterop;
using Subnetting_Calculator.Models;
using System.Text.RegularExpressions;

namespace Subnetting_Calculator.Pages
{
	public partial class Index
	{
		public const int TOTALBITS = 32;

		private IJSInProcessObjectReference _module;

		private int _subnetNumber = 3;
		private long? _totalHost;
		private int _mask;
		private List<int?> _list = new();
		private bool _vlsm;
		private string _ipBaseWithMask;
		private string _ipBase;
		private List<List<string>> _paramsList;

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
		private async Task DrawResultJS() => await _module.InvokeVoidAsync("drawResult", _paramsList);
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

			int total = 0;

			if (Vlsm)
			{
				foreach (var host in List)
				{
					total += (int)Math.Pow(2, SearchRaisedTwo(host.Value));
				}

				TotalHost = total;
			}
			else
			{
				TotalHost = List.Max() * List.Count;

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

				List.ForEach(item => list.Add(item.Value));

				Subnetting subnetting = new Subnetting();

				if (!Vlsm)
				{
					subnetting.SubnetFlsm(list, IpBase, Mask, SubnetNumber);
					_paramsList = subnetting.paramsList;
					await DrawResultJS();
				}
				else
				{
					//NO ESTÁ HECHO ES COPIA Y PEGA
					subnetting.SubnetVlsm(list, IpBase, Mask, SubnetNumber);
					_paramsList = subnetting.paramsList;
					await DrawResultJS();
				}
			}
			else
			{
				await ErrorJS();
			}
		}
		private int SearchRaisedTwo(int host)
		{
			int raised = 0;

			while (!(Math.Pow(2, raised)-2 >= host))
			{
				raised++;
			}

			return raised;
		}
	}
}