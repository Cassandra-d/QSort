using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSort_Naive
{
    class Program
    {
        static void Swap(int[] arr, int i, int j)
        {
            int tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }

        static void SwapIf(int[] arr, int i, int j)
        {
            // does placing of this var outside of the if affect perf?
            var k = arr[j];
            if (arr[i] > arr[j])
            {
                arr[j] = arr[i];
                arr[i] = k;
            }
        }

        static bool SwapIf2(int[] arr, int i, int j)
        {
            var k = arr[j];
            if (arr[i] > arr[j])
            {
                arr[j] = arr[i];
                arr[i] = k;
                return true;
            }
            return false;
        }

        static void QSort(int[] arr, int lo, int hi)
        {
            // placing two below if's to the end of the func to avoid unnecessary call do decreases perf
            if (hi - lo == 0)
                return;
            if (hi - lo == 1)
            {
                if (arr[lo] > arr[hi])
                    Swap(arr, lo, hi);
                return;
            }

            int pivot = arr[(hi + lo) / 2];
            int i = lo;
            int j = hi;

            while (j > i)
            {
                while (arr[j] > pivot && j > i)
                {
                    --j;
                }

                while (arr[i] < pivot && i < j)
                {
                    ++i;
                }

                if (i < j)
                {
                    Swap(arr, i, j);
                    --j;
                }
            }

            QSort(arr, lo, i);
            QSort(arr, i + 1, hi);
        }

        // from https://visualgo.net/sorting
        static void QSort2(int[] arr, int lo, int hi)
        {
            if (hi - lo == 1)
            {
                SwapIf(arr, lo, hi);
                return;
            }

            int pivot = arr[lo];
            int storeIndex = lo + 1;
            int k = storeIndex;
            while (k <= hi)
            {
                if (arr[k] < pivot)
                {
                    if (k != storeIndex)
                        Swap(arr, k, storeIndex);
                    storeIndex += 1;
                }
                k += 1;
            }
            var wasSwap = SwapIf2(arr, lo, storeIndex - 1);

            if (storeIndex - 2 > lo)
                QSort2(arr, wasSwap ? lo : lo + 1, storeIndex - 2);
            if (storeIndex < hi)
                QSort2(arr, storeIndex, hi);
        }

        // from http://blog.bodurov.com/Visualizing-QuickSort-Algorithm/
        public static void QSort3(int[] list, int left, int right)
        {
            int initial_left = left;
            int initial_right = right;
            var pivotValue = list[initial_left];

            while (left < right)
            {
                while (list[right] > pivotValue && (left < right))
                {
                    right--;
                }
                if (left != right)
                {
                    list[left] = list[right];
                    left++;
                }
                while (list[left] < pivotValue && (left < right))
                {
                    left++;
                }
                if (left != right)
                {
                    list[right] = list[left];
                    right--;
                }
            }

            list[left] = pivotValue;
            int pivotIndex = left;

            if ((pivotIndex - 1) > initial_left) QSort3(list, initial_left, pivotIndex - 1);
            if ((pivotIndex + 1) < initial_right) QSort3(list, pivotIndex + 1, initial_right);
        }

        // not finished, only a proto
        static void QSortParallel(int[] arr, int lo, int hi, int lvl = 0)
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

            if (lvl > 4)
            {
                var tasks = new[]
                {
                    Task.Run(() => QSort(arr, lo, i)),
                    Task.Run(() => QSort(arr, i + 1, hi))
                };
                Task.WaitAll(tasks);
            }
            else
            {
                QSortParallel(arr, lo, i, lvl + 1);
                QSortParallel(arr, i + 1, hi, lvl + 1);
            }
        }

        static void Main(string[] args)
        {
            var arrays = GetTestArrays();
            long overallMS = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Go");
            foreach (var arr in arrays)
            {
                sw.Restart();
                QSort(arr, 0, arr.Length - 1);
                overallMS += sw.ElapsedMilliseconds;

                if (!IsSorted(arr))
                    Print(arr);
            }

            Console.WriteLine($"Done in {TimeSpan.FromMilliseconds(overallMS).TotalSeconds} seconds");
            Console.ReadKey();
        }

        private static List<int[]> GetTestArrays()
        {
            int cnt = 10000000;
            return new List<int[]>
            {
                // correctness
                new[] {1},
                new[] {1,2},
                new[] {2,1},
                new[] {2, 2, 3, 5, 3, 5},
                new[] { 3, 2, 4, 5, 1 },
                new[] { 3, 2, 5, 4, 1 },
                new[] { 3, 2, 5, 4, 1, 0 },
                new[] { 3, 2, 5, 4, 1, 1 },
                new[] { 1, 2, 3, 6, 5 },
                new[] { 1, 2, 3, 4, 5, 6 },
                new[] { 6, 5, 4, 3, 2, 1},
                new[] { 1, 2, 4, 5, 6, 3, 4, 5, 6, 6 },
                new[] { 1, 2, 4, 5, 6, 1, 2, 3, 4, 5, 6 },
                new[] { 1, 2, 4, 5, 6, 3, 3},
                new[] { 3, 3, 6, 5, 4, 2, 1 },
                new[] { 3, 3, 7, 1, 1 },
                new[] { 1, 1, 7, 3, 3 },
                new[] { 1, 1, 7, 7, 3, 3 },
                new[] { 3, 3, 7, 7, 1, 1 },
                new[] { 1, 7, 7, 7, 3 },
                new[] { 1, 2, 3, 4, 5},
                new[] { 5, 4, 3},
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new[] { 42, 9, 17, 54, 602, -3, 54, 999, -11 },
                new[] {-11, -3, 9, 17, 42, 54, 54, 602, 999 },
                new[] {1,1,1,1,1,1,1,1,1,1,0},
                new[] {1,1,1,1,1,1,1,1,1,0,0},
                new[] {1,1,1,1,1,1,1,1,0,0,0},
                new[] {9,1,1,1,1,1,1,1,1,1,1,1},
                new[] {9,9,1,1,1,1,1,1,1,1,1,1},
                new[] {9,9,9,1,1,1,1,1,1,1,1,1},

                // perf
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
                GetRandomArray(cnt),
            };
        }
        static int[] GetRandomArray(int cnt)
        {
            var rand = new Random((int)DateTime.Now.Ticks & 0x7FFFFFFF);
            var randArr = new int[cnt];
            for (int i = 0; i < cnt; i++)
            {
                randArr[i] = rand.Next();
            }
            return randArr;
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
