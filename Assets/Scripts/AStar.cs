/*
 * 优先队列实现AStar
 */
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class AStar: PathFind
{


    public AStar(bool[,] map, LinkE s, LinkE e) : base(map, s, e)
    {
    }

    public override bool Finding()
    {
        return Finding(start, end);
    }

    private bool Finding(LinkE s, LinkE e)
    {
        bool bo = false;

        MinHeap<LinkE> heap = new MinHeap<LinkE>();
        heap.Add(s);
        LinkE current;
        while (heap.Count > 0)
        {
            current = heap.ExtractMin();
            Debug.Log(current);
            Map[current.R, current.C] = false; //设为已走过

            if (current.EqualTo(end))
            {
                end.Pre = current.Pre;
                return true;
            }

            List<LinkE> nextElements = GetNextElements(current);
            if (nextElements.Count > 0)
            {
                for (int i = 0; i < nextElements.Count; i++)
                {
                    nextElements[i].Pre = current;
                    heap.Add(nextElements[i]);
                }
            }

        }

        return bo;
    }

    public override void IE_Finding()
    {
        mono.StartCoroutine(IE_finding(start, end));
    }

    private IEnumerator IE_finding(LinkE s, LinkE e)
    {
        bool bo = false;
        GameObject gameobject = new GameObject();
        List<GameObject> gos = new List<GameObject>();
        MinHeap<LinkE> heap = new MinHeap<LinkE>();
        heap.Add(s);
        LinkE current;
        while (heap.Count > 0)
        {
            yield return new WaitForSeconds(FindDeltT);
            current = heap.ExtractMin();
            Debug.Log(current);
            gameobject = GameObject.Instantiate(GameControl._Instance.cube_green);
            gameobject.transform.position = new Vector3(GetX(current.C), 0, GetZ(current.R));
            gos.Add(gameobject);
            current.go = gameobject;
            Map[current.R, current.C] = false; //设为已走过

            if (current.EqualTo(end))
            {
                end.Pre = current.Pre;
                //结束
                foreach (var item in gos)
                {
                    GameObject.Destroy(item);
                    yield return new WaitForEndOfFrame();
                }

                ShowPath(GameControl._Instance.ShowPath);

                yield break;
            }

            List<LinkE> nextElements = GetNextElements(current);
            if (nextElements.Count > 0)
            {
                for (int i = 0; i < nextElements.Count; i++)
                {
                    nextElements[i].Pre = current;
                    heap.Add(nextElements[i]);
                }
            }
          

        }

        yield return null;
    }

    public override void ShowPath(Action<LinkE> ac)
    {
        LinkE current = new LinkE(end, false);
        while (current.Pre != null)
        {
            current = current.Pre;
            ac(current);
        }
    }

    //获取节点周围可行动的点
    protected List<LinkE> GetNextElements(LinkE s)
    {
        List<LinkE> list = new List<LinkE>();
        LinkE e;
        if (s.R - 1 >= 0 && Map[s.R - 1, s.C])
        {
            e = new LinkE(s.R - 1, s.C);
            e.F = GetF(e);
            list.Add(e);
        }
        if (s.C + 1 <= MapC - 1 && Map[s.R, s.C + 1])
        {
            e = new LinkE(s.R, s.C + 1);
            e.F = GetF(e);
            list.Add(e);
        }
        if (s.R + 1 <= MapR - 1 && Map[s.R + 1, s.C])
        {
            e =new LinkE(s.R + 1, s.C);
            e.F = GetF(e);
            list.Add(e);

        }
        if (s.C - 1 >= 0 && Map[s.R, s.C - 1])
        {
            e = new LinkE(s.R, s.C - 1);
            e.F = GetF(e);
            list.Add(e);
        }

        return list;
    }

    //需要被优化
    protected override LinkE GetNextE(LinkE s)
    {
        LinkE e = null;
        if (s.R - 1 >= 0 && Map[s.R - 1, s.C])
        {
            e = new LinkE(s.R - 1, s.C);
        }
        else if (s.C + 1 <= MapC - 1 && Map[s.R, s.C + 1])
        {
            e = new LinkE(s.R, s.C + 1);
        }
        else if (s.R + 1 <= MapR - 1 && Map[s.R + 1, s.C])
        {
            e = new LinkE(s.R + 1, s.C);

        }
        else if (s.C - 1 >= 0 && Map[s.R, s.C - 1])
        {
            e = new LinkE(s.R, s.C - 1);
        }
        if (e != null)
        {
            Map[e.R, e.C] = false; //设为已走过
        }
        return e;
    }

    private int GetF(LinkE e)
    {
        int G = 1;//从上一点移动到e的距离
        int H = Math.Abs(e.C - end.C) + Math.Abs(e.R - end.R);
        return G + H;
    }
}
