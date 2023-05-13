using System.Text.RegularExpressions;

namespace Subnetting_Calculator.Classes
{
	public class Subnetting
	{
		public void SubnetFlsm(string ipAddress, int mask)
		{
			List<string> maskInBinaryDivide = new List<string>();
			List<string> ipInBinaryDivide = new List<string>();

			foreach (var item in ipAddress.Split('.'))
			{
				ipInBinaryDivide.Add(String.Concat(ConvertToBinary(int.Parse(item))).PadRight(8, '0').PadLeft(8, '0'));
			}

			string maskInBinary = "".PadLeft(mask, '1').PadRight(32, '0');

			foreach (var item in DivideInOct(maskInBinary))
			{
				maskInBinaryDivide.Add(item);
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
	}
}
