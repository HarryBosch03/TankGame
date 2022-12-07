using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MenuActions
{
    [SerializeField] GameObject menu;

    static int pauses;

    InputAction escapeAction;

    private void Awake()
    {
        escapeAction = new InputAction(binding: "<Keyboard>/escape");
        menu.SetActive(false);
    }

    public void TogglePauseMenu(InputAction.CallbackContext ctx) => TogglePauseMenu();
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
    }

    private void OnDisable()
    {
        if (menu.activeSelf) TogglePauseMenu();

        escapeAction.started -= TogglePauseMenu;

        escapeAction.Disable();
    }

    private void OnDestroy()
    {
        escapeAction.started -= TogglePauseMenu;
    }

    private void UpdatePauseState()
    {
        Time.timeScale = pauses <= 0 ? 1.0f : 0.0f;
        Cursor.visible = pauses > 0;
    }
}
