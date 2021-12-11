using System.Diagnostics;

Console.WriteLine($"Start time: {DateTimeOffset.Now:O}");
var day = new AdventOfCode2021.Day2.Main { Debug = true };
var stopwatch = new Stopwatch();

stopwatch.Start();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"******************************  {day.DayName}  ******************************");
Console.WriteLine("\n****************************** Part 1 ******************************");
Console.ResetColor();
Console.WriteLine(await day.Part1());
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Part 1 took: {stopwatch.Elapsed}");

stopwatch.Restart();
Console.WriteLine("\n****************************** Part 2 ******************************");
Console.ResetColor();
Console.WriteLine(await day.Part2());
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Part 2 took: {stopwatch.Elapsed}");
Console.WriteLine($"End time: {DateTimeOffset.Now:O}");
Console.ResetColor();