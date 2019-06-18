using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
class DFS_Stack : PathFind
{

    private Stack<LinkE> linkEStack;
    public DFS_Stack(bool[,] map, LinkE s, LinkE e) : base(map, s, e)
    {
    }

    public override bool Finding()
    {
        return Finding(start,end);
    }

    private bool Finding(LinkE s, LinkE e)
    {
        bool bo = false;
        linkEStack = new Stack<LinkE>();
        linkEStack.Push(s);
        LinkE current;
        while (linkEStack.Count>0)
        {
            current = linkEStack.Pop();
            Debug.Log(current);
         
            if (current.EqualTo(e))
            {
                e.Pre = current.Pre;
                return true;
            }
           LinkE temp ;
           while ((temp = GetNextE(current) )!= null)
            {
                temp.Pre = current;
                linkEStack.Push(temp);
            }

        }

        return bo;
    }



    public override void ShowPath(Action<LinkE> ac)
    {
        LinkE current = new LinkE(end, false);
        while (current.Pre!=null)
        {
            current = current.Pre;
            ac(current);
        }
    }

    public override void IE_Finding()
    {
        mono.StartCoroutine(IE_finding(start,end));
    }

    private IEnumerator IE_finding(LinkE s,LinkE e)
    {
        linkEStack = new Stack<LinkE>();
        linkEStack.Push(s);
        LinkE current;
        GameObject gameobject = new GameObject();
        while (linkEStack.Count > 0)
        {

            yield return new WaitForSeconds(FindDeltT);
            current = linkEStack.Pop();
            Debug.Log(current);
            gameobject = GameObject.Instantiate(GameControl._Instance.cube_green);
            gameobject.transform.position = new Vector3(GetX(current.C), 0, GetZ(current.R));
            current.go = gameobject;
            if (current.EqualTo(e))
            {
                e.Pre = current.Pre;
                yield break;
            }
            LinkE temp;

            bool isAdd = false;

            //顺时针压入栈中
            while ((temp = GetNextE(current)) != null)
            {
                temp.Pre = current;
                linkEStack.Push(temp);
                isAdd = true;
            }

            //删除操作
            if (!isAdd)
            {
                temp = new LinkE();
                temp.Pre = current;
                while (temp.Pre != linkEStack.Peek().Pre)
                {
                    temp = temp.Pre;
                    GameObject.Destroy(temp.go);
                }

            }

        }

        yield return null;
    }

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
        if (e!=null)
        {
            Map[e.R, e.C] = false; //设为已走过
        }
        return e;
    }




}