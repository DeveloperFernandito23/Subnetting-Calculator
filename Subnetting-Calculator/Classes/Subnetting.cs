using System.Net;
using System.Text.RegularExpressions;

namespace Subnetting_Calculator.Classes
{
	public class Subnetting
	{
		public void SubnetFlsm(string ipAddress, int mask, int subnetsRequired)
		{
			List<string> maskInBinaryDivide = new List<string>();
			List<string> ipInBinaryDivide = new List<string>();

			foreach (var item in ipAddress.Split('.'))
			{
				ipInBinaryDivide.Add(String.Concat(ConvertToBinary(int.Parse(item))).PadLeft(8, '0').PadRight(8, '0'));
			}

			string maskInBinary = "".PadLeft(mask, '1').PadRight(32, '0');

			foreach (var item in DivideInOct(maskInBinary))
			{
				maskInBinaryDivide.Add(item);
			}

			List<string> ipBaseCalculated = MultiplyInBinary(ipInBinaryDivide, maskInBinaryDivide);

			int raisedTwoSubnet = SearchRaisedToTwo(subnetsRequired);

			int newMask = mask + raisedTwoSubnet;
			string newMaskInBinary = "".PadLeft(newMask, '1').PadRight(32, '0');

			List<string> newMaskInBinaryDivide = new List<string>();

			newMaskInBinaryDivide = DivideInOct(newMaskInBinary);

			int jump = 256 - Convert.ToInt32(newMaskInBinaryDivide.Last(), 2);
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

		}

		public void GetRange(string ipAddressBase, string ipAddressEnd)
		{
			for(int i = int.Parse(ipAddressBase.Split('.')[3]); i < int.Parse(ipAddressEnd.Split('.')[3]); i++)
			{
                Console.WriteLine(ipAddressBase.Split('.')[0] + ipAddressBase.Split('.')[1]+ ipAddressBase.Split('.')[2] + i);
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
		public List<string> MultiplyInBinary(List<string> list1, List<string> list2)
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
		public int SearchRaisedToTwo(int subnetsRequired)
		{
			int count = 0;

			while (!(Math.Pow(2, count) >= subnetsRequired))
			{
				count++;
			}

			return count;
		}
	}
}
