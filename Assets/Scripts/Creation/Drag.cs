using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drag : MonoBehaviour
{
    Vector3 startPos;
    Vector3 lastPos;

    public int bi, bj;
    public int sizeOfVis;           // 用于控制House分散放置 3

    public int[,] tableOfType;

    void Start()
    {
        tableOfType = Creation.instance.tableOfType;
        lastPos = startPos;
    }

    void Update()
    {

    }

    private Vector3 screenPos;
    private Vector3 offset;
    void OnMouseDown()
    {
        startPos = transform.position;
        bi = Mathf.RoundToInt(startPos.x);
        bj = Mathf.RoundToInt(startPos.y);
        screenPos = Camera.main.WorldToScreenPoint(transform.position);//获取物体的屏幕坐标     
        offset = screenPos - Input.mousePosition;//获取物体与鼠标在屏幕上的偏移量    
    }

    List<Vector2Int> path;
    void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + offset);
        int ei = Mathf.RoundToInt(transform.position.x);
        int ej = Mathf.RoundToInt(transform.position.y);
        transform.position = new Vector3(ei, ej, transform.position.z);
        if (lastPos == transform.position) return;
        if (ei < 0 || ei >= Global.instance.width || ej < 0 || ej >= Global.instance.height) return;

        if (placeRoad(new Vector2Int(bi, bj), new Vector2Int(ei, ej), out path))
        {
            if (tableOfType[ei, ej] == 0)
            {
                tableOfType[ei, ej] = 2;

                List<Vector2Int> temp = new List<Vector2Int>();
                for (int index = 0; index < path.Count; index++)
                {
                    Vector2Int pos = path[index];
                    if (tableOfType[pos.x, pos.y] == 0)
                    {
                        tableOfType[pos.x, pos.y] = 1;
                        temp.Add(pos);
                    }
                }
                Creation.instance.draw();
                foreach (var pos in temp)
                {
                    tableOfType[pos.x, pos.y] = 0;
                }
                tableOfType[ei, ej] = 0; // TODO
            }
        }
        lastPos = transform.position;
    }

    void OnMouseUp()
    {
        if (path.Count == 0) return;

        int ei = Mathf.RoundToInt(transform.position.x);
        int ej = Mathf.RoundToInt(transform.position.y);
        tableOfType[ei, ej] = 2;

        Creation.instance.dic["[" + bi + "," + bj + "],[" + ei + "," + ej + "]"] = new List<Vector2Int>(path); // TODO key 格式
        Creation.instance.dic["[" + ei + "," + ej + "],[" + bi + "," + bj + "]"] =
            new List<Vector2Int>(Creation.instance.dic["[" + bi + "," + bj + "],[" + ei + "," + ej + "]"]);
        Creation.instance.dic["[" + ei + "," + ej + "],[" + bi + "," + bj + "]"].Reverse();

        for (int index = 0; index < path.Count; index++)
        {
            Vector2Int pos = path[index];
            if (tableOfType[pos.x, pos.y] == 0)
                tableOfType[pos.x, pos.y] = 1;
        }
        Creation.instance.draw(false);

        //for (int i = 0; i < Global.instance.width; i++)
        //{
        //    string str = "";
        //    for (int j = 0; j < Global.instance.width; j++)
        //    {
        //        str += tableOfType[i, j];
        //    }
        //    Debug.Log(str);
        //}
    }


    bool placeRoad(Vector2Int start, Vector2Int end, out List<Vector2Int> path)
    {
        bool isReverse = false;
        path = new List<Vector2Int>();
        Vector2Int dir = end - start;
        Vector2 line = start;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (start.x > end.x)
            {
                Vector2Int temp = start;
                start = end;
                end = temp;
                dir = end - start;
                line = start;
                isReverse = true;
            }
            float k = ((float)dir.y) / dir.x;
            int lastY = -1;
            while (line.x != end.x)
            {
                line.x++;
                line.y += k;
                if (lastY != -1 && lastY != (int)line.y)
                    path.Add(new Vector2Int((int)line.x, lastY));
                lastY = (int)line.y;
                path.Add(new Vector2Int((int)line.x, (int)line.y));
            }
        }
        else
        {
            if (start.y > end.y)
            {
                Vector2Int temp = start;
                start = end;
                end = temp;
                dir = end - start;
                line = start;
                isReverse = true;
            }
            float k = dir.x / ((float)dir.y);
            int lastX = -1;
            while (line.y != end.y)
            {
                line.y++;
                line.x += k;
                if (lastX != -1 && lastX != (int)line.x)
                    path.Add(new Vector2Int(lastX, (int)line.y));
                lastX = (int)line.x;
                path.Add(new Vector2Int((int)line.x, (int)line.y));
            }
        }
        if (isReverse) path.Reverse();

        // 两条道路间周围存在道路或者水泡，返回false
        for (int i = 4; i < path.Count - 4; i++) // TODO
            for (int offsetY = -sizeOfVis / 2; offsetY <= sizeOfVis / 2; offsetY++)
                for (int offsetX = -sizeOfVis / 2; offsetX <= sizeOfVis / 2; offsetX++)
                {
                    int newX = offsetX + path[i].x;
                    int newY = offsetY + path[i].y;
                    if (tableOfType[newX, newY] == 1 || tableOfType[newX, newY] == 2)
                        return false;
                }
        return true;
    }
}
