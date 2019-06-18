using System.Collections;
using UnityEngine;
using System;
abstract class PathFind
{
    public MonoBehaviour mono;
    protected bool isFindPass = false;
    public float FindDeltT = 0.1f;
    protected bool[,] Map;
    protected LinkE start;
    protected LinkE end;
    public int MapR{get{ return Map.GetLength(0); }}
    public int MapC{get{ return Map.GetLength(1); } }

    public PathFind(bool[,] map,LinkE s,LinkE e)
        {
            this.Map = map;
            this.start = s;
            this.end = e;
        }

    public abstract bool Finding();//非携程寻路
    public abstract void IE_Finding();//携程寻路
    protected abstract LinkE GetNextE(LinkE s);//寻路策略
    public abstract void ShowPath(Action<LinkE> ac);//显示路径

    protected float GetX(int c)
    {
        float halfC = MapC / 2f;
        return c - halfC;
    }

    protected float GetZ(int r)
    {
        float halfR = MapR / 2f;
        return halfR - r;
    }

}



