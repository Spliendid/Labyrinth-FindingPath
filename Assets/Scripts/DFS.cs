using System;
using System.Collections;
using UnityEngine;
 class DFS : PathFind
 {
     public DFS(bool[,] map, LinkE s,LinkE e) : base(map,s, e)
     {

     }

     public override bool Finding()
     {
         return FindingPath(start,end);
     }


    public override void IE_Finding()
    {
        mono.StartCoroutine(IE_finding( start,end));
    }

    private IEnumerator IE_finding(LinkE s,LinkE e)
    {

        if (isFindPass)
        {
            yield break;
        }

        yield return new WaitForSeconds(FindDeltT);
        LinkE linkE = null;
        Map[s.R, s.C] = false; //设为已走过
        GameObject gameobject = GameObject.Instantiate(GameControl._Instance.cube_green);
        gameobject.transform.position = new Vector3(GetX(s.C), 0, GetZ(s.R));

        

        if (s.EqualTo(e))
        {
            isFindPass = true;
            yield break;
        }

        while ((linkE = GetNextE(s)) != null)
        {
           yield return mono.StartCoroutine(IE_finding(linkE, e));
            if (isFindPass)
            {
                s.Next = linkE;
                yield break;
            }

        }
        GameObject.Destroy(gameobject);

        yield return null;
    }

    private bool FindingPath(LinkE s,LinkE e)
    {
        LinkE linkE = null;
        Map[s.R, s.C] = false; //设为已走过
    

        if (s .EqualTo(e))
        {
            return true;
        }

        while ((linkE = GetNextE(s))!=null)
        {
            bool bo = FindingPath(linkE, e);
            if (bo)
            {
                s.Next = linkE;
                return true;
            }
          
        }

        return false;
          
        
    }

    //顺时针，从上开始
    protected override LinkE GetNextE(LinkE s)
    {
        LinkE e = null;
        if ( s.R - 1>=0&&Map[s.R-1,s.C])
        {
            e = new LinkE(s.R - 1, s.C);
        }
        else if (s.C + 1 <= MapC-1 && Map[s.R, s.C + 1])
        {
            e = new LinkE(s.R , s.C+1);
        }
        else if (s.R + 1 <=MapR-1 && Map[s.R + 1, s.C])
        {
            e = new LinkE(s.R +1, s.C);

        }
        else if (s.C -1 >= 0 && Map[s.R, s.C - 1])
        {
            e = new LinkE(s.R , s.C-1);
        }


        return e;
    }

    public override void ShowPath(Action<LinkE> ac)
    {
        LinkE current = new LinkE(start);
        while (current.Next!=null)
        {
            current = current.Next;
            ac(current);
        }
    }
}

