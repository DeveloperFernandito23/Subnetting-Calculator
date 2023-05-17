using System.Text.RegularExpressions;
using Index = Subnetting_Calculator.Pages.Index;

namespace Subnetting_Calculator.Models
{
	public class Subnetting
	{
		private List<string> _newMaskInBinaryDivide = new();
		private List<string> _jumpInBinary = new();
		private List<Subnet> _subnetsList = new();

		private int HostBits { get; set; }
		private int NewMask { get; set; }
		public List<string> NewMaskInBinaryDivide { get => _newMaskInBinaryDivide; set => _newMaskInBinaryDivide = value; }
		private int Jump { get; set; }
		public List<string> JumpInBinary { get => _jumpInBinary; set => _jumpInBinary = value; }
		public List<Subnet> SubnetsList { get => _subnetsList; set => _subnetsList = value; }

		private List<int> GetIPBase(List<int> ipAddress, int mask)
		{
			List<string> ipInBinaryDivide = new List<string>();
			List<string> maskInBinaryDivide = new List<string>();

			foreach (var item in ipAddress)
			{
				ipInBinaryDivide.Add(BinaryString(item, 8));
			}

			string maskInBinary = "".PadLeft(mask, '1').PadRight(32, '0');

			foreach (var item in DivideInOct(maskInBinary))
			{
				maskInBinaryDivide.Add(item);
			}

			return BinaryOperator(ipInBinaryDivide, maskInBinaryDivide, '*');
		}

		private string BinaryString(int number, int fillNumber) => string.Concat(ConvertToBinary(number)).PadLeft(fillNumber, '0').PadRight(fillNumber, '0');

		private void StartSubnet(List<int> hosts)
		{
			NewMask = Index.TOTALBITS - HostBits; //Obtenemos la nueva máscara

			string newMaskInBinary = "".PadLeft(NewMask, '1').PadRight(32, '0'); //Pasamos la máscara a binario

			NewMaskInBinaryDivide = DivideInOct(newMaskInBinary); //Dividimos la máscara en octetos

			Jump = (int)Math.Pow(2, HostBits); //Calculamos el salto elevando dos a el número de bits de host sacado arriba

			JumpInBinary = DivideInOct(BinaryString(Jump, 32)); //Convertimos el salto en binario
		}

		private List<int> FillSubnet(int index, List<int> ipBaseCalculated, List<int> hosts)
		{
			Subnet subnet = new Subnet();

			List<string> ipBaseInBinary = new List<string>();

			ipBaseCalculated.ForEach(item => ipBaseInBinary.Add(BinaryString(item, 8))); //Transformamos la IP Base en binario

			subnet.Name = $"LAN {index + 1}";

			subnet.Size = hosts[index];

			subnet.TotalSize = Jump - 2;

			subnet.IPBase = ipBaseCalculated;

			subnet.Mask = NewMaskInBinaryDivide.Select(item => Convert.ToInt32(item, 2));

			subnet.CIDR = NewMask;

			string broadcastInBinary = string.Concat(ipBaseInBinary).Substring(0, NewMask) + new string('1', HostBits); //Conseguimos el BroadCast en Binario

			List<string> broadcastInBinaryDivide = DivideInOct(broadcastInBinary); //Transformamos la IP del BroadCast en decimal

			List<int> broadcast = new();

			broadcastInBinaryDivide.ForEach(item => broadcast.Add(Convert.ToInt32(item, 2)));

			subnet.RangeStart = Range(ipBaseCalculated, '+');

			subnet.RangeEnd = Range(broadcast, '-');

			subnet.Broadcast = broadcast;

			_subnetsList.Add(subnet);

			return BinaryOperator(ipBaseInBinary, JumpInBinary, '+');
		}

		public void SubnetFlsm(List<int> hosts, List<int> ipAddress, int mask, int subnetsRequired)
		{
			List<int> ipBaseCalculated = GetIPBase(ipAddress, mask); //Calcula IP Base

			HostBits = Index.FindHostBits(hosts.Max(x => x)); //Bits de hosts (Coge el valor máximo porque al ser FLSM todas tienen que ser iguales, pero eso no quiere decir que no puedas meter distintos tamaños en cada subred, por eso se coje el más grande para que todas sean iguales y quepan)

			StartSubnet(hosts);

			for (int i = 0; i < subnetsRequired; i++)
			{
				ipBaseCalculated = FillSubnet(i, ipBaseCalculated, hosts);
			}
		}

		public void SubnetVlsm(List<int> hosts, List<int> ipAddress, int mask, int subnetsRequired)
		{
			List<int> ipBaseCalculated = GetIPBase(ipAddress, mask);

			hosts = hosts.OrderByDescending(x => x).ToList();

			for (int i = 0; i < subnetsRequired; i++)
			{
				HostBits = Index.FindHostBits(hosts[i]); //Bits de hosts (Coge el valor máximo porque al ser FLSM todas tienen que ser iguales, pero eso no quiere decir que no puedas meter distintos tamaños en cada subred, por eso se coje el más grande para que todas sean iguales y quepan)

				StartSubnet(hosts);

				ipBaseCalculated = FillSubnet(i, ipBaseCalculated, hosts);
			}
		}

		private List<int> Range(List<int> list, char option)
		{
			List<int> result = new();
			int item;

			for (int i = 0; i < list.Count; i++)
			{
				if (i == list.Count - 1)
				{
					item = option == '+' ? list[i] + 1 : list[i] - 1;
				}
				else
				{
					item = list[i];
				}

				result.Add(item);
			}

			return result;
		}

		private IEnumerable<char> ConvertToBinary(int number)
		{
			string result = "";

			while (number > 0)
			{
				int rest = number % 2;
				number /= 2;
				result += rest;
			}

			return result.Reverse();
		}
		private List<string> DivideInOct(string mask)
		{
			List<string> result = new List<string>();

			string pattern = @"\d{1,8}";

			MatchCollection matches = Regex.Matches(mask, pattern);

			foreach (Match match in matches)
			{
				result.Add(match.Value);
			}

			return result;
		}
		private List<int> BinaryOperator(List<string> binary1, List<string> binary2, char option)
		{
			List<int> result = new();

			for (int i = 0; i < binary1.Count; i++)
			{
				int num1 = Convert.ToInt32(binary1[i], 2);
				int num2 = Convert.ToInt32(binary2[i], 2);

				//result.Add(option == '+' ? num1 + num2 : num1 & num2);

				if (option == '+')
				{
					if ((num1 + num2) < 255)
					{
						result.Add(num1 + num2);
					}
					else
					{
						result[i - 1] += 1;
						result.Add(0);
					}
				}
				else
				{
					result.Add(num1 & num2);
				}
			}

			return result;
		}
	}
}