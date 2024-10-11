namespace MultiThreadedSum
{
	class Program
	{
		private static int sum = 0;

		private static readonly object sumLock = new();

		private static int[] numbers;

		static void Main(string[] args)
		{
			InitializeArray(1000);

			DisplayArray();

			int numberOfThreads = 4;

			int segmentSize = numbers.Length / numberOfThreads;
			Thread[] threads = new Thread[numberOfThreads];

			for (int i = 0; i < numberOfThreads; i++)
			{
				int start = i * segmentSize;
				int end = (i == numberOfThreads - 1) ? numbers.Length : start + segmentSize;

				threads[i] = new Thread(() => SumSegment(start, end));
				threads[i].Start();
			}

			foreach (Thread thread in threads)
			{
				thread.Join();
			}

			Console.WriteLine($"Сума всіх елементів масиву: {sum}");
		}

		private static void InitializeArray(int size)
		{
			numbers = new int[size];
			Random rand = new();

			for (int i = 0; i < size; i++)
			{
				numbers[i] = rand.Next(1, 1000);
			}
		}

		private static void DisplayArray()
		{
			Console.WriteLine(string.Join(", ", numbers));
			Console.WriteLine();
		}

		private static void SumSegment(int start, int end)
		{
			int localSum = 0;
			for (int i = start; i < end; i++)
			{
				localSum += numbers[i];
			}

			lock (sumLock)
			{
				sum += localSum;
			}
		}
	}
}
