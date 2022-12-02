using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Stats
{
    public Stat score = new Stat();
    public Stat tanksDestroyed = new Stat();
    public Stat timeAlive = new Stat();
    public Stat shotsFired = new Stat();
    public Stat damageDelt = new Stat();

    public static Stats _main;
    public static Stats Main
    {
        get
        {
            if (_main == null)
            {
                _main = new Stats();
            }
            return _main;
        }
    }

    public Stats ()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (loadMode == LoadSceneMode.Single)
        {
            Clear();
        }
    }

    private void Clear()
    {
        var fields = typeof(Stats).GetFields();
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(Stat))
            {
                ((Stat)field.GetValue(this)).Clear();
            }
        }
    }

    [System.Serializable]
    public class Stat
    {
        float value;
        public event Action ValueChangedEvent;

        public float Value
        {
            get => value;
            set
            {
                if (this.value.Equals(value)) return;

                ValueChangedEvent?.Invoke();
                this.value = value;
            }
        }

        public void Clear()
        {
            value = default;
        }

        public override string ToString() => value.ToString();
    }
}
