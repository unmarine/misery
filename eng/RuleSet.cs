using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using misery.Eng;

namespace misery.eng;
public class RuleSet
{
    public bool Wrap = true;

    private readonly Dictionary<State, List<Condition>> ConditionsForState = [];

    public RuleSet() { 
        
    }

    public void AddCondition(Condition condition)
    {
        if (!ConditionsForState.ContainsKey((condition.Starting)))
        {
            ConditionsForState.Add(condition.Starting, new List<Condition>());
        }
        ConditionsForState[condition.Starting].Add(condition);
    }

    public void AddCondition(State starting, State counted, State resulting, int amount)
    {
        AddCondition(new Condition(starting, counted, resulting, amount, amount));
    }


    public void RemoveCondition(Condition condition) =>
        ConditionsForState[condition.Starting].Remove(condition);

    public void AddConditionRangedInclusive(State starting, State counted, State resulting, int start,
        int end)
    {
        int up = Math.Max(start, end);
        int down = Math.Min(start, end);
        AddCondition(new Condition(starting, counted, resulting, down, up));
    }

    public void AddConditionRangedInclusive(int starting, int counted, int resulting, int start, int end)
    {
        AddConditionRangedInclusive(new State(starting), new State(counted), new State(resulting), start, end);
    }

    public List<Condition> GetConditionsForState(State state)
    {
        return ConditionsForState[state];
    }

    public List<Condition> GetConditionsForState(int state)
    {
        return ConditionsForState[new State(state)];
    }
}
