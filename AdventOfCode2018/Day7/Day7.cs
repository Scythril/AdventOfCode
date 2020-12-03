using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day7
{
    class Day7
    {
        public string Part1(string[] input)
        {
            var completedSteps = new List<string>();
            var steps = GetSteps(input, 0);

            while (true)
            {
                var readySteps = GetReadySteps(steps, completedSteps);
                if (!readySteps.Any())
                {
                    break;
                }

                var stepToRun = readySteps.First();
                completedSteps.Add(stepToRun.Key);
                steps.Remove(stepToRun.Key);
            }

            return string.Join("", completedSteps);
        }

        public int Part2(string[] input, int workerCount, int baseTime)
        {
            var completedSteps = new List<string>();
            var steps = GetSteps(input, baseTime);
            var workers = Enumerable.Range(0, workerCount).Select(x => new Worker(x)).ToDictionary(x => x.Id, x => x);
            var second = 0;
            while (true)
            {
                foreach (var worker in workers.Where(x => x.Value.CurrentStep != null))
                {
                    workers[worker.Key].TimeElapsed++;
                    if (workers[worker.Key].TimeElapsed == workers[worker.Key].CurrentStep.Duration)
                    {
                        completedSteps.Add(workers[worker.Key].CurrentStep.Name);
                        steps.Remove(workers[worker.Key].CurrentStep.Name);
                        workers[worker.Key].CurrentStep = null;
                    }
                }

                if (!steps.Any())
                {
                    break;
                }

                if (workers.Any(x => x.Value.CurrentStep == null))
                {
                    var readySteps = GetReadySteps(steps.Where(x => !x.Value.InProgress), completedSteps).ToList();
                    for (int i = 0; i < readySteps.Count(); i++)
                    {
                        if (!workers.Any(x => x.Value.CurrentStep == null))
                        {
                            break;
                        }

                        var worker = workers.First(x => x.Value.CurrentStep == null);
                        workers[worker.Key].CurrentStep = readySteps[i].Value;
                        workers[worker.Key].TimeElapsed = 0;
                        steps[readySteps[i].Key].InProgress = true;
                    }
                }

                second++;
            }

            return second;
        }

        private int GetLetterValue(string letter)
        {
            return Convert.ToInt32(letter[0]) - 64;
        }

        private IOrderedEnumerable<KeyValuePair<string, Step>> GetReadySteps(IEnumerable<KeyValuePair<string, Step>> availableSteps, IList<string> completedSteps)
        {
            return availableSteps.Where(x => x.Value.Prerequisites.All(y => completedSteps.Contains(y))).OrderBy(x => x.Key);
        }

        private Dictionary<string, Step> GetSteps(string[] input, int baseTime)
        {
            var steps = new Dictionary<string, Step>();
            foreach (var line in input)
            {
                var (step1, step2) = ParseLine(line);
                if (!steps.ContainsKey(step1))
                {
                    steps.Add(step1, new Step(step1, GetLetterValue(step1) + baseTime));
                }

                if (!steps.ContainsKey(step2))
                {
                    steps.Add(step2, new Step(step2, GetLetterValue(step2) + baseTime));
                }

                steps[step2].Prerequisites.Add(step1);
            }

            return steps;
        }

        private (string, string) ParseLine(string line)
        {
            var match = Regex.Match(line, @"Step (\w*) must be finished before step (\w*) can begin\.");
            return (match.Groups[1].Value, match.Groups[2].Value);
        }

        public static void Run()
        {
            var input = GetInput().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var exampleInput = @"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.".Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var Day7 = new Day7();
            Console.WriteLine("\n###############\n###############\nDay7\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(Day7.Part1(exampleInput));
            Console.WriteLine(Day7.Part1(input));
            
            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(Day7.Part2(exampleInput, 2, 0));
            Console.WriteLine(Day7.Part2(input, 5, 60));
        }

        private static string GetInput()
        {
            return @"Step S must be finished before step B can begin.
Step B must be finished before step Y can begin.
Step R must be finished before step E can begin.
Step H must be finished before step M can begin.
Step C must be finished before step F can begin.
Step K must be finished before step A can begin.
Step V must be finished before step W can begin.
Step W must be finished before step L can begin.
Step J must be finished before step L can begin.
Step Q must be finished before step A can begin.
Step U must be finished before step L can begin.
Step Y must be finished before step M can begin.
Step T must be finished before step F can begin.
Step D must be finished before step A can begin.
Step I must be finished before step M can begin.
Step O must be finished before step P can begin.
Step A must be finished before step L can begin.
Step P must be finished before step N can begin.
Step X must be finished before step Z can begin.
Step G must be finished before step N can begin.
Step M must be finished before step F can begin.
Step N must be finished before step L can begin.
Step F must be finished before step Z can begin.
Step Z must be finished before step E can begin.
Step E must be finished before step L can begin.
Step O must be finished before step X can begin.
Step B must be finished before step V can begin.
Step H must be finished before step Q can begin.
Step T must be finished before step M can begin.
Step A must be finished before step G can begin.
Step R must be finished before step H can begin.
Step S must be finished before step C can begin.
Step N must be finished before step Z can begin.
Step Z must be finished before step L can begin.
Step Q must be finished before step Z can begin.
Step R must be finished before step G can begin.
Step P must be finished before step Z can begin.
Step U must be finished before step M can begin.
Step W must be finished before step D can begin.
Step F must be finished before step L can begin.
Step D must be finished before step P can begin.
Step I must be finished before step E can begin.
Step M must be finished before step E can begin.
Step H must be finished before step N can begin.
Step F must be finished before step E can begin.
Step D must be finished before step L can begin.
Step C must be finished before step E can begin.
Step H must be finished before step Z can begin.
Step W must be finished before step Q can begin.
Step X must be finished before step E can begin.
Step G must be finished before step M can begin.
Step X must be finished before step M can begin.
Step Y must be finished before step P can begin.
Step S must be finished before step I can begin.
Step P must be finished before step X can begin.
Step S must be finished before step T can begin.
Step I must be finished before step N can begin.
Step P must be finished before step L can begin.
Step C must be finished before step X can begin.
Step I must be finished before step G can begin.
Step O must be finished before step F can begin.
Step I must be finished before step X can begin.
Step C must be finished before step Z can begin.
Step B must be finished before step K can begin.
Step T must be finished before step P can begin.
Step Q must be finished before step X can begin.
Step M must be finished before step N can begin.
Step H must be finished before step O can begin.
Step Q must be finished before step M can begin.
Step U must be finished before step F can begin.
Step Y must be finished before step O can begin.
Step D must be finished before step O can begin.
Step R must be finished before step T can begin.
Step A must be finished before step E can begin.
Step A must be finished before step M can begin.
Step C must be finished before step N can begin.
Step G must be finished before step E can begin.
Step C must be finished before step Y can begin.
Step A must be finished before step Z can begin.
Step S must be finished before step X can begin.
Step V must be finished before step Z can begin.
Step Q must be finished before step I can begin.
Step P must be finished before step E can begin.
Step D must be finished before step F can begin.
Step M must be finished before step Z can begin.
Step U must be finished before step N can begin.
Step Q must be finished before step L can begin.
Step O must be finished before step Z can begin.
Step N must be finished before step E can begin.
Step S must be finished before step W can begin.
Step S must be finished before step O can begin.
Step U must be finished before step T can begin.
Step A must be finished before step P can begin.
Step J must be finished before step I can begin.
Step A must be finished before step F can begin.
Step U must be finished before step D can begin.
Step W must be finished before step X can begin.
Step O must be finished before step L can begin.
Step J must be finished before step D can begin.
Step R must be finished before step Z can begin.
Step O must be finished before step N can begin.";
        }

        class Step
        {
            public string Name { get; set; }
            public HashSet<string> Prerequisites { get; set; }
            public int Duration { get; set; }
            public bool InProgress { get; set; }

            public Step(string name, int duration)
            {
                Name = name;
                Prerequisites = new HashSet<string>();
                Duration = duration;
            }
        }

        class Worker
        {
            public int Id { get; set; }
            public Step CurrentStep { get; set; }
            public int TimeElapsed { get; set; }

            public Worker(int id)
            {
                Id = id;
            }
        }
    }
}
