using System;

namespace Automat
{
    public static class ProcessArray
    {
        static int[] a1 = { 1, 2, 3 };
        static int[] a2 = { 4, 5, 6 };
        static int[] a3 = { 7, 8, 9, 10, 11 };
        static int[] a4 = { 50, 58, 90, 91 };

        static int[][] arr = { a1, a2, a3, a4 };

        public static void TestArray()
        {
            foreach (var t in arr)
            {
                for (int j = 0; j < t.Length; j++)
                {
                    Console.WriteLine("\t" + t[j].ToString());
                    //return arr[i][j];
                }
            }
        }
    }
}
