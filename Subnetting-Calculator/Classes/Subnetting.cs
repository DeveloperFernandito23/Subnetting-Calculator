namespace Subnetting_Calculator.Classes
{
	public class Subnetting
	{
		public void SubnetFlsm(string ipAddress, int mask)
		{
			int firstOct = int.Parse(ipAddress.Split('.')[0]);
			int secondOct = int.Parse(ipAddress.Split('.')[1]);
			int thirstOct = int.Parse(ipAddress.Split('.')[2]);
			int fourthtOct = int.Parse(ipAddress.Split('.')[3]);

			string firsOctInBinary = String.Concat(ConvertToBinary(firstOct)).PadRight(8, '0').PadLeft(8, '0').PadRight(8, '0');
			string secondOctInBinary = String.Concat(ConvertToBinary(secondOct)).PadRight(8, '0').PadLeft(8, '0').PadRight(8, '0');
			string thirstOctInBinary = String.Concat(ConvertToBinary(thirstOct)).PadRight(8, '0').PadLeft(8, '0').PadRight(8, '0');
			string fourthtOctInBinary = String.Concat(ConvertToBinary(fourthtOct)).PadRight(8, '0').PadLeft(8, '0').PadRight(8, '0');



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
	}
}
