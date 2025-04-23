using System;
using System.Diagnostics;

public class Program
{
    private static readonly Random rnd = new Random();
    private static readonly int length = 1000000000;
    private static readonly int[] array = new int[length];
    private static readonly int threadsCount = Environment.ProcessorCount;

    private static readonly Thread[] threads = new Thread[threadsCount];
    private static int minArrayElement = int.MaxValue;

    private static readonly object minLock = new object();

    private static int completedThreads = 0;
    private static readonly object countLock = new object();
    private static Stopwatch stopwatch = new Stopwatch();
    public static void Main(string[] args)
    {
        GenerateArray();
        Console.WriteLine("Array generated");
        int chunkSize = (int)Math.Ceiling(length / (double)threadsCount);

        stopwatch.Start();
        for(int i = 0; i < threadsCount; i++)
        {
            int threadIndex = i;
            int start = threadIndex * chunkSize;
            int end = Math.Min(start + chunkSize, length);

            threads[i] = new Thread(() => FindMinInRange(start, end));
            threads[i].Start();
        }

        lock (countLock)
        {
            while (completedThreads < threadsCount)
            {
                Monitor.Wait(countLock);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Parallel min = {minArrayElement} in {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Reset();

        stopwatch.Start();
        int singleThreadedMinElement = FindMinSingleThreaded();
        stopwatch.Stop();

        Console.WriteLine($"Single-threaded min = {singleThreadedMinElement} in {stopwatch.ElapsedMilliseconds} ms");
    }

    private static void FindMinInRange(int start, int end)
    {
        int localMin = int.MaxValue;

        for (int i = start; i < end; i++)
        {
            if (array[i] < localMin)
            {
                localMin = array[i];
            }
        }

        lock (minLock)
        {
            if (localMin < minArrayElement)
            {
                minArrayElement = localMin;
            }
        }

        lock (countLock)
        {
            completedThreads++;
            Monitor.Pulse(countLock);
        }
    }

    private static int FindMinSingleThreaded()
    {
        int min = int.MaxValue;
        for (int i = 0; i < length; i++)
        {
            if (array[i] < min)
            {
                min = array[i];
            }
        }
        return min;
    }

    private static void GenerateArray()
    {
        for (int i = 0; i < length; i++)
        {
            array[i] = rnd.Next(int.MinValue, int.MaxValue);
        }
    }
}