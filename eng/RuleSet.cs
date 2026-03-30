using misery.eng.automaton;

namespace misery.eng;

public class RuleSet(string name)
{
        private readonly Dictionary<State, List<Condition>> _conditionsForState = [];

        public void LifeLike(string rule)
        {
                var cursor = 0;

                while (cursor < rule.Length)
                        if (rule[cursor] == 'B')
                        {
                                cursor++;
                                while (cursor < rule.Length && char.IsDigit(rule[cursor]))
                                {
                                        var number = rule[cursor] - '0';
                                        var c = new Condition(0, 1, 1, number, number);
                                        AddCondition(c);
                                        cursor++;
                                }
                        }
                        else if (rule[cursor] == 'S')
                        {
                                cursor++;
                                while (cursor < rule.Length && char.IsDigit(rule[cursor]))
                                {
                                        var number = rule[cursor] - '0';
                                        var c = new Condition(1, 1, 1, number, number);
                                        AddCondition(c);
                                        cursor++;
                                }
                        }
                        else
                        {
                                cursor++;
                        }
        }


        public void AddCondition(Condition condition)
        {
                if (!_conditionsForState.ContainsKey(condition.Starting))
                        _conditionsForState[condition.Starting] = new List<Condition>();

                if (!_conditionsForState[condition.Starting].Contains(condition))
                        _conditionsForState[condition.Starting].Add(condition);
        }

        public void AddCondition(State starting, State counted, State resulting, int amount)
        {
                AddCondition(new Condition(starting, counted, resulting, amount, amount));
        }

        public void RemoveCondition(Condition condition)
        {
                if (_conditionsForState.TryGetValue(condition.Starting, out var conditions))
                        conditions.Remove(condition);
        }

        private void AddConditionRangedInclusive(State starting, State counted, State resulting, int start,
                int end)
        {
                var up = Math.Max(start, end);
                var down = Math.Min(start, end);
                AddCondition(new Condition(starting, counted, resulting, down, up));
        }

        public void AddConditionRangedInclusive(int starting, int counted, int resulting, int start, int end)
        {
                AddConditionRangedInclusive(new State(starting), new State(counted), new State(resulting), start, end);
        }

        public List<Condition> GetConditionsForState(State state)
        {
                return _conditionsForState.TryGetValue(state, out var conditions) ? conditions : new List<Condition>();
        }

        public List<Condition> GetConditionsForState(int value) => GetConditionsForState(new State(value));

        public List<Condition> GetConditions() => _conditionsForState.Values.SelectMany(x => x).ToList();
        public override string ToString() => name;
}