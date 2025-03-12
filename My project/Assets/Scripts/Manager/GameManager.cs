using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyUI
{
    public GameObject[] reversableTexts;
    public GameObject[] irreversableTexts;
    public GameObject levelName;

    private static int textIndex;
    private bool initialized;
    private void Awake()
    {
        textIndex = 0;
        initialized = true;
        StartCoroutine(StartFadeIn(initialized));
    }
    public IEnumerator StartFadeOut(bool initialized = false)
    {
        Debug.Log($"FadeOutIndex = {textIndex}");
        if(!initialized)
        {
            yield return StartCoroutine(HandleFadeOut(irreversableTexts[textIndex].GetComponent<CanvasGroup>(), 1f, 0.5f));
            textIndex++;
            if (textIndex < irreversableTexts.Length) StartCoroutine(StartFadeIn());
        }
        else
        {
            yield return StartCoroutine(HandleFadeOut(levelName.GetComponent<CanvasGroup>(), 1.5f, 3f));
        }
    }

    public IEnumerator StartFadeIn(bool initialized = false)
    {
        if (!initialized) yield return StartCoroutine(HandleFadeIn(irreversableTexts[textIndex].transform.parent.GetComponent<CanvasGroup>(), 1f, 0.5f));
        else
        {
            yield return StartCoroutine(HandleFadeIn(levelName.GetComponent<CanvasGroup>(), 1.5f, 1.5f));
            yield return StartCoroutine(StartFadeOut(initialized));
            yield return StartCoroutine(HandleFadeIn(irreversableTexts[textIndex].transform.parent.GetComponent<CanvasGroup>(), 1f, 1f));
        }
    }

}
