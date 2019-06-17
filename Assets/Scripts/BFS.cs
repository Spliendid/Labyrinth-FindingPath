using System;
using System.Collections;
using UnityEngine;
 class BFS : PathFind
 {
     public BFS(bool[,] map, LinkE s,LinkE e) : base(map,s, e)
     {

     }

     public override bool Finding()
     {
         return FindingPath(start,end);
     }

    public MonoBehaviour mono;
    private bool isFindPass = false;

    public void IE_Finding()
    {
        mono.StartCoroutine(IE_finding( start,end));
    }

    private IEnumerator IE_finding(LinkE s,LinkE e)
    {

        if (isFindPass)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.2f);
        LinkE linkE = null;
        Map[s.R, s.C] = false; //设为已走过
        GameObject gameobject = GameObject.Instantiate(GameControl._Instance.cube_green);
        gameobject.transform.position = new Vector3(GetX(s.C), 0, GetZ(s.R));

        

        if (s.CompareTo(e))
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
    

        if (s .CompareTo(e))
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
    private LinkE GetNextE(LinkE s)
    {
        Debug.Log("Thread.Sleep");
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

    private float GetX(int c)
    {
        float halfC = MapC / 2f;
        return c - halfC;
    }

    private float GetZ(int r)
    {
        float halfR = MapR / 2f;
        return halfR - r;
    }
}

