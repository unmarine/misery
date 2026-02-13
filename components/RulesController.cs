using misery.eng;
using misery.Eng;

namespace misery.components;
public class RulesController
{

    // disgusting abomination.
    RuleSet ruleSet;
    NumericUpDown starting, counted, resulting, lower, upper;
    CheckBox isUnconditional;
    Button addRuleButton;
    ListBox listOfRules;

    public RulesController(RuleSet ruleSet, NumericUpDown starting, NumericUpDown counted, NumericUpDown resulting, NumericUpDown lower, NumericUpDown upper, CheckBox isUnconditional, Button addRuleButton, ListBox listOfRules)
    {
        this.ruleSet = ruleSet;
        this.starting = starting;
        this.counted = counted;
        this.resulting = resulting;
        this.lower = lower;
        this.upper = upper;
        this.isUnconditional = isUnconditional;
        this.addRuleButton = addRuleButton;
        this.listOfRules = listOfRules;

        this.addRuleButton.Click += addRule;
        this.addRuleButton.Text = @"Add";
    }

    private void addRule(object? sender, EventArgs e)
    {
        Condition condition;

        int startingState = (int)starting.Value;
        int countedState = (int)counted.Value;
        int resultingState = (int)resulting.Value;
        int lowerNumber = (int)lower.Value;
        int upperNumber = (int)upper.Value;
        bool isUnconditionalValue = isUnconditional.Checked;

        condition = new Condition(startingState, countedState, resultingState, lowerNumber, upperNumber, isUnconditionalValue);
        this.ruleSet.AddCondition(condition);
        listOfRules.Invalidate();
        ReloadList();
    }

    private void ReloadList()
    {
        listOfRules.Items.Clear();
        List<Condition> conditions = ruleSet.GetConditions();
        foreach (Condition condition in conditions)
        {
            listOfRules.Items.Add(condition.ToString());
        }
    }
}

