using System.Collections;
using TMPro;
using UnityEngine;

public class TextColorChange : MonoBehaviour
{
    TextMeshProUGUI _text;

    Color _dark = new Color(0.29f, 0.29f, 0.29f, 1);
    Color _light = new Color(1, 1, 1, 1);

    bool _ColorChanged;



    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UIManager.changeTextColor += ChangeColor;
    }

    // Update is called once per frame
    void ChangeColor()
    {
        if (UXManager.inDarkMode && !_ColorChanged)
            _text.color = _light;
        else
            _text.color = _dark;

    }

    //private void OnEnable()
    //{
    //    ChangeColor();
    //}
}
