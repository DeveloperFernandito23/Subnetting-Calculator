using System.Text.RegularExpressions;
using Subnetting_Calculator.Pages;

namespace Subnetting_Calculator.Models
{
	public class Subnetting
	{
		public const int TOTALBITS = 32;
		public List<List<string>> paramsList = new List<List<string>>();

		public void SubnetFlsm(List<int> hosts, string ipAddress, int mask, int subnetsRequired)
		{
			List<string> ipInBinaryDivide = new List<string>();
			List<string> maskInBinaryDivide = new List<string>();

			foreach (var item in ipAddress.Split('.'))
			{
				ipInBinaryDivide.Add(string.Concat(ConvertToBinary(int.Parse(item))).PadLeft(8, '0').PadRight(8, '0'));
			}

			string maskInBinary = "".PadLeft(mask, '1').PadRight(32, '0');

			foreach (var item in DivideInOct(maskInBinary))
			{
				maskInBinaryDivide.Add(item);
			}

			List<string> ipBaseCalculated = MultiplyInBinary(ipInBinaryDivide, maskInBinaryDivide); //Calcula IP Base

			int hostBits = FindHostBits(hosts.Max(x => x)); //Bits de hosts (Coge el valor máximo porque al ser FLSM todas tienen que ser iguales, pero eso no quiere decir que no puedas meter distintos tamaños en cada subred, por eso se coje el más grande para que todas sean iguales y quepan)

			int newMask = TOTALBITS - hostBits; //Obtenemos la nueva máscara
			string newMaskInBinary = "".PadLeft(newMask, '1').PadRight(32, '0'); //Pasamos la máscara a binario

			List<string> newMaskInBinaryDivide = DivideInOct(newMaskInBinary); //Dividimos la máscara en octetos

			int changedPosition = FindChangedPosition(newMaskInBinaryDivide); //Calculamos la posición de la máscara que varía

			int jump = (int)Math.Pow(2, hostBits); //Calculamos el salto elevando dos a el número de bits de host sacado arriba

			List<string> jumpInBinary = DivideInOct(string.Concat(ConvertToBinary(jump)).PadLeft(32, '0').PadRight(32, '0')); //Convertimos el salto en binario

			for (int i = 0; i < subnetsRequired; i++)
			{
				List<string> ipBaseInBinary = new List<string>();

				ipBaseCalculated.ForEach(item => ipBaseInBinary.Add(string.Concat(ConvertToBinary(int.Parse(item))).PadLeft(8, '0').PadRight(8, '0'))); //Transformamos la IP Base en binario

				Console.WriteLine($"LAN {i + 1}");

				string lan = $"LAN {i + 1}";

				Console.WriteLine(hosts[i]);

				string host = hosts[i].ToString();

				Console.WriteLine($"Host Totales: {jump - 2}");

				string totalHost = (jump - 2).ToString();

				Console.WriteLine($"IP Base: {string.Join('.', ipBaseCalculated)}");

				string ipBase = string.Join('.', ipBaseCalculated);

				Console.WriteLine($"Máscara De Red: {string.Join('.', newMaskInBinaryDivide.Select(item => Convert.ToInt32(item, 2)))}"); //Transformamos la máscara en decimal

				string maskString = string.Join('.', newMaskInBinaryDivide.Select(item => Convert.ToInt32(item, 2)));

				Console.WriteLine($"CIDR: /{newMask}");

				string cidr = $"/{newMask}";

				string broadcastInBinary = string.Concat(ipBaseInBinary).Substring(0, newMask) + new string('1', hostBits); //Conseguimos el BroadCast en Binario

				List<string> broadcastInBinaryDivide = DivideInOct(broadcastInBinary); //Transformamos la IP del BroadCast en decimal

				List<string> broadcast = new();

				broadcastInBinaryDivide.ForEach(item => broadcast.Add(Convert.ToInt32(item, 2).ToString()));

				Console.WriteLine($"Hosts Disponibles: {string.Join('.', Range(ipBaseCalculated, '+'))} - {string.Join('.', Range(broadcast, '-'))}");

				string availableHost = $"{string.Join('.', Range(ipBaseCalculated, '+'))} - {string.Join('.', Range(broadcast, '-'))}";

				Console.WriteLine($"IP BroadCast: {string.Join('.', broadcast)}");

				string broadCast = string.Join('.', broadcast);

				List<string> nextIp = SumInBinary(ipBaseInBinary, jumpInBinary);

				Console.WriteLine("-------------------------------------------------");

				paramsList.Add(new() { lan, host, totalHost, ipBase, maskString, cidr, availableHost, broadCast });

                ipBaseCalculated = nextIp;
			}



			/*
			string prueba = string.Concat(ipInBinaryDivide);
			int takeBroadCast = SearchRaisedToTwo(jump);

			// POR FIIIIN, ESTA ES LA DIRECCION BROADCAST FINAL
			string ipBroadCast = prueba.Substring(0, 32 - takeBroadCast) + new string('1', takeBroadCast);

			List<string> ipAddressEndList = new List<string>();

			string ipAddressBase = string.Join('.', ipBaseCalculated);

			for (int i = 0; i < subnetsRequired; i++)
			{
				for (int j = 0; j < ipBaseCalculated.Count; j++)
				{
					if (j == 3)
					{
						ipAddressEndList.Add((int.Parse(ipBaseCalculated[j]) + (jump * (i + 1))).ToString());
					}
					else
					{
						ipAddressEndList.Add(ipBaseCalculated[j]);
					}
				}
				string ipAddressEnd = string.Join('.', ipAddressEndList);


				GetRange(ipAddressBase, ipAddressEnd);

				Console.WriteLine(i + "_-");
				ipAddressBase = ipAddressEnd;

				ipAddressEndList.Clear();
			}


			Console.WriteLine(Convert.ToInt32(prueba.Substring(0, 8), 2));
			*/
		}

		public void SubnetVlsm(List<int> hosts, string ipAddress, int mask, int subnetsRequired)
		{
			List<string> ipInBinaryDivide = new List<string>();
			List<string> maskInBinaryDivide = new List<string>();

			foreach (var item in ipAddress.Split('.'))
			{
				ipInBinaryDivide.Add(string.Concat(ConvertToBinary(int.Parse(item))).PadLeft(8, '0').PadRight(8, '0'));
			}

			string maskInBinary = "".PadLeft(mask, '1').PadRight(32, '0');

			foreach (var item in DivideInOct(maskInBinary))
			{
				maskInBinaryDivide.Add(item);
			}

			List<string> ipBaseCalculated = MultiplyInBinary(ipInBinaryDivide, maskInBinaryDivide); //Calcula IP Base

			int hostBits = FindHostBits(hosts.Max(x => x)); //Bits de hosts (Coge el valor máximo porque al ser FLSM todas tienen que ser iguales, pero eso no quiere decir que no puedas meter distintos tamaños en cada subred, por eso se coje el más grande para que todas sean iguales y quepan)

			int newMask = TOTALBITS - hostBits; //Obtenemos la nueva máscara
			string newMaskInBinary = "".PadLeft(newMask, '1').PadRight(32, '0'); //Pasamos la máscara a binario

			List<string> newMaskInBinaryDivide = DivideInOct(newMaskInBinary); //Dividimos la máscara en octetos

			int changedPosition = FindChangedPosition(newMaskInBinaryDivide); //Calculamos la posición de la máscara que varía

			int jump = (int)Math.Pow(2, hostBits); //Calculamos el salto elevando dos a el número de bits de host sacado arriba

			List<string> jumpInBinary = DivideInOct(string.Concat(ConvertToBinary(jump)).PadLeft(32, '0').PadRight(32, '0')); //Convertimos el salto en binario

			for (int i = 0; i < subnetsRequired; i++)
			{
				List<string> ipBaseInBinary = new List<string>();

				ipBaseCalculated.ForEach(item => ipBaseInBinary.Add(string.Concat(ConvertToBinary(int.Parse(item))).PadLeft(8, '0').PadRight(8, '0'))); //Transformamos la IP Base en binario

				Console.WriteLine($"LAN {i + 1}");

				string lan = $"LAN {i + 1}";

				Console.WriteLine(hosts[i]);

				string host = hosts[i].ToString();

				Console.WriteLine($"Host Totales: {jump - 2}");

				string totalHost = (jump - 2).ToString();

				Console.WriteLine($"IP Base: {string.Join('.', ipBaseCalculated)}");

				string ipBase = string.Join('.', ipBaseCalculated);

				Console.WriteLine($"Máscara De Red: {string.Join('.', newMaskInBinaryDivide.Select(item => Convert.ToInt32(item, 2)))}"); //Transformamos la máscara en decimal

				string maskString = string.Join('.', newMaskInBinaryDivide.Select(item => Convert.ToInt32(item, 2)));

				Console.WriteLine($"CIDR: /{newMask}");

				string cidr = $"/{newMask}";

				string broadcastInBinary = string.Concat(ipBaseInBinary).Substring(0, newMask) + new string('1', hostBits); //Conseguimos el BroadCast en Binario

				List<string> broadcastInBinaryDivide = DivideInOct(broadcastInBinary); //Transformamos la IP del BroadCast en decimal

				List<string> broadcast = new();

				broadcastInBinaryDivide.ForEach(item => broadcast.Add(Convert.ToInt32(item, 2).ToString()));

				Console.WriteLine($"Hosts Disponibles: {string.Join('.', Range(ipBaseCalculated, '+'))} - {string.Join('.', Range(broadcast, '-'))}");

				string availableHost = $"{string.Join('.', Range(ipBaseCalculated, '+'))} - {string.Join('.', Range(broadcast, '-'))}";

				Console.WriteLine($"IP BroadCast: {string.Join('.', broadcast)}");

				string broadCast = string.Join('.', broadcast);

				List<string> nextIp = SumInBinary(ipBaseInBinary, jumpInBinary);

				Console.WriteLine("-------------------------------------------------");

				paramsList.Add(new() { lan, host, totalHost, ipBase, maskString, cidr, availableHost, broadCast });

				ipBaseCalculated = nextIp;
			}

		}

		public List<string> Range(List<string> list, char option)
		{
			List<string> result = new List<string>();
			string item;

			for (int i = 0; i < list.Count; i++)
			{
				if (i == list.Count - 1)
				{
					item = option == '+' ? (int.Parse(list[i]) + 1).ToString() : (int.Parse(list[i]) - 1).ToString();
				}
				else
				{
					item = list[i];
				}

				result.Add(item);
			}

			return result;
		}

		public void GetRange(string ipAddressBase, string ipAddressEnd)
		{
			for (int i = int.Parse(ipAddressBase.Split('.')[3]); i < int.Parse(ipAddressEnd.Split('.')[3]); i++)
			{
				Console.WriteLine(ipAddressBase.Split('.')[0] + ipAddressBase.Split('.')[1] + ipAddressBase.Split('.')[2] + i);
			}
		}

		public IEnumerable<char> ConvertToBinary(int number)
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
		public List<string> DivideInOct(string mask)
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
		public List<string> MultiplyInBinary(List<string> list1, List<string> list2) //TODO REUTILIZAR MÉTODO DE SUMA Y MULTIPLICACIÓN PARA QUE SEAN EL MISMO
		{
			List<string> result = new List<string>();

			for (int i = 0; i < list1.Count; i++)
			{
				int num1 = Convert.ToInt32(list1[i], 2);
				int num2 = Convert.ToInt32(list2[i], 2);

				// & porque nose si os acordais que la operacion es un AND y la & que es? Pues eso
				result.Add((num1 & num2).ToString());
			}

			return result;
		}

		public List<string> SumInBinary(List<string> ip, List<string> jump)
		{
			List<string> result = new List<string>();

			for (int i = 0; i < ip.Count; i++)
			{
				int num1 = Convert.ToInt32(ip[i], 2);
				int num2 = Convert.ToInt32(jump[i], 2);

				result.Add((num1 + num2).ToString());
			}

			return result;
		}

		public int FindHostBits(int maxHost/*int subnetsRequired*/)
		{//He cambiado este método, porque lo que hacía antes era buscar el elevado para el número de subredes, y lo he cambiado para que directamente busque el elevado para el mayor número de hosts
			int number = 0;

			while (!(Math.Pow(2, number) - 2 >= maxHost)) number++;

			return number;


			//int count = 0;

			//while (!(Math.Pow(2, count) >= subnetsRequired))
			//{
			//	count++;
			//}

			//return count;
		}

		public int FindChangedPosition(List<string> mask) //11111111.11111111.11111111.110000000
		{
			int position = 0;
			bool change = true;

			while (change)
			{
				change = mask[position].All(bit => bit == '1');

				position = change ? position + 1 : position;
			}

			return position;
		}
	}
}