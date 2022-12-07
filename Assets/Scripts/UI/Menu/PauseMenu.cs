using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the pause menu
/// </summary>
public class PauseMenu : MenuActions
{
    [SerializeField] GameObject menu;
    [SerializeField] Signal allPlayersDeadSignal;

    static int pauses;

    InputAction escapeAction;

    private void Awake()
    {
        escapeAction = new InputAction(binding: "<Keyboard>/escape");
        menu.SetActive(false);
    }

    /// <summary>
    /// Input System friendly callback
    /// </summary>
    /// <param name="ctx"></param>
    void TogglePauseMenu(InputAction.CallbackContext ctx) => TogglePauseMenu();

    /// <summary>
    /// Swaps the state of the pause menu, pauses the game, and releases the cursor.
    /// </summary>
    public void TogglePauseMenu()
    {
        menu.SetActive(!menu.activeSelf);

        if (menu.activeSelf) pauses++;
        else pauses--;

        OpenMenu(menuContainer.GetChild(0).gameObject);

        UpdatePauseState();
    }

    private void OnEnable()
    {
        escapeAction.started += TogglePauseMenu;

        escapeAction.Enable();

        allPlayersDeadSignal.OnRaise += Disable;
    }

    private void OnDisable()
    {
        if (menu.activeSelf) TogglePauseMenu();

        escapeAction.started -= TogglePauseMenu;

        escapeAction.Disable();

        allPlayersDeadSignal.OnRaise -= Disable;
    }

    private void OnDestroy()
    {
        escapeAction.started -= TogglePauseMenu;

        allPlayersDeadSignal.OnRaise -= Disable;
    }

    /// <summary>
    /// Callback used to stop the pause screen from showing post game end.
    /// </summary>
    private void Disable ()
    {
        enabled = false;
    }

    private void UpdatePauseState()
    {
        Time.timeScale = pauses <= 0 ? 1.0f : 0.0f;
        Cursor.visible = pauses > 0;
    }
}
