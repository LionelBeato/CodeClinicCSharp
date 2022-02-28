using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace tests;

// write a function that accepts a beginning date and time, an ending date and time, 
// return the coefficient of the slope of the barometric pressure

public class PondOreille
{
    private string text;
    private Regex rx;
    private List<(DateTime, String)> tuples; 

    public double CoefficientCal(DateTime begin, DateTime end)
    {
        var entryOne = tuples.Find(t => t.Item1.Equals(begin));
        var entryTwo = tuples.Find(t => t.Item1.Equals(end));

        var num = float.Parse(entryTwo.Item2) - float.Parse(entryOne.Item2); 
        var denom = entryTwo.Item1 - entryOne.Item1;
        
        // Console.WriteLine($"Our values are {entryTwo}, {num} and {denom.TotalMinutes}");
        

        return num / denom.TotalHours; 
    }

    public float SimpleCoCalc(List<int> point1, List<int> point2)
    {
        return point2[1] - point1[1] / point2[0] - point1[0];
    }
    
    
    [SetUp]
    public void Setup()
    {
         text = System.IO.File.ReadAllText("/Users/lionel.beato/Projects/dotnet/CodeClinic/tests/pond.txt");
         rx = new Regex(@"([0-9]*_[0-9]*_[0-9]*\s[0-9]*:[0-9]*:[0-9]*)\s([0-9][0-9].?[0-9]?)\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    }

    [Test]
    public void ReadAndOutPut()
    {
        Console.WriteLine($"Contents of pond.txt =\n {text}");
        Assert.Pass();
    }

    [Test]
    public void SimpleCalc()
    {
        var result = SimpleCoCalc(new List<int> {0, 0}, new List<int> {1, 2});
        Console.WriteLine($"{result} is the Coefficient!");
    }

    [Test]
    public void TestDate()
    {
        DateTime dateTime = DateTime.Now;
        Console.WriteLine($"{dateTime}");

        DateTime parsed = DateTime.Parse("2012/01/01 00:02:14"); 
        Console.WriteLine($"{parsed}");

    }

    [Test]
    public void OutPutOnlyBaro()
    {
        MatchCollection matches = rx.Matches(text);
        Console.WriteLine($"{matches.Count} matches found in text");
        var fullList = matches.ToImmutableList(); 
        var baroList = matches.ToImmutableList().Select(match => match.Groups[3].Value);
        
        baroList.ToList()
            .ForEach(match => Console.WriteLine(match));
    }

    [Test]
    public void NormalizeData()
    {
        MatchCollection matches = rx.Matches(text);
        var fullList = matches.ToImmutableList();
        tuples = fullList.Select(match =>
        {
            var datetime = DateTime.Parse(match.Groups[1].Value.Replace("_", "/"));
            var baro = match.Groups[2].Value;
            (DateTime, string) tuple = (datetime, baro);
            return tuple; 
        }).ToList();
        
        // tuples.ForEach(t => Console.WriteLine(t));

        //2012_12_31 23:57:14
        DateTime dtOne = new DateTime(2012, 01, 01, 00, 02, 14);
        DateTime dtTwo = new DateTime(2012, 12, 31, 23, 57, 14);


        var result = CoefficientCal(dtOne, dtTwo);
        Console.WriteLine($"{result} is the result of our calculations!");
    }

}