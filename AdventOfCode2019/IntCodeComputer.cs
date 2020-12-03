using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    public class IntCodeComputer
    {
        public Dictionary<long, long> Mem;
        public long Input;
        public long? Output;
        public State State;

        private bool Debug;
        private long InstPtr = 0;
        private long RelBase = 0;
        private long Opcode = 0;
        private IList<long> Params;
        private string Instruction;
        private string ParamModes;        

        public IntCodeComputer(string program, long input = 0, bool debug = false) : this(program.Split(',').Select(x => long.Parse(x)).ToList(), input, debug) { }

        public IntCodeComputer(IList<long> program, long input = 0, bool debug = false)
        {
            Mem = new Dictionary<long, long>(program.Count);
            var index = 0L;
            foreach (var num in program)
                Mem[index++] = num;

            Input = input;
            Debug = debug;
            State = State.Ready;            
        }

        public long? RunToOutput()
        {
            while (RunInstruction())
            {
                if (Output.HasValue)
                    return Output.Value;
            }

            return null;
        }

        public void RunToHalt()
        {
            while (RunInstruction()) ;
        }

        public bool RunInstruction()
        {
            if (State == State.Halt || InstPtr >= Mem.LongCount())
                return false;

            Output = null;
            State = State.Ready;
            SetOpCode();
            IList<long> paramVals;
            switch (Instruction)
            {
                case "01": // add
                    SetParams(3);
                    paramVals = GetParamValues(3);
                    SetParamValue(2, paramVals[0] + paramVals[1]);
                    break;
                case "02": // multiply
                    SetParams(3);
                    paramVals = GetParamValues(3);
                    SetParamValue(2, paramVals[0] * paramVals[1]);
                    break;
                case "03": // input and save
                    SetParams(1);
                    SetParamValue(0, Input);
                    break;
                case "04": // output
                    SetParams(1);
                    paramVals = GetParamValues(1);
                    Output = paramVals[0];
                    State = State.HasOutput;
                    if (Debug)
                        Console.WriteLine(Output);
                    break;
                case "05": // jump-if-true
                    SetParams(2);
                    paramVals = GetParamValues(2);
                    if (paramVals[0] != 0)
                        InstPtr = paramVals[1];
                    break;
                case "06": // jump-if-false
                    SetParams(2);
                    paramVals = GetParamValues(2);
                    if (paramVals[0] == 0)
                        InstPtr = paramVals[1];
                    break;
                case "07": // less than
                    SetParams(3);
                    paramVals = GetParamValues(3);
                    SetParamValue(2, paramVals[0] < paramVals[1] ? 1 : 0);
                    break;
                case "08": // equals
                    SetParams(3);
                    paramVals = GetParamValues(3);
                    SetParamValue(2, paramVals[0] == paramVals[1] ? 1 : 0);
                    break;
                case "09":
                    SetParams(1);
                    paramVals = GetParamValues(1);
                    RelBase += paramVals[0];
                    break;
                case "99": // halt
                    State = State.Halt;
                    break;
                default:
                    throw new ArgumentException($"Invalid opcode: {Mem[InstPtr]}");
            }

            return State != State.Halt;
        }

        private void SetOpCode()
        {
            Opcode = Mem[InstPtr++];

            var instruction = Opcode.ToString();
            if (instruction.Length > 2)
                Instruction = instruction.Substring(instruction.Length - 2);
            else
                Instruction = instruction.PadLeft(2, '0');

            var paramModes = Opcode.ToString();
            if (paramModes.Length > 2)
            {
                paramModes = paramModes.Substring(0, paramModes.Length - 2);
                ParamModes = new string(paramModes.Reverse().ToArray());
            }
            else
                ParamModes = string.Empty;
        }

        private void SetParams(int count)
        {
            ParamModes = ParamModes.PadRight(count, '0');
            Params = new List<long>();
            for (var i = 0; i < count; i++)
            {
                Params.Add(Mem[InstPtr++]);
            }
        }

        private long GetParamValue(int position)
        {
            long value;
            switch (ParamModes[position])
            {
                case '0': // position mode
                    return Mem.TryGetValue(Params[position], out value) ? value : 0L;
                case '1': // immediate mode
                    return Params[position];
                case '2': // relative mode
                    return Mem.TryGetValue(Params[position] + RelBase, out value) ? value : 0L;
                default:
                    throw new Exception($"Unknown parameter mode: {ParamModes[position]}");
            }
        }

        private void SetParamValue(int position, long value)
        {
            switch (ParamModes[position])
            {
                case '0': // position mode
                    Mem[Params[position]] = value;
                    break;
                case '2': // relative mode
                    Mem[Params[position] + RelBase] = value;
                    break;
                default:
                    throw new Exception($"Unknown parameter mode: {ParamModes[position]}");
            }
        }

        private IList<long> GetParamValues(long count)
        {
            var paramValues = new List<long>();
            for (var i = 0; i < count; i++)
                paramValues.Add(GetParamValue(i));

            return paramValues;
        }
    }

    public enum State
    {
        Ready,
        HasOutput,
        Halt
    }
}
