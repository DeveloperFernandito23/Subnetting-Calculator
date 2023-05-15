namespace Subnetting_Calculator.Models
{
	public class Subnet
	{
		private string _name;
		private int _size;
		private int _totalSize;
		private List<int> _ipBase;
		private Dictionary<List<int>, List<int>> _range;
		private List<int> _broadcast;
		private List<int> _mask;
		private int _cidr;

		public string Name { get => _name; set => _name = value; }
		public int Size { get => _size; set => _size = value; }
		public int TotalSize { get => _totalSize; set => _totalSize = value; }
		public List<int> IPBase { get => _ipBase; set => _ipBase = value; }
		public Dictionary<List<int>, List<int>> Range { get => _range; set => _range = value; }
		public List<int> Broadcast { get => _broadcast; set => _broadcast = value; }
		public List<int> Mask { get => _mask; set => _mask = value; }
		public int Cidr { get => _cidr; set => _cidr = value; }
	}
}
