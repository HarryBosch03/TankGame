using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Used to animate and move an object as the cursor.
/// </summary>
public class TankCursor : MonoBehaviour
{
    [SerializeField] CursorProfile defaultUI;
    [SerializeField] CursorProfile defaultGame;
    [SerializeField] CursorProfile emptyGame;

    [Space]
    [SerializeField] GameObject mainCannonObject;

    IAttack mainCannon;

    private void OnEnable()
    {
        mainCannon = mainCannonObject.GetComponent<IAttack>();

        mainCannon.AttackEvent += UpdateCursor;
        mainCannon.CooldownFinishedEvent += UpdateCursor;
        PauseMenu.PauseChangedEvent += UpdateCursor;

        defaultGame.Apply();
    }

    private void UpdateCursor(bool _) => UpdateCursor();
    private void UpdateCursor()
    {
        if (PauseMenu.Paused) defaultUI.Apply();
        else if (mainCannon.Cooldown < 0.999f) emptyGame.Apply();
        else defaultGame.Apply();
    }

    private void OnDisable()
    {
        mainCannon.AttackEvent -= UpdateCursor;
        mainCannon.CooldownFinishedEvent -= UpdateCursor;
        PauseMenu.PauseChangedEvent -= UpdateCursor;

        defaultUI.Apply();
    }

    private void OnDestroy()
    {
        mainCannon.AttackEvent -= UpdateCursor;
        mainCannon.CooldownFinishedEvent -= UpdateCursor;
        PauseMenu.PauseChangedEvent -= UpdateCursor;

        defaultUI.Apply();
    }

    [System.Serializable]
    public struct CursorProfile
    {
        public Texture2D tex;
        public Vector2 hotspot;
        public CursorMode mode;

        public CursorProfile(Texture2D tex, Vector2 hotspot, CursorMode mode)
        {
            this.tex = tex;
            this.hotspot = hotspot;
            this.mode = mode;
        }

        public void Apply ()
        {
            Cursor.SetCursor(tex, hotspot, mode);
        }
    }
}
