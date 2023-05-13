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

			int jump = 256 - int.Parse(maskInBinaryDivide.Last());
			string prueba = string.Concat(ipInBinaryDivide);
			int takeBroadCast = SearchRaisedToTwo(jump);

			// POR FIIIIN, ESTA ES LA DIRECCION BROADCAST FINAL
			string ipBroadCast = prueba.Substring(0, 32 - takeBroadCast) + new string('1', takeBroadCast);
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
