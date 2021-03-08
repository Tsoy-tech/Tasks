using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Task3
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				throw new Exception("Отсутсвуют входные данные!");
			}

			string[] stringData = new string[] { };
			float[][] queue = new float[args.Length][];

			for (int index = 0; index < args.Length; index++)
			{
				string inputData = FileRead($@"{args[index]}");
				stringData = StringPrepare(inputData);
				queue[index] = ParseFloat(stringData);
			}

			float[] result = new float[queue[0].Length];

			for (int index = 0; index < args.Length; index++)
			{
				for (int i = 0; i < stringData.Length; i++)
				{
					result[i] += queue[index][i];
				}
			}

			Console.WriteLine($"{Array.IndexOf(result, Maximum(result)) + 1}");
		}
		static float Maximum(float[] digits)
		{
			return digits.Max();
		}
		static string FileRead(string fileName)
		{
			string result = string.Empty;

			using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				const int readBufferSize = 100;
				byte[] bytesOfContent = new byte[readBufferSize];
				int bytesRead;

				do
				{
					bytesRead = stream.Read(bytesOfContent, 0, readBufferSize);
					result += Encoding.ASCII.GetString(bytesOfContent, 0, bytesRead);

				} while (bytesRead > 0);
			}

			return result;
		}
		static string[] StringPrepare(string inputText)
		{
			try
			{
				do
				{
					string[] stringData = inputText.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

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
		static float[] ParseFloat(string[] stringData)
		{
			List<float> digits = new List<float>() { };

			for (int i = 0; i < stringData.Length; i++)
			{
				try
				{
					float number = float.Parse(stringData[i]);
					digits.Add(number);
				}
				catch(FormatException)
				{ 
					Console.WriteLine("Ошибка! " +
						$"В качестве входных данных могут быть только числа в диапозоне от {float.MinValue} до {float.MaxValue}!");
						string delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
						Console.WriteLine($"Внимание! В качестве десятичного разделителя используйте \'{delimiter}\'");
					throw;
				}
			}

			return digits.ToArray();
		}
	}
}
