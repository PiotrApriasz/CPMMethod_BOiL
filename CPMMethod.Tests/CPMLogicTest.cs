using System.Collections.Generic;
using System.Linq;
using CPMMethod.Logic;
using Xunit;

using System;

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

        activities.MoveForward();
        
        Assert.Equal(0 /*Jakaś liczba która ma wyjść */, activities.ElementAt(0).EarlyStart);
        //itd
    }

    [Fact]
    public void CPMLogic_Test()
    {
        Activity A = new(); A.Id = "A"; A.Duration = 0.5; A.Preccessors = new List<Activity> {         };
        Activity B = new(); B.Id = "B"; B.Duration =   1; B.Preccessors = new List<Activity> { A       };
        Activity C = new(); C.Id = "C"; C.Duration =   5; C.Preccessors = new List<Activity> { B       };
        Activity E = new(); E.Id = "E"; E.Duration =   4; E.Preccessors = new List<Activity> { B       };
        Activity F = new(); F.Id = "F"; F.Duration =   3; F.Preccessors = new List<Activity> { B       };
        Activity G = new(); G.Id = "G"; G.Duration = 0.5; G.Preccessors = new List<Activity> { C       };
        Activity D = new(); D.Id = "D"; D.Duration =   3; D.Preccessors = new List<Activity> { C       };
        Activity H = new(); H.Id = "H"; H.Duration = 0.5; H.Preccessors = new List<Activity> { D, E, F };
        Activity I = new(); I.Id = "I"; I.Duration = 0.5; I.Preccessors = new List<Activity> { G, H    };

        var activities = new List<Activity>{G, E, F, I, H, A, B, C, D};

        activities.InitActivitiesFields();

        Assert.True(activities?.Find(actv => actv.Id == "A")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "B")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "C")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "D")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "H")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "I")?.Critical);
    }
}