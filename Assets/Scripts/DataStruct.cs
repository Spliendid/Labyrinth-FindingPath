using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
#region 链表元素类

public class LinkE : IComparable<LinkE>
{
    public int C;//列数
    public int R;//行数
    public LinkE Next = null;
    public LinkE Pre = null;
    public float F;//F值
    public GameObject go;

    public LinkE() { }

    public LinkE(int r, int c)
    {
        this.R = r;
        this.C = c;
    }


    public LinkE(LinkE e, bool isNext = true)
    {
        if (isNext)
            this.Next = e;
        else
            this.Pre = e;
    }

    public bool EqualTo(LinkE otherE)
    {
        return this.R == otherE.R && this.C == otherE.C;
    }

    public override string ToString()
    {
        return $"R:{R}  C:{C}";
    }

    public int CompareTo(LinkE OtherE)
    {
        return this.F.CompareTo(OtherE.F);
    }
}

#endregion

#region Heap

public class MinHeap<T> where T : IComparable<T>
{
    private List<T> list = new List<T>();

    public int Count { get { return list.Count; } }

    public MinHeap()
    {

    }


    public void Add(T t)
    {
        list.Add(t);
        ShiftUp(list.Count-1);
    }

    //查找最小值
    public T FindMin()
    {
        if (list.Count == 0)
        {
            throw new Exception("Can not FindMin when heap is empty.");
        }
        return list[0];
    }

    public T ExtractMin()
    {
        T min = FindMin();
        Swap(0,list.Count-1);
        list.RemoveAt(list.Count-1);

        ShiftDown(0);

        return min;
    }

    //返回父节点位置
    private int GetParent(int index)
    {
        if (index == 0)
        {
            throw new Exception("Index-0 is doesn't have parent");
        }
        return (index - 1) / 2;
    }

    private int GetLeftChild(int index)
    {
        return 2 * index;
    }

    private int GetRightChild(int index)
    {
        return 2 * index+1;
    }

    private void ShiftUp(int index)
    {
        while (index > 0 && list[index].CompareTo(list[GetParent(index)])<0)
        {
            Swap(index,GetParent(index));
            index = GetParent(index);
        }
    }

    private void ShiftDown(int index)
    {
        while (GetLeftChild(index)<list.Count)
        {
            int j = GetLeftChild(index);

            //获取最小的子节点
            if (j+1<list.Count)
            {
                if (list[j].CompareTo(list[j+1])>0)
                {
                    j++;
                }
            }

            if (list[index].CompareTo(list[j])<0)
            {
                break;
            }

            Swap(index,j);
            index = j; 
        }
    }
    public void Swap(int a,int b)
    {
        T Temp = list[a];
        list[a] = list[b];
        list[b] = Temp;
    }
}

#endregion

public enum E_PathFind
{
    DFS = 1,
    DFS_Stack = 2,
    BFS = 3,
    AStar_Stack = 4,
    BStar = 5,
    Dijkstra = 6,
    SPFA = 7,
    AStar = 8,
}

public enum E_FindingType
{
    Instant = 1,
    Gradually = 2,
}
