using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017
{
    public class Day18
    {
        public long Part1(string input)
        {
            var registers = new Dictionary<string, long>();
            var instructions = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            long lastSound = 0;
            for (long i = 0; i < instructions.Length && i >= 0; i++)
            {
                var inst = instructions[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (!Int64.TryParse(inst[1], out long result) && !registers.ContainsKey(inst[1]))
                    registers.Add(inst[1], 0);
                switch (inst[0])
                {
                    case "snd":
                        lastSound = GetParameter(inst[1], registers);
                        break;
                    case "set":
                        registers[inst[1]] = GetParameter(inst[2], registers);
                        break;
                    case "add":
                        registers[inst[1]] += GetParameter(inst[2], registers);
                        break;
                    case "mul":
                        registers[inst[1]] *= GetParameter(inst[2], registers);
                        break;
                    case "mod":
                        registers[inst[1]] %= GetParameter(inst[2], registers);
                        break;
                    case "rcv":
                        if (GetParameter(inst[1], registers) != 0)
                            return lastSound;
                        break;
                    case "jgz":
                        if (GetParameter(inst[1], registers) < 1)
                            break;
                        i += GetParameter(inst[2], registers) - 1;
                        break;
                    default:
                        throw new ArgumentException(String.Format("Unknown instruction {0}", instructions[i]));
                }
            }

            throw new ArgumentException("No sound found.");
        }

        public long Part2(string input)
        {
            var instructions = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var threads = new Dictionary<int, MyThread>
            {
                { 0, new MyThread(0) },
                { 1, new MyThread(1) }
            };
            var curThread = 0;
            while (true)
            {
                threads[curThread].Waiting = false;
                for (; threads[curThread].InstructionPosition < instructions.Length && threads[curThread].InstructionPosition >= 0 && !threads[curThread].Waiting; threads[curThread].InstructionPosition++)
                {
                    var inst = instructions[threads[curThread].InstructionPosition].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (!Int64.TryParse(inst[1], out long result) && !threads[curThread].Registers.ContainsKey(inst[1]))
                        threads[curThread].Registers.Add(inst[1], 0);
                    switch (inst[0])
                    {
                        case "snd":
                            threads[curThread].AddMessage(GetParameter(inst[1], threads[curThread].Registers));
                            break;
                        case "set":
                            threads[curThread].Registers[inst[1]] = GetParameter(inst[2], threads[curThread].Registers);
                            break;
                        case "add":
                            threads[curThread].Registers[inst[1]] += GetParameter(inst[2], threads[curThread].Registers);
                            break;
                        case "mul":
                            threads[curThread].Registers[inst[1]] *= GetParameter(inst[2], threads[curThread].Registers);
                            break;
                        case "mod":
                            threads[curThread].Registers[inst[1]] %= GetParameter(inst[2], threads[curThread].Registers);
                            break;
                        case "rcv":
                            try
                            {
                                threads[curThread].Registers[inst[1]] = threads[curThread == 0 ? 1 : 0].GetNextMessage();
                            }
                            catch (InvalidOperationException ioe)
                            {
                                threads[curThread].Waiting = true;
                                threads[curThread].InstructionPosition--;
                            }
                            break;
                        case "jgz":
                            if (GetParameter(inst[1], threads[curThread].Registers) < 1)
                                break;
                            threads[curThread].InstructionPosition += GetParameter(inst[2], threads[curThread].Registers) - 1;
                            break;
                        default:
                            throw new ArgumentException(String.Format("Unknown instruction {0}", instructions[threads[curThread].InstructionPosition]));
                    }
                }
                if (threads.All(x => x.Value.InstructionPosition == instructions.Length) || threads.All(x => x.Value.Waiting && x.Value.MessageSize == 0))
                    break;
                curThread = curThread == 0 ? 1 : 0;
            }
            return threads[1].MessageCounter;
        }

        public static long GetParameter(string param, Dictionary<string, long> registers)
        {
            if (Int64.TryParse(param, out long result))
                return result;
            else if (registers.TryGetValue(param, out result))
                return result;

            registers.Add(param, 0);
            return registers[param];
        }

        public static void Run()
        {
            var day18 = new Day18();
            Console.WriteLine("\n###############\n###############\nDay 18\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day18.Part1(@"set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2"));
            Console.WriteLine(day18.Part1(Input));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day18.Part2(@"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d"));
            Console.WriteLine(day18.Part2(Input));
        }

        private static string Input
        {
            get
            {
                return @"set i 31
set a 1
mul p 17
jgz p p
mul a 2
add i -1
jgz i -2
add a -1
set i 127
set p 826
mul p 8505
mod p a
mul p 129749
add p 12345
mod p a
set b p
mod b 10000
snd b
add i -1
jgz i -9
jgz a 3
rcv b
jgz b -1
set f 0
set i 126
rcv a
rcv b
set p a
mul p -1
add p b
jgz p 4
snd a
set a b
jgz 1 3
snd b
set f 1
add i -1
jgz i -11
snd a
jgz f -16
jgz a -19";
            }
        }
    }

    public class MyThread
    {
        public int ProgramId { get; private set; }
        public readonly Dictionary<string, long> Registers = new Dictionary<string, long>();
        private Queue<long> Messages { get; set; }
        public long MessageCounter { get; private set; }
        public bool Waiting { get; set; }
        public long InstructionPosition { get; set; }

        public MyThread(int programId)
        {
            ProgramId = programId;
            Messages = new Queue<long>();
            Registers.Add("p", programId);
        }

        public void AddMessage(long value)
        {
            Messages.Enqueue(value);
            MessageCounter++;
        }

        public long GetNextMessage()
        {
            return Messages.Dequeue();
        }

        public int MessageSize
        {
            get
            {
                return Messages.Count;
            }
        }
    }
}
