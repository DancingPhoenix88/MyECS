using System;
using System.Diagnostics; // to use Stopwatch
using System.Runtime.InteropServices; // to use SizeOf

namespace MyECS {
    /**
     * Conclusion:
     *      + Data size matters, because it produces cache-misses
     *      + Take a look at data-alignment
     */
    public class TestCacheMissByDataSize {
        private static Stopwatch stopwatch;
        private static Random randomizer;
        private const int MIN_RANDOM_VALUE = 0;
        private const int MAX_RANDOM_VALUE = 10;
        private const int ELEMENTS_COUNT   = 10_000_000;
        //----------------------------------------------------------------------
        public static void Execute () {
            Console.WriteLine("Begin.");
            stopwatch  = new Stopwatch();
            randomizer = new Random();
            
            Console.WriteLine(string.Format("Processing array of int          (x{0}):   {1}ms.", 
                Marshal.SizeOf<int>(),
                ProfileArrayOfInts()
            ));
            Console.WriteLine(string.Format("Processing array of POT structs  (x{0}):  {1}ms.", 
                Marshal.SizeOf<MyPotStruct>(),
                ProfileArrayOfPotStructs()
            ));
            Console.WriteLine(string.Format("Processing array of NPOT structs (x{0}):  {1}ms.", 
                Marshal.SizeOf<MyNpotStruct>(),
                ProfileArrayOfNpotStructs()
            ));

            Console.WriteLine("End.");
        }
        //----------------------------------------------------------------------
        private static long ProfileArrayOfInts () {
            // Arrange
            int[] objectGroups = new int[ELEMENTS_COUNT]; // 4 bytes each
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                objectGroups[i] = randomizer.Next(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE);
            }
            int count = 0;

            // Act: Count objects in group 0
            stopwatch.Restart();
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                if (objectGroups[i] == 0) {
                    ++count;
                }
            }
            stopwatch.Stop();

            // Assert
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long ProfileArrayOfPotStructs () {
            // Arrange
            MyPotStruct[] objects = new MyPotStruct[ELEMENTS_COUNT];
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                objects[i].groupId = randomizer.Next(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE);
            }
            int count = 0;

            // Act: Count objects in group 0
            stopwatch.Restart();
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                if (objects[i].groupId == 0) {
                    ++count;
                }
            }
            stopwatch.Stop();

            // Assert
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long ProfileArrayOfNpotStructs () {
            // Arrange
            MyNpotStruct[] objects = new MyNpotStruct[ELEMENTS_COUNT];
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                objects[i].groupId = randomizer.Next(MIN_RANDOM_VALUE, MAX_RANDOM_VALUE);
            }
            int count = 0;

            // Act: Count objects in group 0
            stopwatch.Restart();
            for (int i = 0; i < ELEMENTS_COUNT; ++i) {
                if (objects[i].groupId == 0) {
                    ++count;
                }
            }
            stopwatch.Stop();

            // Assert
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        #pragma warning disable CS0169, CS0649 // about unused variables
        private struct MyVector3 { // 4 bytes x 3 = 12 bytes
            float x;
            float y;
            float z;
        }
        //----------------------------------------------------------------------
        private struct MyPotStruct { // 32 bytes
            public int id;
            public MyVector3 position;
            public MyVector3 velocity;
            public int groupId;
        }
        //----------------------------------------------------------------------
        private struct MyNpotStruct { // 33 bytes (-> 36 bytes in runtime)
            public int id;
            public MyVector3 position;
            public MyVector3 velocity;
            public int groupId;
            public byte tag;
            // public byte tag2;    // un-comment this line -> still 36 bytes
                                    // but move it to top ? 40 bytes
        } // 1 bool =  4 bytes, not 1 bit
        #pragma warning restore CS0169, CS0649
    }
}
