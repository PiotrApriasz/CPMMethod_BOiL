using System.Collections.Generic;
using System.Linq;
using CPMMethod.Logic;
using Xunit;

namespace CPMMethod.Tests;

public class CPMLogicTest
{
    [Fact]
    public void CalculateEarly_ValidActivities_AddingCorrectTimesToActivities()
    {
        var activities = new List<Activity>()
        {
            new()
            {
                Id = "A",
                Duration = 2,
                PreActivities = "-"
            },
            new()
            {
                Id = "B",
                Duration = 5,
                PreActivities = "A"
            },
            new()
            {
                Id = "C",
                Duration = 1,
                PreActivities = "A"
            },
            new()
            {
                Id = "D",
                Duration = 4,
                PreActivities = "B,C"
            },
        };

        activities.CalculateEarly();
        
        Assert.Equal(0 /*Jakaś liczba która ma wyjść */, activities.ElementAt(0).EarlyStart);
        //itd
    }
}