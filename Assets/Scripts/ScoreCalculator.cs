using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreCalculator
{
    public static void CalculateScore (Health hitObject)
    {
        if (hitObject)
        {
            int value = (int)hitObject.MaxHealth;
            Stats.Main.score.Value += value;
            ScoreCounter.Display($"Kill\t+{value}");
        }
    }
}
