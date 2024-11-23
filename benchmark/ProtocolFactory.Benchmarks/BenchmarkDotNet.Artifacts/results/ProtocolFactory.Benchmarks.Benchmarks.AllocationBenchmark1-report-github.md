```

BenchmarkDotNet v0.14.0, Linux Mint 22 (Wilma)
AMD Ryzen 5 4500U with Radeon Graphics, 1 CPU, 6 logical and 6 physical cores
.NET SDK 8.0.110
  [Host]     : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2


```
| Method | Mean     | Error     | StdDev    | Gen0   | Allocated |
|------- |---------:|----------:|----------:|-------:|----------:|
| First  | 6.654 μs | 0.1320 μs | 0.1807 μs | 0.6943 |   1.43 KB |
