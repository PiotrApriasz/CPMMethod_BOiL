using System.Collections.Generic;
using System.Linq;
using CPMMethod.Logic;
using Xunit;

using System;

namespace CPMMethod.Tests;

public class CPMLogicTest
{
    [Fact]
    public void CPMLogic_Test()
    {
        Activity A = new(); A.Id = 1; A.Duration =   5; A.Preccessors = new List<Activity> {         };
        Activity B = new(); B.Id = 2; B.Duration =   1; B.Preccessors = new List<Activity> { A       };
        Activity C = new(); C.Id = 3; C.Duration =   5; C.Preccessors = new List<Activity> { B       };
        Activity E = new(); E.Id = 5; E.Duration =   4; E.Preccessors = new List<Activity> { B       };
        Activity F = new(); F.Id = 6; F.Duration =   3; F.Preccessors = new List<Activity> { B       };
        Activity G = new(); G.Id = 7; G.Duration =   2; G.Preccessors = new List<Activity> { C       };
        Activity D = new(); D.Id = 4; D.Duration =   3; D.Preccessors = new List<Activity> { C       };
        Activity H = new(); H.Id = 8; H.Duration =   1; H.Preccessors = new List<Activity> { D, E, F };
        Activity I = new(); I.Id = 9; I.Duration =   2; I.Preccessors = new List<Activity> { G, H    };

        var activities = new List<Activity>{G, E, F, I, H, A, B, C, D};

        activities.InitActivitiesFields();

        /*Assert.True(activities?.Find(actv => actv.Id == "A")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "B")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "C")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "D")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "H")?.Critical);
        Assert.True(activities?.Find(actv => actv.Id == "I")?.Critical);*/
    }
}