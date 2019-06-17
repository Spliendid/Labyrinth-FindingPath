
abstract class PathFind
{
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

        public abstract bool Finding();
    }

