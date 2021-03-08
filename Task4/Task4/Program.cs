using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Task4
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int> clientsQuantity = new List<int>();
			List<DateTimeOffset> timeList = new List<DateTimeOffset>();

			DateTimeOffset.TryParse("8:00", out DateTimeOffset workTime);
			DateTimeOffset.TryParse("20:00", out DateTimeOffset closeTime);

			string inputText = FileRead(args[0]);//"8:00 8:30\n8:15 8:45\n8:45 9:00\n8:30 9:00\n9:00 9:30\n9:10 9:20\n";
			string[] arrayElements = inputText.Split('\n', StringSplitOptions.RemoveEmptyEntries);

			DateTimeOffset[] arrivalTime = new DateTimeOffset[arrayElements.Length];//args.Length
			DateTimeOffset[] departureTime = new DateTimeOffset[arrayElements.Length];

			string[][] stringData = StringPrepare(inputText);

			for (int i = 0; i < stringData.Length; i++)
			{
				DateTimeOffset.TryParse(stringData[i][0], out arrivalTime[i]);
				DateTimeOffset.TryParse(stringData[i][1], out departureTime[i]);
			}

			RegistrationVisitors(arrivalTime, departureTime, workTime, closeTime, out clientsQuantity, out timeList);

			Output(clientsQuantity, timeList);
		}
		static void RegistrationVisitors(DateTimeOffset[] arrivalTime, DateTimeOffset[] departureTime, DateTimeOffset workTime, DateTimeOffset closeTime,
			out List<int> listOfClients, out List<DateTimeOffset> listOfTime)
		{
			int clientsCount = 0;
			listOfClients = new List<int>();
			listOfTime = new List<DateTimeOffset>();

			int k = 0;
			int l = 0;

			Array.Sort(arrivalTime);
			Array.Sort(departureTime);

			do
			{
				if (k < arrivalTime.Length && workTime.Equals(arrivalTime[k]))
				{
					int count = SameCount(arrivalTime, l);

					clientsCount += count;

					if (arrivalTime[k] != departureTime[l])
					{
						listOfTime.Add(arrivalTime[k]);
						listOfClients.Add(clientsCount);
					}

					k += count;

					if (k >= arrivalTime.Length)
						k = 0;

				}

				if (l < arrivalTime.Length && workTime.Equals(departureTime[l]))
				{
					int count = SameCount(departureTime, l);

					clientsCount -= count;

					if (departureTime[l] != arrivalTime[k])
					{
						listOfTime.Add(departureTime[l]);
						listOfClients.Add(clientsCount);
					}

					l += count;

					if (l >= departureTime.Length) 
						l = 0;
				}

				workTime += TimeSpan.FromMinutes(5);

				if (workTime.DateTime > closeTime.DateTime)
					break;

			} while (true);
		}
		static void Output(List<int> clientsQuantity, List<DateTimeOffset> timeList)
		{
			string result = string.Empty;
			int max = clientsQuantity.Max();
			var indexes = clientsQuantity
				.Select((t, i) => new { Index = i, Text = t })
				.Where(p => p.Text == max)
				.Select(p => p.Index);

			foreach (var i in indexes)
			{
				result = $"{timeList[i]:t} ";

				if (clientsQuantity[i - 1] >= 0 &&
					clientsQuantity[i] == clientsQuantity[i - 1] &&
					clientsQuantity[i] == clientsQuantity[i + 1])
				{
					continue;
				}

				else if (clientsQuantity[i] == clientsQuantity[i - 1] &&
					clientsQuantity[i] != clientsQuantity[i + 1])
				{
					result = $"{timeList[i + 1]:t}\n";
				}

				Console.Write(result);
			}

			if (timeList.Count > (indexes.Last() + 1))
				Console.WriteLine($"{timeList[indexes.Last() + 1]:t}");
		}
		static string[][] StringPrepare(string inputText)
		{
			try
			{
				do
				{
					int i = 0;

					string[] stringData = inputText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
					string[][] result = new string[stringData.Length][];

					for (int j = 0; j < stringData.Length; j++)
					{
						result[i] = stringData[j].Split(' ', StringSplitOptions.RemoveEmptyEntries);
						i++;
					}

					return result;

					if (stringData.Length > 1)
						break;

				} while (true);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Ошибка {e.GetType()}!");
				throw;
			}
		}
		static string FileRead(string fileName)
		{
			string result = string.Empty;

			using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{

				const int readBufferSize = 1024;
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
		static int SameCount(DateTimeOffset[] array, int index)
		{
			int l = index;
			int count = 1;

			if (l < (array.Length - 1) && array[l] == array[l + 1])
			{
				do
				{
					count++;
					l++;
				} while ((l == (array.Length - 1)) || (array[l - 1] != array[l]));
			}

			return count;
		}
	}
}
