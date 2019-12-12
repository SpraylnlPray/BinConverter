using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festkomma
{
	public class DataComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			if (x.Length == y.Length)
				return x.CompareTo(y);
			return x.Length.CompareTo(y.Length);
		}
	}
}
