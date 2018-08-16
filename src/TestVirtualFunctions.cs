using System;
using System.Diagnostics; // to use Stopwatch

namespace MyECS {
    public class TestVirtualFunctions {
        private static Stopwatch stopwatch;
        private const int LOOPS_COUNT = 10_000_000;
        //----------------------------------------------------------------------
        public static void Execute () {
            stopwatch = new Stopwatch();
            Console.WriteLine("Begin.");

            Console.WriteLine("Base class A      (0 indirection):   " + ProfileMyBaseClassA() + "ms");
            Console.WriteLine("Derived class A10 (1 indirection):   " + ProfileMyDerivedClassA10() + "ms");
            Console.WriteLine("Base class B      (0 indirection):   " + ProfileMyBaseClassB() + "ms");
            Console.WriteLine("Derived class B10 (10 indirections): " + ProfileMyDerivedClassB10() + "ms");

            Console.WriteLine("End.");
        }
        //----------------------------------------------------------------------
        #region Derived class without overriding functions
        private class MyBaseClassA { public virtual int GetId () { return 0; } }
        private class MyDerivedClassA1 : MyBaseClassA {}
        private class MyDerivedClassA2 : MyDerivedClassA1 {}
        private class MyDerivedClassA3 : MyDerivedClassA2 {}
        private class MyDerivedClassA4 : MyDerivedClassA3 {}
        private class MyDerivedClassA5 : MyDerivedClassA4 {}
        private class MyDerivedClassA6 : MyDerivedClassA5 {}
        private class MyDerivedClassA7 : MyDerivedClassA6 {}
        private class MyDerivedClassA8 : MyDerivedClassA7 {}
        private class MyDerivedClassA9 : MyDerivedClassA8 {}
        private class MyDerivedClassA10 : MyDerivedClassA9 { public override int GetId() { return base.GetId(); } }
        #endregion
        //----------------------------------------------------------------------
        #region Derived class with overriding functions
        private class MyBaseClassB                          { public virtual int GetId () { return 0; } }
        private class MyDerivedClassB1 : MyBaseClassB       { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB2 : MyDerivedClassB1   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB3 : MyDerivedClassB2   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB4 : MyDerivedClassB3   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB5 : MyDerivedClassB4   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB6 : MyDerivedClassB5   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB7 : MyDerivedClassB6   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB8 : MyDerivedClassB7   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB9 : MyDerivedClassB8   { public override int GetId() { return base.GetId(); } }
        private class MyDerivedClassB10 : MyDerivedClassB9  { public override int GetId() { return base.GetId(); } }
        #endregion
        //----------------------------------------------------------------------
        private static long ProfileMyBaseClassA () {
            MyBaseClassA obj = new MyBaseClassA();
            stopwatch.Restart();
            for (int i = 0; i < LOOPS_COUNT; ++i) {
                obj.GetId();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long ProfileMyDerivedClassA10 () {
            MyDerivedClassA10 obj = new MyDerivedClassA10();
            stopwatch.Restart();
            for (int i = 0; i < LOOPS_COUNT; ++i) {
                obj.GetId();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long ProfileMyBaseClassB () {
            MyBaseClassB obj = new MyBaseClassB();
            stopwatch.Restart();
            for (int i = 0; i < LOOPS_COUNT; ++i) {
                obj.GetId();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        //----------------------------------------------------------------------
        private static long ProfileMyDerivedClassB10 () {
            MyDerivedClassB10 obj = new MyDerivedClassB10();
            stopwatch.Restart();
            for (int i = 0; i < LOOPS_COUNT; ++i) {
                obj.GetId();
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
