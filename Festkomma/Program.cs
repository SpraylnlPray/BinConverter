using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festkomma
{
	class Program
	{
		static SortedDictionary<string, CharData> _inputData = new SortedDictionary<string, CharData>();
		static string _message = string.Empty;

		static void Main(string[] args)
		{
			Console.WriteLine("Type your message");
			string message = Console.ReadLine();
			// string message = "adcabdcdb";
			_message = message.ToLower();
			Console.WriteLine("Input: {0} ", message);

			// Absolute Häufigkeiten
			foreach (char Char in _message)
			{
				if (!_inputData.Keys.Contains(Char.ToString()))
					_inputData[Char.ToString()] = CountLetter(_message, Char.ToString());
			}

			// kummulierte Häufigkeiten
			int cumAppearance = 0;
			foreach (var data in _inputData)
			{
				data.Value.cumAppearance = cumAppearance;
				cumAppearance += data.Value.absAppearance;
				Console.WriteLine("{0}: absAppearance: {1}, cumAppearance: {2}", data.Key, data.Value.absAppearance, data.Value.cumAppearance);
			}

			// eof einfügen
			CharData eof = new CharData
			{
				cumAppearance = cumAppearance
			};
			_inputData["eof"] = eof;
			Console.WriteLine("eof: absAppearance: {0}, cumAppearance: {1}", 1, eof.cumAppearance);

			int low = 0;
			int high = CalcualteUpperLimit();
			int range = high - low + 1;
			int Q1 = Convert.ToInt32(high / 4) + 1;
			int Q2 = 2 * Q1;
			int Q3 = 3 * Q1;

			Console.WriteLine("Initialization: ");
			Console.WriteLine("Low: {0} High: {1} Q1: {2} Q2: {3} Q3: {4} range: {5}", low, high, Q1, Q2, Q3, range);

			int savedNumbers = 0;
			string output = string.Empty;

			foreach (char Char in _message)
			{
				var data = _inputData[Char.ToString()];

				range = high - low + 1;
				var indexOfCurrentChar = GetWBIndexOfChar(Char);
				var nextEntry = GetEntryOfNextIndex(indexOfCurrentChar);
				high = low + Convert.ToInt32(range * nextEntry.Value.cumAppearance / _inputData.Last().Value.cumAppearance) - 1;
				low = low + Convert.ToInt32(range * data.cumAppearance / _inputData.Last().Value.cumAppearance);

				while (true)
				{
					if (high < Q2)
					{
						output += 0.ToString();
						while (savedNumbers > 0)
						{
							output += 1.ToString();
							savedNumbers--;
						}
					}
					else if (low >= Q2)
					{
						output += 1.ToString();
						while (savedNumbers > 0)
						{
							output += 0.ToString();
							savedNumbers--;
						}
						low -= Q2;
						high -= Q2;
					}
					else if (low >= Q1 && high < Q3)
					{
						savedNumbers++;
						low -= Q1;
						high -= Q1;
					}
					else
						break;

					low = 2 * low;
					high = 2 * high + 1;
				}
			}

			savedNumbers++;
			if (low < Q1)
			{
				output += 0.ToString();
				while (savedNumbers > 0)
				{
					output += 1.ToString();
					savedNumbers--;
				}
			}
			else
			{
				output += 1.ToString();
				while (savedNumbers > 0)
				{
					output += 0.ToString();
					savedNumbers--;
				}
			}

			Console.WriteLine("Encoding result: \n {0}", output);

			Console.ReadLine();
		}

		static int CalcualteUpperLimit()
		{
			int highestCumAppearance = _inputData.Aggregate((x, y) => x.Value.cumAppearance > y.Value.cumAppearance ? x : y).Value.cumAppearance;
			int M = 0;
			int B = 0;
			int Q1 = 0;
			while (highestCumAppearance > Q1)
			{
				M = Convert.ToInt32(Math.Pow(2, B) - 1);
				Q1 = Convert.ToInt32(M / 4) + 1;
				B++;
			}
			return M;
		}

		static CharData CountLetter(string message, string letter)
		{
			CharData data = new CharData();
			foreach (char Char in message)
				if (Char.ToString() == letter)
					data.absAppearance++;

			return data;
		}

		static KeyValuePair<string, CharData> GetEntryOfNextIndex(int index)
		{
			index = index + 1;
			var asList = _inputData.ToList();
			var data = asList[index];
			return data;
		}


		static int GetWBIndexOfChar(char keyToFind)
		{
			int index = 0;
			foreach (var entry in _inputData)
			{
				if (entry.Key == keyToFind.ToString())
					return index;
				index++;
			}
			return index;
		}
	}
}
