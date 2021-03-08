using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Task2
{
	class Program
	{
		static void Main(string[] args)
		{
			if( args.Length == 0)
			{
				throw new Exception("Отсутсвуют входные данные!");
			}

			try
			{
				string inputData = FileRead(args[0]);
				float[][] quadrangleCoordinates = ArrayPrepare(inputData);

				inputData = FileRead(args[1]);
				float[][] pointCoordinates = ArrayPrepare(inputData);

				float[] result = new float[quadrangleCoordinates[0].Length];

				for (int i = 0; i < pointCoordinates[0].Length; i++)
				{
					int j = 0;

					do
					{
						result[j] = (quadrangleCoordinates[0][j] - pointCoordinates[0][i]) *
						(quadrangleCoordinates[1][j + 1] - quadrangleCoordinates[1][j]) -
						(quadrangleCoordinates[0][j + 1] - quadrangleCoordinates[0][j]) *
						(quadrangleCoordinates[1][j] - pointCoordinates[1][i]);

						j++;

					} while (j != (quadrangleCoordinates[0].Length - 1));

					result[j] = (quadrangleCoordinates[0][j] - pointCoordinates[0][i]) *
						(quadrangleCoordinates[1][0] - quadrangleCoordinates[1][j]) -
						(quadrangleCoordinates[0][0] - quadrangleCoordinates[0][j]) *
						(quadrangleCoordinates[1][j] - pointCoordinates[1][i]);

					Console.WriteLine(Output(result));
				}
			}
			catch (FormatException)
			{
				string delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				Console.WriteLine($"Внимание! В качестве десятичного разделителя используйте \'{delimiter}\'");
			}
		}
	static int Output(float[] array)
		{
			int countNegative = 0;
			int countNormal = 0;
			int countZero = 0;

			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == 0)
					countZero++;
				else if (float.IsNegative(array[i]))
					countNegative++;
				else if (float.IsNormal(array[i]))
					countNormal++;
			}

			if (countNegative == array.Length || countNormal == array.Length)
			{
				return 2;
			}
			else if ((countNegative == (array.Length - 1) || countNormal == (array.Length - 1)) && countZero == 1)
			{
				return 1;
			}
			else if ((countNegative == (array.Length - 2) || countNormal == (array.Length - 2)) && countZero == 2)
			{
				return 0;
			}
			else
				return 3;
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

				stream.Close();
			}

			return result;
		}
	static float[][] ArrayPrepare(string inputText) //prepare array with coordinates
		{
			const int coordinates = 2;
			int x = 0;
			int y = 0;

			try
			{
				string[] array = inputText.Split(new char[] {'\r', ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				int count = array.Length / 2;
				float[][] result = new float[coordinates][] { new float[count], new float[count] };

				for (int i = 0; i < array.Length; i++)
				{
					if ((i % 2) == 0)
					{
						result[0][x] = float.Parse(array[i]);//[0] - all coordinates "X"
						if (result[0][x] > float.MaxValue || result[0][x] < float.MinValue)
							throw new Exception($"The range of the float type is {float.MinValue} to {float.MaxValue}");
						x++;
					}
					else
					{
						result[1][y] = float.Parse(array[i]);//[1] - all coordinates "Y"
						if (result[1][y] > float.MaxValue || result[1][y] < float.MinValue)
							throw new Exception($"The range of the float type is {float.MinValue} to {float.MaxValue}");
						y++;
					}
				}

				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine($"{e.GetType()}! {e.Message}");
				throw;
			}
		}
	}
}
