namespace AdventOfCode2024.Day2;

internal class Main : BaseClass
{
    public new async Task<string> Part1()
    {
        var input = await ReadInput();
        var safeReports = 0;
        foreach (var line in input)
        {
            var report = line.Split(' ').Select(int.Parse).ToList();
            var isIncreasing = report[1] > report[0];
            if (IsSafe(report))
            {
                safeReports++;
                LogMessage($"Report safe: {line}");
            }
        }
        
        return safeReports.ToString();
    }

    public new async Task<string> Part2()
    {
        //var input = GetSample();
        var input = await ReadInput();
        var safeReports = 0;
        foreach (var line in input)
        {
            var report = line.Split(' ').Select(int.Parse).ToList();
            var isIncreasing = report[1] > report[0];
            var isSafe = IsSafe(report);
            
            // problem dampener
            if (!isSafe)
            {
                for (var i = 0; i < report.Count; i++)
                {
                    var newReport = report.ToList();
                    newReport.RemoveAt(i);
                    isSafe = IsSafe(newReport);
                    if (isSafe)
                    {
                        LogMessage($"Problem dampener fixed report: {string.Join(' ', newReport)}");
                        break;
                    }
                }
            }

            if (isSafe)
            {
                safeReports++;
                LogMessage($"Report safe: {line}");
            }
        }
        
        return safeReports.ToString();
    }

    private bool IsSafe(List<int> report)
    {
        var isIncreasing = report[1] > report[0];
        for (var i = 0; i < report.Count - 1; i++)
        {
            var diff = report[i + 1] - report[i];
            if ((Math.Abs(diff) is < 1 or > 3)
                || (isIncreasing && diff < 0)
                || (!isIncreasing && diff > 0))
            {
                return false;
            }
        }

        return true;
    }
    
    private string[] GetSample()
    {
        var sample = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";
        return sample.Split(Environment.NewLine);
    } 
}