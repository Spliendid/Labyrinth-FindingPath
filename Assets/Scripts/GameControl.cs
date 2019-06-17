using UnityEngine;
using System.IO;
using System.Threading;
public class GameControl : MonoBehaviour
{
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
        pf = new BFS(Map,start,end);
        if (pf.Finding())
        {
            Debug.Log("Finding success");
            ShowPath();
        }
        else
        {
            Debug.Log("Finding Faild");
        }


    }

    private void ShowPath()
    {
        LinkE current = new LinkE(start);
        while (current.Next!=null)
        {
            GameObject go = GameObject.Instantiate(cube_red);
            go.transform.position = new Vector3(GetX(current.Next.C),0,GetZ(current.Next.R));
            current = current.Next;
        }
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

    private void UpdateFind(GameObject go,LinkE linkE ,bool bo)
    {

    }

}


#region 链表元素类

public class LinkE
{
    public int C;//列数
    public int R;//行数
    public LinkE Next = null;

    public LinkE() { }

    public LinkE(int r,int c)
    {
        this.R = r;
        this.C = c;
        this.Next = null;
    }

    public LinkE(LinkE next)
    {
        this.Next = next;
    }

    public bool CompareTo(LinkE otherE)
    {
        return this.R == otherE.R && this.C == otherE.C ;
    }
}
#endregion