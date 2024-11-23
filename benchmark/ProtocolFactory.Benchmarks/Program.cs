using BenchmarkDotNet.Running;
using ProtocolFactory.Benchmarks.Benchmarks;

Console.WriteLine("Hello, World!");
BenchmarkRunner.Run<AllocationBenchmark1>();