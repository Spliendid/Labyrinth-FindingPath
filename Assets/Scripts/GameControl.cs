using UnityEngine;
using System.IO;
using System.Threading;
public class GameControl : MonoBehaviour
{
    public E_PathFind E_PF;
    public E_FindingType E_FT;
    public float FindDeltT = 0.2f;
    public GameObject cube;
    public GameObject cube_red;
    public GameObject cube_green;
    string Path = "";
    int row, column;
    private bool[,] Map;
    private LinkE start;
    private LinkE end;

    private PathFind pf;
    public static GameControl _Instance;
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        LoadMap();
        CreatMap();
        FindingPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LoadMap()
    {
        Path = Application.streamingAssetsPath + "/" + "Map.txt";
        StreamReader reader = new StreamReader(Path);
     
        string[]rc = reader.ReadLine().Split(' ');
        column = int.Parse(rc[0]);
        row = int.Parse(rc[1]);
        Map = new bool[row, column];

        rc = reader.ReadLine().Split(' ');
        start = new LinkE(int.Parse(rc[0]),int.Parse(rc[1]));

        rc = reader.ReadLine().Split(' ');
        end = new LinkE(int.Parse(rc[0]), int.Parse(rc[1]));

        for (int i = 0; i < row; i++)
        {
            string line = reader.ReadLine();
            for (int j = 0; j < column; j++)
            {
                    Map[i, j] = line[j] == '0';
            }
        }
        Debug.Log(Map);
    }

    private void CreatMap()
    {
      
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (!Map[i,j])
                {
                    GameObject go = GameObject.Instantiate(cube);
                    go.transform.position = new Vector3(GetX(j),0,GetZ(i));
                }
            }
        }
    }

    private void FindingPath()
    {

        switch (E_PF)
        {
            case E_PathFind.DFS:

                 pf = new DFS(Map,start,end);

              
                break;
            case E_PathFind.DFS_Stack:
                pf = new DFS_Stack(Map,start,end);
                break;
            case E_PathFind.BFS:
                pf = new BFS_Queue(Map, start, end);
                break;
            case E_PathFind.AStar:
                pf = new AStar(Map, start, end);
                break;
            case E_PathFind.BStar:
                break;
            case E_PathFind.Dijkstra:
                break;
            default:
                break;
        }

        if (E_FT == E_FindingType.Gradually)
        {
            pf.mono = this;
            pf.FindDeltT = FindDeltT;
            pf.IE_Finding();
        }
        else if (E_FT == E_FindingType.Instant)
        {
            if (pf.Finding())
            {
                Debug.Log("<color=red>Find path success.</color>");
                pf.ShowPath(ShowPath);
            }
            else
            {
                Debug.Log("<color=red>Find path faile.</color>");
            }
        }
       


    }

    public void ShowPath(LinkE e)
    {
            GameObject go = GameObject.Instantiate(cube_red);
            go.transform.position = new Vector3(GetX(e.C),0,GetZ(e.R));        
    }


    private float GetX(int c)
    {
        float halfC = column / 2f;
        return c - halfC;
    }

    private float GetZ(int r)
    {
        float halfR = row / 2f;
        return halfR - r;
    }

    public GameObject UpdateFind(LinkE e )
    {
        GameObject go = GameObject.Instantiate(cube_green);
        go.transform.position = new Vector3(GetX(e.C), 0, GetZ(e.R));
        return go;
    }

}


#region 链表元素类

public class LinkE
{
    public int C;//列数
    public int R;//行数
    public LinkE Next = null;
    public LinkE Pre = null;

    public GameObject go;

    public LinkE() { }

    public LinkE(int r,int c)
    {
        this.R = r;
        this.C = c;
    }


    public LinkE(LinkE e,bool isNext = true)
    {
        if (isNext)
            this.Next = e;
        else
            this.Pre = e;
    }

    public bool EqualTo(LinkE otherE)
    {
        return this.R == otherE.R && this.C == otherE.C ;
    }

    public override string ToString()
    {
        return $"R:{R}  C:{C}";
    }
}
#endregion

public enum E_PathFind
{
    DFS =1,
    DFS_Stack =2,
    BFS = 3,
    AStar = 4,
    BStar = 5,
    Dijkstra = 6,
    SPFA = 7,
}

public enum E_FindingType
{
    Instant = 1,
    Gradually = 2,
}