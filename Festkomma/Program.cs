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
			Console.WriteLine("Type your message");
			string message = Console.ReadLine();

			Compressor compressor = new Compressor(message.ToLower());
			compressor.CompressMessage();

			Console.ReadLine();
		}
	}
}