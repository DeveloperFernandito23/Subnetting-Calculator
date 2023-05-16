namespace Subnetting_Calculator.Models
{
	public class Subnet
	{
		private string _name;
		private int _size;
		private int _totalSize;
		private List<int> _ipBase;
		private List<int> _rangeStart;
		private List<int> _rangeEnd;
		private List<int> _broadcast;
		private IEnumerable<int> _mask;
		private int _cidr;

		public string Name { get => _name; set => _name = value; }
		public int Size { get => _size; set => _size = value; }
		public int TotalSize { get => _totalSize; set => _totalSize = value; }
		public List<int> IPBase { get => _ipBase; set => _ipBase = value; }
		public List<int> RangeStart { get => _rangeStart; set => _rangeStart = value; }
		public List<int> RangeEnd { get => _rangeEnd; set => _rangeEnd = value; }
		public List<int> Broadcast { get => _broadcast; set => _broadcast = value; }
		public IEnumerable<int> Mask { get => _mask; set => _mask = value; }
		public int CIDR { get => _cidr; set => _cidr = value; }
		
		public Subnet()
		{
		}
	}
}
