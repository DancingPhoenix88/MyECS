using System;
using System.Runtime.InteropServices; // to use SizeOf

namespace MyECS {
    public class TestDataAlignment {
        public static void Execute () {
            Console.WriteLine("Begin.");

            Console.WriteLine("Un-aligned data:              " + Marshal.SizeOf<MyUnalignedStruct>()    + " bytes");
            Console.WriteLine("Un-aligned data with padding: " + Marshal.SizeOf<MyUnalignedStruct2>()   + " bytes");
            Console.WriteLine("Aligned data:                 " + Marshal.SizeOf<MyAlignedStruct>()      + " bytes");

            Console.WriteLine("End.");
        }
        //----------------------------------------------------------------------
        #pragma warning disable CS0169, CS0649 // about unused variables
        private struct MyUnalignedStruct {
            byte data1;         // 1 byte   -> Block 1 (1/4)
            short data2;        // 2 bytes  -> Block 1 (2/4)
            int data3;          // 4 bytes  -> Block 2 (4/4)
            byte data4;         // 1 byte   -> Block 3 (1/4)
        }
        //----------------------------------------------------------------------
        private struct MyUnalignedStruct2 {
            byte data1;         // 1 byte   -> Block 1 (1/4)
            byte padding11;     // 1 byte   -> Block 1 (2/4)
            short data2;        // 2 bytes  -> Block 1 (4/4)
            int data3;          // 4 bytes  -> Block 2 (4/4)
            byte data4;         // 1 byte   -> Block 3 (1/4)
            byte padding21;     // 1 byte   -> Block 3 (2/4)
            byte padding22;     // 1 byte   -> Block 3 (3/4)
            byte padding23;     // 1 byte   -> Block 3 (4/4)
        }
        //----------------------------------------------------------------------
        private struct MyAlignedStruct {
            int data3;          // 4 bytes  -> Block 1 (4/4)
            short data2;        // 2 bytes  -> Block 2 (2/4)
            byte data1;         // 1 byte   -> Block 2 (3/4)
            byte data4;         // 1 byte   -> Block 2 (4/4)
        }
        #pragma warning restore CS0169, CS0649
    }
}
