using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Team")]
public class Team : ScriptableObject
{
    public string id;
    public Signal memberDeadSignal;
    public Signal teamWipeSignal;

    public HashSet<TeamPlayer> players = new HashSet<TeamPlayer>();

    static Dictionary<string, Team> registeredTeams = new Dictionary<string, Team>();

    public static Team GetTeamByID (string id)
    {
        return registeredTeams[id];
    }

    private void Awake()
    {
        if (!registeredTeams.ContainsKey(id))
        {
            registeredTeams.Add(id, this);
        }
        else
        {
            Debug.LogError("Team trying to register after already being registered", this);
        }
    }

    private void OnDestroy()
    {
        if (registeredTeams.ContainsKey(id))
        {
            registeredTeams.Remove(id);
        }
        else
        {
            Debug.LogError("Team has been destroyed but was never registered", this);
        }
    }

    public void Register (TeamPlayer player)
    {
        players.Add(player);
    }

    public void Deregister (TeamPlayer player)
    {
        players.Remove(player);
        if (memberDeadSignal) memberDeadSignal.Raise();

        if (players.Count == 0 && teamWipeSignal) teamWipeSignal.Raise();
    }

    public static HashSet<TeamPlayer> GetOverlapingSet (params Team[] teams)
    {
        HashSet<TeamPlayer> superSet = new HashSet<TeamPlayer>();
        foreach (Team team in teams)
        {
            superSet.UnionWith(team.players);
        }

        return superSet;
    }

    public static bool IsInOne (TeamPlayer player, params Team[] teams)
    {
        foreach (Team team in teams)
        {
            if (team.players.Contains(player))
            {
                return true;
            }
        }
        return false;
    }
}
