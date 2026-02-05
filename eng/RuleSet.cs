using misery.Eng;

namespace misery.eng;

public class RuleSet
{
        public bool Wrap = true;

        private readonly Dictionary<State, List<Condition>> _conditionsForState = [];

        public void AddCondition(Condition condition)
        {
                if (!_conditionsForState.ContainsKey(condition.Starting))
                        _conditionsForState.Add(condition.Starting, new List<Condition>());
                _conditionsForState[condition.Starting].Add(condition);
        }

        public void AddCondition(State starting, State counted, State resulting, int amount)
        {
                AddCondition(new Condition(starting, counted, resulting, amount, amount));
        }


        public void RemoveCondition(Condition condition)
        {
                _conditionsForState[condition.Starting].Remove(condition);
        }

        public void AddConditionRangedInclusive(State starting, State counted, State resulting, int start,
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
                return _conditionsForState[state];
        }

        public List<Condition> GetConditionsForState(int state)
        {
                return _conditionsForState[new State(state)];
        }
}