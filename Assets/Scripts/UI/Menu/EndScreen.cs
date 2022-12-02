using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text;

public class EndScreen : MonoBehaviour
{
    public TMP_Text flavourTextElement;
    public Transform layout;
    public TMP_Text body;
    public GameObject[] continueButtons;

    [Space]
    public TextAsset flavourTextSource;

    [Space]
    public float shakeStrength;
    public float shakeFrequency;

    [Space]
    public Signal allPlayersDeadSignal;

    bool runSetup = false;

    private void Start()
    {
        if (runSetup) return;

        allPlayersDeadSignal.OnRaise += Show;

        runSetup = true;
        gameObject.SetActive(false);
    }

    public void StartDisplayRoutine() => StartCoroutine(DisplayRoutine());
    private IEnumerator DisplayRoutine()
    {
        yield return StartCoroutine(Typewriter($"High Score: ", 0.02f, (s) => body.text = s));

        int highScore = Save.Get().lastMaxScore;
        yield return StartCoroutine(Counter(highScore, 1.0f / highScore, (s) => body.text = s, body.text));

        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(Typewriter($"\nFinal Score: ", 0.02f, (s) => body.text = s, body.text));

        int score = (int)Stats.Main.score.Value;
        yield return StartCoroutine(Counter(score, 3.0f / score, (s) => body.text = s, body.text));

        if (score >= highScore)
        {
            yield return StartCoroutine(Typewriter("\n<size=120>New High Score</size>", 0.1f, (s) => body.text = s, body.text));
        }

        yield return new WaitForSeconds(1.0f);

        foreach (var button in continueButtons) button.SetActive(true);
    }

    private IEnumerator Typewriter (string text, float characterTime, System.Action<string> callback, string prefix = "")
    {
        var sb = new StringBuilder(prefix);

        bool tag = false;
        
        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            callback(sb.ToString());

            if (text[i] == '<') tag = true;
            if (text[i] == '>') tag = false;

            if (!tag) yield return new WaitForSeconds(characterTime);
        }

        callback(sb.ToString());
    }

    private IEnumerator Counter(int target, float characterTime, System.Action<string> callback, string prefix = "")
    {
        for (int i = 0; i < target; i++)
        {
            callback(prefix + i.ToString());

            yield return new WaitForSeconds(Mathf.Pow(1.05f, -i) * characterTime);
        }

        callback(prefix + target.ToString());
    }

    private void OnDestroy()
    {
        allPlayersDeadSignal.OnRaise -= Show;

        runSetup = false;
    }
    
    private void Update()
    {
        float angle = Mathf.PerlinNoise(Time.time * shakeFrequency, 0.5f) * Mathf.PI * 2.0f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        ((RectTransform)layout).anchoredPosition = direction * shakeStrength;
    }

    private void Show()
    {
        gameObject.SetActive(true);

        string[] flavourTexts = flavourTextSource.text.Split('\n');
        flavourTextElement.text = flavourTexts[Random.Range(0, flavourTexts.Length)];
    }

    public void TryAgain ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit ()
    {
        SceneManager.LoadScene(0);
    }
}
