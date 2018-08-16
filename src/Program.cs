using System;

/**
 * `dotnet run`             : Run cache-miss by data size tests
 * `dotnet run -- order`    : Run cache-miss by processing order tests
 * `dotnet run -- align`    : Run data alignment test
 * `dotnet run -- virtual`  : Run virtual function call test
 * `dotnet run -- algorithm`: Run algorithm test
 */
namespace MyECS {
    public class Program {
        public static void Main (string[] arguments) {
            if (arguments.Length > 0) {
                if (arguments[0] == "align") {
                    TestDataAlignment.Execute();
                    return;
                } else if (arguments[0] == "virtual") {
                    TestVirtualFunctions.Execute();
                    return;
                } else if (arguments[0] == "algorithm") {
                    TestGoodEnoughAlgorithms.Execute();
                    return;
                } else if (arguments[0] == "order") {
                    TestCacheMissByOrder.Execute();
                    return;
                }
            }

            TestCacheMissByDataSize.Execute();
        }
    }
}
