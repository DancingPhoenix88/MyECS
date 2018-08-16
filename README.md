# MyECS

This is where I put some tests (mainly about CPU Cache) to explain why I should move to Data-Oriented Design, especially Entity-Component-System.

There's a presentation slide also, but I will add it later.

`dotnet run`             : Run cache-miss by data size tests
`dotnet run -- order`    : Run cache-miss by processing order tests
`dotnet run -- align`    : Run data alignment test
`dotnet run -- virtual`  : Run virtual function call test
`dotnet run -- algorithm`: Run algorithm test