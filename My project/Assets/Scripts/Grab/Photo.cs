using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.EventSystems;

using UnityEngine.UI;



public class Photo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{

    //记录玩家开始拖拽时的位置

    private Vector3 vector;

    //需要移动物品的位置组件

    private RectTransform rectTransform;

    //UI事件管理器

    private EventSystem _EventSystem;

    private GraphicRaycaster gra;

    private Vector3 initialPosition;



    private void Awake()
    {

        _EventSystem = FindObjectOfType<EventSystem>();

        gra = FindObjectOfType<GraphicRaycaster>();

        rectTransform = GetComponent<RectTransform>();

    }

    /// <summary>

    /// 开始拖拽时执行一次

    /// </summary>

    /// <param name="eventData"></param>

    public void OnBeginDrag(PointerEventData eventData)
    {
        vector = this.transform.position;
    }



    /// <summary>

    /// 拖拽时持续调用

    /// </summary>

    /// <param name="eventData"></param>

    public void OnDrag(PointerEventData eventData)
    {
        this.rectTransform.anchoredPosition += eventData.delta;
    }

    /// <summary>

    /// 结束拖拽时执行一次

    /// </summary>

    /// <param name="eventData"></param>

    public void OnEndDrag(PointerEventData eventData)

    {
        //判断是否拖拽到格子上

        bool isSolt = false;

        //保存格子的位置

        Vector3 SlotVector = new Vector3();

        //通过定义的读取方法读取鼠标所在位置的UI对象

        var list = GraphicRaycaster(Input.mousePosition);

        foreach (var item in list)
        {
            //检测是否在物品上

            if (item.gameObject.tag == "Item")
            {
                //如果在物品上则执行以下代码

                //---交换位置---//

                this.rectTransform.position = item.gameObject.transform.position;

                item.gameObject.transform.position = vector;
            }

            //检测是否在格子上
            else if (item.gameObject.tag == "Slot")
            {
                //如果在格子上则将isSlot设置为true方便后续代码

                isSolt = true;

                //保存格子位置坐标以便切换位置

                SlotVector = item.gameObject.transform.position;

            }

        }

        //判断是否检测在格子上

        if (isSolt)
        {
            //如果在格子上,则切换到格子上

            this.rectTransform.position = SlotVector;
        }

        else
        {

            //如果不在格子上,则返回原位置

            this.rectTransform.position = vector;

        }

    }



    /// <summary>

    /// 定义通过射线读取所在位置的UI对象

    /// </summary>

    /// <param name="pos">射线位置</param>

    /// <returns>返回读取的所有UI对象</returns>

    private List<RaycastResult> GraphicRaycaster(Vector2 pos)

    {

        var mPointerEventData = new PointerEventData(_EventSystem);

        mPointerEventData.position = pos;

        List<RaycastResult> results = new List<RaycastResult>();

        gra.Raycast(mPointerEventData, results);

        return results;

    }



}