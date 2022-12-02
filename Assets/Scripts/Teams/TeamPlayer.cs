using UnityEngine;

[DisallowMultipleComponent]
public class TeamPlayer : MonoBehaviour
{
    public Team team;

    private void OnEnable()
    {
        team.Register(this);
    }

    private void OnDisable()
    {
        team.Deregister(this);
    }
}
