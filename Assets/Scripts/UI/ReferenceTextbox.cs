using UnityEngine;
using TMPro;
using System.Text;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public sealed class ReferenceTextbox : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        var sb = new StringBuilder();

        for (int i = 0; i < text.text.Length;)
        {
            if (text.text[i] == '#')
            {
                sb.Append(GetReferenceText(text.text, ref i));
            }
            else sb.Append(text.text[i++]);
        }

        text.text = sb.ToString();
    }

    private string GetReferenceText(string word, ref int head)
    {
        int start = head;

        while (head <= word.Length)
        {
            switch (word.Substring(start, head - start))
            {
                case "#GAME_VERSION":
                    return Application.version.ToString();
            }

            head++;
        }
        
        return word[start].ToString();
    }
}
