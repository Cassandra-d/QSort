using System;

namespace QSort_Naive
{
    class Program
    {
        static void SwapIf(int[] arr, int i, int j)
        {
            var k = arr[j];
            if (arr[i] > arr[j])
            {
                arr[j] = arr[i];
                arr[i] = k;
            }
        }

        static void QSort(int[] arr, int lo, int hi)
        {
            if (hi - lo == 0)
                return;
            if (hi - lo == 1)
            {
                SwapIf(arr, lo, hi);
                return;
            }

            int pivot = arr[(hi + lo) / 2];

            int i = lo;
            int j = hi;

            while (j > i)
            {
                if (arr[j] > pivot)
                {
                    --j;
                    continue;
                }
                while (i < j)
                {
                    if (arr[i] < pivot)
                    {
                        ++i;
                        continue;
                    }
                    break;
                }
                if (i < j)
                {
                    SwapIf(arr, i, j);
                    --j;
                }
            }

            QSort(arr, lo, i);
            QSort(arr, i + 1, hi);
        }

        static void Main(string[] args)
        {
            var arrays = new[]
            {
                new[] { 3, 2, 4, 5, 1 },
                new[] { 1, 2, 3, 6, 5 },
                new[] { 6, 5, 4, 3, 2, 1 },
                new[] { 1, 2, 4, 5, 6, 3, 4, 5, 6, 6 },
                new[] { 1, 2, 3, 4, 5, 6 },
                new[] { 1, 2, 4, 5, 6, 3, 3},
                new[] { 3, 3, 6, 5, 4, 2, 1 },
                new[] { 3, 3, 7, 1, 1 },
                new[] { 1, 1, 7, 3, 3 },
                new[] { 1, 1, 7, 7, 3, 3 },
                new[] { 3, 3, 7, 7, 1, 1 },
                new[] { 1, 7, 7, 7, 3 },
                new[] {1,2,3,4,5,6},
                new[] {6,5,4,3,2,1},
                new[] {1,2,3,4,5},
                new[] {5,4,3,2,1},
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new[] {10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
                new[] {42, 9, 17, 54, 602, -3, 54, 999, -11 },
                new[] {-11, -3, 9, 17, 42, 54, 54, 602, 999 },
                new[] {1,1,1,1,1,1,1,1,1,1,0},
                new[] {1,1,1,1,1,1,1,1,1,0,0},
                new[] {1,1,1,1,1,1,1,1,0,0,0},
                new[] {9,1,1,1,1,1,1,1,1,1,1,1},
                new[] {9,9,1,1,1,1,1,1,1,1,1,1},
                new[] {9,9,9,1,1,1,1,1,1,1,1,1}
            };

            foreach (var arr in arrays)
            {
                QSort(arr, 0, arr.Length - 1);
                if (!IsSorted(arr))
                    Print(arr);
                else
                    Console.WriteLine("Sorted");
            }

            Console.ReadKey();
        }

        static bool IsSorted(int[] arr)
        {
            for (int i = 0; i < arr.Length - 2; i++)
            {
                if (arr[i] > arr[i + 1])
                    return false;
            }
            return true;
        }
        static void Print(int[] arr)
        {
            foreach (var n in arr)
            {
                Console.Write(n + " ");
            }

            Console.WriteLine();
        }
    }
}
