using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task1
{
	class Program
	{
		static void Main(string[] args)
		{
			if(args.Length == 0)
			{
				throw new Exception("Отсутствуют входные данные!");
			}

			string inputText = FileRead($@"{args[0]}");
			string[] stringData = StringPrepare(inputText);
			List<double> digits = ParseDouble(stringData);

			Console.WriteLine(OutputString(digits));
		}

		static string FileRead(string fileName)
		{
			string result = string.Empty;

			FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

			const int readBufferSize = 100;
			byte[] bytesOfContent = new byte[readBufferSize];
			int bytesRead;

			do
			{
				bytesRead = stream.Read(bytesOfContent, 0, readBufferSize);
				result += Encoding.ASCII.GetString(bytesOfContent, 0, bytesRead);

			} while (bytesRead > 0);

			stream.Close();

			return result;
		}
		static List<double> ParseDouble(string[] stringData)
		{
			List<double> digits = new List<double>() { };

			for (int i = 0; i < stringData.Length; i++)
			{
				try
				{
					double number = double.Parse(stringData[i]);
					digits.Add(number);
				}
				catch
				{
					Console.WriteLine("Ошибка! " +
						$"В качестве входных данных могут быть только числа в диапозоне от {short.MinValue} до {short.MaxValue}!");
					throw;
				}
			}

			return digits;
		}
		static string[] StringPrepare(string inputText)
		{
			try
			{
				do
				{
					string[] stringData = inputText.Split(new char[] { '\r', ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

					return stringData;

					if (stringData.Length > 1)
						break;

					Console.WriteLine("Слишком мало цифр:( Попробуйте ещё раз:");
				} while (true);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Ошибка {e.GetType()}!");
				throw;
			}
		}
		static string OutputString(List<double> digits)
		{
			return $"{Percentile(digits):f}\n" +
				$"{Median(digits):f}\n" +
				$"{Maximum(digits):f}\n" +
				$"{Minimum(digits):f}\n" +
				$"{Average(digits):f}\n";
		}
		static double Percentile(List<double> digits, double percentile = 0.9)
		{
			digits.Sort();

			int N = digits.Count;
			double n = (N - 1) * percentile + 1;

			if (n == 1d)
				return digits[0];

			else if (n == N)
				return digits[N - 1];

			else
			{
				int k = (int)n;
				double d = n - k;
				return digits[k - 1] + d * (digits[k] - digits[k - 1]);
			}
		}
		static double Median(List<double> digits)
		{
			digits.Sort();
			int index = digits.Count;

			if (digits.Count % 2 == 0)
			{
				return (digits[index / 2] + digits[index / 2 - 1]) / 2;
			}
			else
			{
				return digits[index / 2];
			}
		}
		static double Maximum(List<double> digits)
		{
			return digits.Max();
		}
		static double Minimum(List<double> digits)
		{
			return digits.Min();
		}
		static double Average(List<double> digits)
		{
			return digits.Average();
		}
	}
}