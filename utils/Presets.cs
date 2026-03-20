using misery.eng;

namespace misery.utils;

public static class Presets
{

    public static RuleSet GameOfLife()
    {
        RuleSet gameOfLife = new RuleSet("Game of Life");
        gameOfLife.LifeLike("B3/S23");
        return gameOfLife;
    }

    public static RuleSet BriansBrain()
    {
        var first = new Condition(0, 1, 1, 2, 2);
        var second = new Condition(1, -1, 2, -1, -1, true);
        var third = new Condition(2, -1, 0, -1, -1, true);
        var briansBrain = new RuleSet("Brian's Brain");
        briansBrain.AddCondition(first);
        briansBrain.AddCondition(second);
        briansBrain.AddCondition(third);
        return briansBrain;
    }

    public static RuleSet DayAndNight()
    {
        var dayAndNight = new RuleSet("Day and Night");
        dayAndNight.LifeLike("B3678/S34678");
        return dayAndNight;
    }

    public static RuleSet Diamoeba()
    {
        var diamoeba = new RuleSet("Diamoeba");
        diamoeba.LifeLike("B35678/S5678");
        return diamoeba;
    }

    public static RuleSet TwoXTwo()
    {
        var twoXTwo = new RuleSet("2x2");
        twoXTwo.LifeLike("B36/S125");
        return twoXTwo;
    }

    public static RuleSet Anneal()
    {
        var anneal = new RuleSet("Anneal");
        anneal.LifeLike("B4678/S35678");
        return anneal;
    }

    public static HashSet<RuleSet> All = new() { GameOfLife(), BriansBrain(), DayAndNight(), Diamoeba(), Anneal(), TwoXTwo() };
}