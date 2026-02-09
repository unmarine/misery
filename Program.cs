using misery.eng;
using misery.Eng;
using misery.windows;

namespace misery;

internal static class Program
{
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
        //Application.Run(new Form1());

        /* 
         * Abomination to get rid of later
         */

        Eng.INeighborhood neighborhood = new Moore();
        var dead = new State(0);
        var live = new State(1);
        var dying = new State(2);
        var none = new State(-1);

        var first = new Condition(dead, live, live, 2, 2);
        var second = new Condition(live, none, dying, -1, -1, true);
        var third = new Condition(dying, none, dead, -1, -1, true);

        var BriansBrain = new RuleSet();
        BriansBrain.AddCondition(first);
        BriansBrain.AddCondition(second);
        BriansBrain.AddCondition(third);

        first = new Condition(live, live, dead, 0, 1);
        second = new Condition(live, live, live, 2, 3);
        third = new Condition(live, live, dead, 4, 8);
        var fourth = new Condition(dead, live, live, 3, 3);

        RuleSet GameOfLife = new RuleSet();
        GameOfLife.AddCondition(first);
        GameOfLife.AddCondition(second);
        GameOfLife.AddCondition(third);
        GameOfLife.AddCondition(fourth);

        var automaton = new Automaton(neighborhood, 400, 400, BriansBrain);


        Application.Run(new Display(automaton));
        }
}