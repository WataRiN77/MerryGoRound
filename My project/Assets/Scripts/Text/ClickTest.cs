using UnityEngine.EventSystems;
using UnityEngine;

public class ClickTest : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"点击对象：{name} 坐标：{eventData.position}");
    }
}