using misery.eng;

namespace misery.components.combiners;

public class RulesControls
{
        private readonly Button _addRuleButton;
        private readonly NumericUpDown _counted;
        private readonly CheckBox _isUnconditional;
        private readonly ListBox _listOfRules;
        private readonly NumericUpDown _lower;
        private readonly NumericUpDown _resulting;
        private readonly RuleSet? _ruleSet;
        private readonly NumericUpDown _starting;
        private readonly NumericUpDown _upper;

        public RulesControls(RuleSet? ruleSet, NumericUpDown starting, NumericUpDown counted, NumericUpDown resulting,
                NumericUpDown lower, NumericUpDown upper, CheckBox isUnconditional, Button addRuleButton,
                ListBox listOfRules)
        {
                _ruleSet = ruleSet;
                _starting = starting;
                _counted = counted;
                _resulting = resulting;
                _lower = lower;
                _upper = upper;
                _isUnconditional = isUnconditional;
                _addRuleButton = addRuleButton;
                _listOfRules = listOfRules;

                _addRuleButton.Click += AddRule;
                _addRuleButton.Text = @"Add";
        }

        private void AddRule(object? sender, EventArgs e)
        {
                var startingState = (int)_starting.Value;
                var countedState = (int)_counted.Value;
                var resultingState = (int)_resulting.Value;
                var lowerNumber = (int)_lower.Value;
                var upperNumber = (int)_upper.Value;
                var isUnconditionalValue = _isUnconditional.Checked;

                var condition = new Condition(startingState, countedState, resultingState, lowerNumber, upperNumber,
                        isUnconditionalValue);

                if (_ruleSet != null)
                        _ruleSet.AddCondition(condition);
                _listOfRules.Invalidate();
                ReloadList();
        }

        private void ReloadList()
        {
                _listOfRules.Items.Clear();
                if (_ruleSet == null) return;
                var conditions = _ruleSet.GetConditions();
                foreach (var condition in conditions) _listOfRules.Items.Add(condition);
        }
}