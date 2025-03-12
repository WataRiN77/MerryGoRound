using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ClickableText : MyUI, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Color highlightColor;

    private TextMeshProUGUI textComponent;
    private Camera mainCamera;

    public string targetStr;
    public string palindromeStr;

    public  bool canDrag;
    private bool unlockedFlag;

    private Vector2 _vector;
    public  GameObject _Slot;
    private RectTransform rectTransform;

    private EventSystem eventSystem;
    private GraphicRaycaster caster;

    private GameManager _gmgr;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();

        eventSystem = FindObjectOfType<EventSystem>();

        caster = FindObjectOfType<GraphicRaycaster>();

        _gmgr = FindObjectOfType<GameManager>();
        //Debug.Log("Script Active");

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click Active");
        // 获取点击位置
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            textComponent.rectTransform,
            eventData.position,
            mainCamera,
            out localPosition);
        Debug.Log($"点击了{localPosition}");

        textComponent.ForceMeshUpdate();

        // 检测字符索引
        int charIndex = TMP_TextUtilities.FindIntersectingCharacter(
            textComponent,
            eventData.position,
            mainCamera,
            false);
        Debug.Log($"点击了{charIndex}");

        if (charIndex != -1 && !unlockedFlag)
        {
            // 获取字符信息
            TMP_CharacterInfo charInfo = textComponent.textInfo.characterInfo[charIndex];

            // 高亮被点击的字符（可选）
            

            // 触发自定义事件
            OnCharacterClicked(charInfo.character);
        }
    }

    private IEnumerator HighlightCharacter(TextMeshProUGUI TMP_Text)
    {
        // 获取字符的顶点数据
        //int meshIndex = textComponent.textInfo.characterInfo[charIndex].materialReferenceIndex;
        //int vertexIndex = textComponent.textInfo.characterInfo[charIndex].vertexIndex;

        //Color32[] vertexColors = textComponent.textInfo.meshInfo[meshIndex].colors32;

        // 保存原始颜色
        Color originalColor = TMP_Text.color;

        // 设置高亮颜色
        //highlightColor
        float duration = 10f;
        float targetSpace = 50;
        float kp;
        for(float timer = 0f; timer < duration; timer += Time.deltaTime)
        {
            kp = timer / duration;
            TMP_Text.characterSpacing += kp * targetSpace >= 0.02f ? kp * targetSpace : 0.02f;
            if (TMP_Text.characterSpacing >= 15) break;
            yield return new WaitForEndOfFrame();
        }

        for (float timer = 0f; timer < duration; timer += Time.deltaTime)
        {
            kp = 0.3f - timer / duration <= 0.08f ? 0.08f : 0.3f - timer / duration;
            TMP_Text.characterSpacing -= kp * kp * kp * targetSpace >= 0.01f ? kp * kp * kp * targetSpace : 0.01f;
            if (TMP_Text.characterSpacing <= -200) break;
            yield return new WaitForEndOfFrame();
        }

        // 保持高亮1秒
        yield return new WaitForSeconds(1f);

        // 恢复颜色

        if (TMP_Text.text != palindromeStr)
        {
            float interval = 0.1f;
            string originalStr = TMP_Text.text;

            for (int i = TMP_Text.text.Length; i >= 0; i--)
            {
                TMP_Text.text = originalStr.Substring(0, i);
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForSeconds(interval);
            StringBuilder sb = new StringBuilder("");

            for (int i = 0;i < palindromeStr.Length; i++)
            {
                TMP_Text.text = sb.Append(palindromeStr[i]).ToString();
                yield return new WaitForSeconds(interval);
            }
        }

        TMP_Text.color = highlightColor;
        //TMP_Text.fontSize = 169;
        unlockedFlag = true;

        //yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(_gmgr.StartFadeOut());
        //FadeOutParent();

        //FadeInNext();
    }

    private void OnCharacterClicked(char clickedChar)
    {
        Debug.Log($"点击了字符: {clickedChar}");

        if(targetStr.Contains(clickedChar))
        {
            StartCoroutine(HighlightCharacter(textComponent));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag && unlockedFlag) _vector = this.rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag && unlockedFlag) this.rectTransform.anchoredPosition = new Vector3(this.rectTransform.anchoredPosition.x + eventData.delta.x < _vector.x ? this.rectTransform.anchoredPosition.x + eventData.delta.x : this.rectTransform.anchoredPosition.x, this.rectTransform.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canDrag && unlockedFlag)
        {
            bool isSlot = false;

            Vector3 slotVector = new Vector3();

            var list = GraphicRaycaster(Input.mousePosition);

            foreach(var item in list)
            {
                if(item.gameObject.tag == "Slot")
                {
                    isSlot = true;
                    slotVector = item.gameObject.transform.position;
                }
            }

            //if (this.rectTransform.position.x <= -1311f) isSlot = true;

            if(isSlot)
            {
                Debug.Log("Activated");
            }
            else
            {
                this.rectTransform.anchoredPosition = _vector;
            }
            Debug.Log(isSlot);
        }

    }
    private List<RaycastResult> GraphicRaycaster(Vector2 pos)
    {
        var _pointerEventData = new PointerEventData(eventSystem);

        _pointerEventData.position = pos;

        List<RaycastResult> results = new List<RaycastResult>();

        caster.Raycast(_pointerEventData, results);

        return results;
    }
    private IEnumerator StartExecuteObj()
    {
        yield return null;
    }

    public void FadeOutParent()
    {
        FadeOut(transform.parent.GetComponent<CanvasGroup>());
    }
    
}
