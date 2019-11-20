﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festkomma
{
	class Compressor
	{
		SortedDictionary<string, CharData> _inputData = new SortedDictionary<string, CharData>(new DataComparer());
		string _message = string.Empty;
		string _output = string.Empty;
		int _savedNumbers = 0;

		int _low;
		int _high;
		int _range;
		int _Q1;
		int _Q2;
		int _Q3;

		public Compressor()
		{
			Initialize();
		}

		void Initialize()
		{
			GetAbsolutAppearances();
			GetCummulatedAppearances();

			_low = 0;
			_high = CalcualteUpperLimit();
			_range = _high - _low + 1;
			_Q1 = Convert.ToInt32(_high / 4) + 1;
			_Q2 = 2 * _Q1;
			_Q3 = 3 * _Q1;
		}

		void GetAbsolutAppearances()
		{
			foreach (char Char in _message)
			{
				if (!_inputData.Keys.Contains(Char.ToString()))
					_inputData[Char.ToString()] = CountLetter(_message, Char.ToString());
			}
		}

		void GetCummulatedAppearances()
		{
			int cumAppearance = 0;
			foreach (var data in _inputData)
			{
				data.Value.cumAppearance = cumAppearance;
				cumAppearance += data.Value.absAppearance;
				Console.WriteLine("{0}: absAppearance: {1}, cumAppearance: {2}", data.Key, data.Value.absAppearance, data.Value.cumAppearance);
			}
			CharData eof = new CharData
			{
				cumAppearance = cumAppearance
			};
			_inputData["eof"] = eof;
		}

		void CompressMessage()
		{
			foreach (char Char in _message)
			{
				var data = _inputData[Char.ToString()];

				_range = _high - _low + 1;
				var indexOfCurrentChar = GetWBIndexOfChar(Char);
				var nextEntry = GetWBEntryOfNextIndex(indexOfCurrentChar);
				_high = _low + Convert.ToInt32(_range * nextEntry.Value.cumAppearance / _inputData.Last().Value.cumAppearance) - 1;
				_low = _low + Convert.ToInt32(_range * data.cumAppearance / _inputData.Last().Value.cumAppearance);

				while (true)
				{
					if (_high < _Q2)
					{
						_output += 0.ToString();
						while (_savedNumbers > 0)
						{
							_output += 1.ToString();
							_savedNumbers--;
						}
					}
					else if (_low >= _Q2)
					{
						_output += 1.ToString();
						while (_savedNumbers > 0)
						{
							_output += 0.ToString();
							_savedNumbers--;
						}
						_low -= _Q2;
						_high -= _Q2;
					}
					else if (_low >= _Q1 && _high < _Q3)
					{
						_savedNumbers++;
						_low -= _Q1;
						_high -= _Q1;
					}
					else
						break;

					_low = 2 * _low;
					_high = 2 * _high + 1;
				}
			}

			_savedNumbers++;
			if (_low < _Q1)
			{
				_output += 0.ToString();
				while (_savedNumbers > 0)
				{
					_output += 1.ToString();
					_savedNumbers--;
				}
			}
			else
			{
				_output += 1.ToString();
				while (_savedNumbers > 0)
				{
					_output += 0.ToString();
					_savedNumbers--;
				}
			}
		}


		int CalcualteUpperLimit()
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

		CharData CountLetter(string message, string letter)
		{
			CharData data = new CharData();
			foreach (char Char in message)
				if (Char.ToString() == letter)
					data.absAppearance++;

			return data;
		}

		KeyValuePair<string, CharData> GetWBEntryOfNextIndex(int index)
		{
			index = index + 1;
			var asList = _inputData.ToList();
			var data = asList[index];
			return data;
		}


		int GetWBIndexOfChar(char keyToFind)
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