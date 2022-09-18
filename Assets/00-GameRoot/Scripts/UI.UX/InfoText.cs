using UnityEngine;
using TMPro;

public class InfoText : MonoBehaviour
{
    TextMeshProUGUI _text;
    public static InfoText instance;
    
    void Awake()
    {
        instance = this;
        _text = GetComponent<TextMeshProUGUI>();
        _text.enabled = false;
    }
    void SetOnScreenText(string text)
    {
        _text.text = text;
        _text.enabled = true;
    }
    void ClearOnScreenText()
    {
        _text.text = string.Empty;
        _text.enabled = false;
    }

    /*              PUBLIC STATICS              */

    public static void Static_SetOnScreenText(string text)
    {
        instance.SetOnScreenText(text);
    }
    public static void Static_ClearOnScreenText()
    {
        instance.ClearOnScreenText();
    }
}
