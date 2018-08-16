using System;
using System.Diagnostics; // to use Stopwatch
using System.Runtime.InteropServices; // to use SizeOf

namespace MyECS {
    /**
     * Conclusion: 
     *      + Utilize loaded data to produce more cache-hits
     */
    public class TestCacheMissByOrder {
        private static Stopwatch stopwatch;
        private static Random randomizer;
        private const int COLUMNS_COUNT = 1000;
        private const int ROWS_COUNT    = 1000;
        private static int CELLS_COUNT  = ROWS_COUNT * COLUMNS_COUNT;
        //----------------------------------------------------------------------
        public static void Execute () {
            Console.WriteLine("Begin.");
            stopwatch  = new Stopwatch();
            randomizer = new Random();

            Console.WriteLine("2D array, row > column:  " + Profile_2DArray_ByRowThenColumn() + "ms.");
            Console.WriteLine("2D array, column > row:  " + Profile_2DArray_ByColumnThenRow() + "ms.");
            Console.WriteLine("1D array:                " + Profile_1DArray() + "ms.");

            Console.WriteLine("End.");
        }
        //----------------------------------------------------------------------
        private static long Profile_2DArray_ByRowThenColumn () {
            int[][] table = CreateTable();
            int minValue = int.MaxValue;

            stopwatch.Restart();
            for (int r = 0; r < ROWS_COUNT; ++r) {
                for (int c = 0; c < COLUMNS_COUNT; ++c) {
                    minValue = Math.Min(minValue, table[r][c]);
                }
            }
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long Profile_2DArray_ByColumnThenRow () {
            int[][] table = CreateTable();
            int minValue = int.MaxValue;
            
            stopwatch.Restart();
            for (int c = 0; c < COLUMNS_COUNT; ++c) {
                for (int r = 0; r < ROWS_COUNT; ++r) {
                    minValue = Math.Min(minValue, table[r][c]);
                }
            }
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long Profile_1DArray () {
            int[] flatTable = CreateFlatTable();
            int minValue = int.MaxValue;
            
            stopwatch.Restart();
            for (int i = 0; i < CELLS_COUNT; ++i) {
                minValue = Math.Min(minValue, flatTable[i]);
            }
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static int[][] CreateTable () {
            int[][] table = new int[ROWS_COUNT][];
            for (int r = 0; r < ROWS_COUNT; ++r) {
                table[r] = new int[COLUMNS_COUNT];
                for (int c = 0; c < COLUMNS_COUNT; ++c) {
                    table[r][c] = randomizer.Next();
                }
            }
            return table;
        }
        //----------------------------------------------------------------------
        private static int[] CreateFlatTable () {
            int[] flatTable = new int[CELLS_COUNT];
            for (int i = 0; i < CELLS_COUNT; ++i) {
                flatTable[i] = randomizer.Next();
            }
            return flatTable;
        }
    }
}
