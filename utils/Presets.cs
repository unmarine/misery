using misery.eng;
using misery.Eng;

namespace misery.utils;

public static class Presets
{
        
        public static RuleSet GameOfLife()
        {
                Condition first = new Condition(1, 1, 0, 0, 1);
                Condition second = new Condition(1, 1, 1, 2, 3);
                Condition third = new Condition(1, 1, 0, 4, 8);
                Condition fourth = new Condition(0, 1, 1, 3, 3);
                RuleSet gameOfLife = new RuleSet("Game of Life");
                gameOfLife.AddCondition(first);
                gameOfLife.AddCondition(third);
                gameOfLife.AddCondition(second);
                gameOfLife.AddCondition(fourth);
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

        public static HashSet<RuleSet> All = new() {GameOfLife(), BriansBrain()};
}