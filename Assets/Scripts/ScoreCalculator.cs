using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreCalculator
{
    /// <summary>
    /// Awards points for an enemy Kill.
    /// </summary>
    /// <param name="hitObject"></param>
    public static void AwardKillPoints(Health hitObject)
    {
        if (hitObject)
        {
            int value = (int)hitObject.MaxHealth;
            Stats.Main.score.Value += value;
            ScoreCounter.Display($"Kill\t+{value}");
        }
    }

    /// <summary>
    /// Awards points gained through developer actions.
    /// </summary>
    /// <param name="value"></param>
    public static void AwardCheatPoints(int value)
    {
        Stats.Main.score.Value += value;
        ScoreCounter.Display($"Cheated\t+{value}");
    }
}
