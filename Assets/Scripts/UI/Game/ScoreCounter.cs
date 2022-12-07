using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

/// <summary>
/// Class that runs the ui for the Score Counter.
/// </summary>
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [TextArea]
    [SerializeField] string titleTemplate;
    [SerializeField] AnimationCurve scaleCurve;

    [Space]
    [SerializeField] TMP_Text body;
    [SerializeField] float expireTime;

    float lastScoreChangeTime;

    List<Message> messages = new List<Message>();
    float oldestMessage;

    static event Action<Message> DisplayEvent;

    private void OnEnable()
    {
        Stats.Main.score.ValueChangedEvent += SetTitle;
        DisplayEvent += DisplayInstance;

        SetTitle();
    }

    private void OnDisable()
    {
        Stats.Main.score.ValueChangedEvent -= SetTitle;
        DisplayEvent -= DisplayInstance;
    }

    private void Start()
    {
        body.text = string.Empty;
    }

    private void Update()
    {
        title.transform.localScale = Vector3.one * scaleCurve.Evaluate(Time.time - lastScoreChangeTime);

        if (Time.time > oldestMessage + expireTime)
        {
            SetBody();
        }
    }

    /// <summary>
    /// Updates the title.
    /// </summary>
    private void SetTitle()
    {
        title.text = string.Format(titleTemplate, Stats.Main.score);
        lastScoreChangeTime = Time.time;
    }

    /// <summary>
    /// Static version of DisplayInstance.
    /// </summary>
    /// <param name="message"></param>
    public static void Display (Message message)
    {
        DisplayEvent?.Invoke(message);
    }

    /// <summary>
    /// Displays a message in the score counter body, will fade away after a specified time.
    /// </summary>
    /// <param name="message"></param>
    public void DisplayInstance(Message message)
    {
        messages.Add(message);

        SetBody();
    }

    /// <summary>
    /// Updates the body text of the score counter object.
    /// </summary>
    private void SetBody()
    {
        var sb = new StringBuilder();

        messages.RemoveAll(q => q.time + expireTime < Time.time);

        for (int i = messages.Count - 1; i >= 0; i--)
        {
            var message = messages[i];
            sb.Append(message);
            sb.Append("\n");

            if (message.time < oldestMessage)
            {
                oldestMessage = message.time;
            }
        }

        body.text = sb.ToString();
    }

    /// <summary>
    /// Wrapper class for holding score message related data.
    /// </summary>
    public struct Message
    {
        public string text;
        public float time;

        public Message (string text)
        {
            this.text = text;
            time = Time.time;
        }

        public override string ToString() => text;
        public static implicit operator Message(string text) => new Message(text);
    }
}
