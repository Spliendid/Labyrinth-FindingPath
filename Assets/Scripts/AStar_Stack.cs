/*
 * 假的AStar寻路，其实还是DFS
 */
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class AStar_Stack : PathFind
{
    public AStar_Stack(bool[,] map, LinkE s, LinkE e) : base(map, s, e)
    {
    }

    public override bool Finding()
    {
        return Finding(start,end);
    }

    private bool Finding(LinkE s,LinkE e)
    {
        bool bo = false;

        Stack<LinkE> stack = new Stack<LinkE>();
        stack.Push(s);
        LinkE current;
        while (stack.Count>0)
        {
            current = stack.Pop();
            Debug.Log(current);
            Map[current.R, current.C] = false; //设为已走过

            if (current.EqualTo(end))
            {
                end.Pre = current.Pre;
                return true;
            }

            List<LinkE> nextElements = GetNextElements(current);
            if (nextElements.Count>0)
            {
                for (int i = 0; i < nextElements.Count; i++)
                {
                    nextElements[i].Pre = current;
                    stack.Push(nextElements[i]);
                }
            }
            
        }

        return bo;
    }

    public override void IE_Finding()
    {
        mono.StartCoroutine(IE_finding(start,end));
    }

    private IEnumerator IE_finding(LinkE s,LinkE e)
    {
        bool bo = false;
        GameObject gameobject = new GameObject();
        Stack<LinkE> stack = new Stack<LinkE>();
        stack.Push(s);
        LinkE current;
        while (stack.Count > 0)
        {
            yield return new WaitForSeconds(FindDeltT);
            current = stack.Pop();
            Debug.Log(current);

            gameobject = GameObject.Instantiate(GameControl._Instance.cube_green);
            gameobject.transform.position = new Vector3(GetX(current.C), 0, GetZ(current.R));
            current.go = gameobject;
            Map[current.R, current.C] = false; //设为已走过

            if (current.EqualTo(end))
            {
                end.Pre = current.Pre;
                yield break;
            }

            List<LinkE> nextElements = GetNextElements(current);
            if (nextElements.Count > 0)
            {
                for (int i = 0; i < nextElements.Count; i++)
                {
                    nextElements[i].Pre = current;
                    stack.Push(nextElements[i]);
                }
            }
            else
            {
                //删除
                LinkE temp = new LinkE(current,false);
                while (temp.Pre!=stack.Peek().Pre)
                {
                    temp = temp.Pre;
                    GameObject.Destroy(temp.go);
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

    //按F从大到小排列
    protected List<LinkE> GetNextElements(LinkE s)
    {
        List<LinkE> list = new List<LinkE>();
        if (s.R - 1 >= 0 && Map[s.R - 1, s.C])
        {
            list.Add( new LinkE(s.R - 1, s.C));
        }
        if (s.C + 1 <= MapC - 1 && Map[s.R, s.C + 1])
        {
            list.Add(new LinkE(s.R, s.C + 1));
        }
        if (s.R + 1 <= MapR - 1 && Map[s.R + 1, s.C])
        {
            list.Add(new LinkE(s.R + 1, s.C));

        }
        if (s.C - 1 >= 0 && Map[s.R, s.C - 1])
        {
            list.Add(new LinkE(s.R, s.C - 1));
        }

        list.Sort((a,b)=>(GetF(b)-GetF(a)));

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
        int H = Math.Abs(e.C - end.C) + Math.Abs(e.R-end.R);
        return G + H;
    }
}
