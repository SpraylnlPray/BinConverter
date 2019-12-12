using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festkomma
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Activate test mode? (y)es/(n)o");
			string testmode = Console.ReadLine();

			Compressor compressor = new Compressor(testmode.ToLower() == "y");
			compressor.CompressMessage();
			
			Console.ReadLine();
		}
	}
}