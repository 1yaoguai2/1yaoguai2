using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ìæ»»°´Å¥Í¼Æ¬
/// </summary>
public class ButtonReplaceImage : MonoBehaviour
{
    public Sprite downSprite;
    private Sprite normalSprite;
    private bool buttonDown;
    
    public void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(ReplaceImage);
    }
    private void Start()
    {
        normalSprite = GetComponent<Button>().image.sprite;
    }
    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void ReplaceImage()
    {
        buttonDown = !buttonDown;
        GetComponent<Button>().image.sprite = buttonDown ? downSprite : normalSprite;
    }

}
