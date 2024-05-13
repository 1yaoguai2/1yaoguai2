using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 defaultScale;
    public Vector3 dynamicScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float time = 2f;
    private Vector3 changeVector
    {
        get => (dynamicScale - defaultScale) / time;
    }

    void Start()
    {
        defaultScale = transform.localScale;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(MosueEnterOrDown(true));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(MosueEnterOrDown(false));
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator MosueEnterOrDown(bool enterB)
    {
        yield return new WaitForSeconds(0.1f);
        if (enterB)
        {
            for (int i = 0; i < time; i++)
            {
                transform.localScale += changeVector;

                yield return new WaitForSeconds(0.05f);
            }
            transform.localScale = dynamicScale;
        }
        else
        {
            for (int i = 0; i < time; i++)
            {
                transform.localScale -= changeVector;

                yield return new WaitForSeconds(0.05f);
            }
            transform.localScale = defaultScale;
        }


        yield return null;
    }
}
