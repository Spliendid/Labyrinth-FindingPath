using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class BFS_Queue : PathFind
{
    public BFS_Queue(bool[,] map, LinkE s, LinkE e) : base(map, s, e)
    {
    }

    public override bool Finding()
    {
        return Finding(start,end);
    }

    private bool Finding(LinkE s,LinkE e)
    {
        bool bo = false;
        Queue<LinkE> queue = new Queue<LinkE>();
        queue.Enqueue(s);

        LinkE current;
        while (queue.Count>0)
        {
            current = queue.Dequeue();
            if (current.EqualTo(end))
            {
                end.Pre = current.Pre;
                return true;
            }

            LinkE temp;

            while ((temp = GetNextE(current))!=null)
            {
                temp.Pre = current;
                queue.Enqueue(temp);
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
        Queue<LinkE> queue = new Queue<LinkE>();
        queue.Enqueue(s);

        GameObject gameobject = new GameObject();
        List<GameObject> gos = new List<GameObject>();

        LinkE current;
        while (queue.Count > 0)
        {
            yield return new WaitForSeconds(FindDeltT);
            current = queue.Dequeue();

            Debug.Log(current);
            gameobject = GameObject.Instantiate(GameControl._Instance.cube_green);
            gameobject.transform.position = new Vector3(GetX(current.C), 0, GetZ(current.R));
            gos.Add(gameobject);


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

            LinkE temp;

            while ((temp = GetNextE(current)) != null)
            {
                temp.Pre = current;
                queue.Enqueue(temp);
            }

        }
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
}

