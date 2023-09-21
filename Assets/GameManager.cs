using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using jsontype;


public class GameManager : MonoBehaviour
{
    // max_x,max_yはjsonから入手したい
    // power_lineとかdistanceの配列の大きさとかは大きい値で初期化かも
    public GameObject jsonsettings;
    int max_x = 0;
    int max_y = 0;
    int sx = 0;
    int sy = 0;
    int gx = 0;
    int gy = 0;
    int[,,] power_line;
    int[,] distance;
    public bool[,] board;
   
    public GameObject block;
    public GameObject piece;
    public GameObject wire;
    public GameObject tile;
    public GameObject lightning;
    public GameObject lightningParent;
    public GameObject tileParent;
    public List<GameObject> pieceList;
    public List<Piece> pieceScriptList;
    public List<GameObject> wireList;
    public RawImage image;

    void initializePiece(data obj)
    {
        string imagePath = new string("Null");
        int block_cnt = 0;
        
        for(int i = 0;i<obj.pieces.Count;i++){
            int init_x = -3+i*2;
            int init_y = -3;

            pieceList.Add((GameObject)Instantiate(piece, new Vector3(init_x,init_y,0), Quaternion.identity));
            pieceList[i].name = "Mino" + i.ToString();
            pieceList[i].AddComponent<Piece>();
            pieceScriptList.Add(pieceList[i].GetComponent<Piece>());
            pieceScriptList[i].max_x = max_x;
            pieceScriptList[i].max_y = max_y;
            pieceScriptList[i].initialPosition = new Vector2(init_x, init_y);


            for (int j = 0; j < obj.pieces[i].cells.Count;j++){
                
                Vector3 pos = new Vector3(init_x+obj.pieces[i].cells[j].x,init_y+obj.pieces[i].cells[j].y,0.0f);
                pieceScriptList[i].cells.Add((GameObject)Instantiate(block, pos, Quaternion.identity));
                pieceScriptList[i].cells.Last().transform.parent = pieceList[i].transform;
                pieceScriptList[i].cells.Last().name = "Block" + (block_cnt + j).ToString();
                PieceCell pieceCellScript = pieceScriptList[i].cells.Last().GetComponent<PieceCell>();
                pieceScriptList[i].cellScripts.Add(pieceCellScript);

                imagePath = obj.pieces[i].cells[j].texture.ToString();
                //imagePath = imagePath.Replace(".png","");
                //Debug.Log(imagePath);
                
                String target = "Block"+(block_cnt+j).ToString()+"/Canvas/RawImage";
                RawImage image = GameObject.Find(target).GetComponent<RawImage>();

                image.texture = Resources.Load<Texture2D>(imagePath);

                for(int k = 0;k<obj.pieces[i].cells[j].wireInterface.Count;k++){
                    pieceCellScript.wireInterfase.Add(obj.pieces[i].cells[j].wireInterface[k]);
                    if (obj.pieces[i].cells[j].wireInterface[k]){
                        switch(k){
                            case 0:
                                //GameObject thumb = (GameObject)Instantiate(wire, pos , Quaternion.Euler(0f,0f,0f));
                                //wireList.Add(thumb);
                                //thumb.transform.parent = blockList[i-1].transform;
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, 0.25f, -0.05f), Quaternion.Euler(0f,0f,0f)));
                                break;
                            case 1:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0.25f, 0f, -0.05f), Quaternion.Euler(0f,0f,90f)));
                                break;
                            case 2:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, -0.25f, -0.05f), Quaternion.Euler(0f,0f,180f)));
                                break;
                            case 3:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(-0.25f,0, -0.05f), Quaternion.Euler(0f,0f,270f)));
                                break;
                            default:
                                break;
                        
                        }

                        wireList.Last().transform.parent = pieceScriptList[i].cells.Last().transform;
                    }
                }
            }
            block_cnt += obj.pieces[i].cells.Count;
        }
        //Debug.Log("ok");
                
    }

    void initializeMap(data obj)
    {
        max_x = obj.map.size.x;
        max_y = obj.map.size.y;
        sx = obj.map.start.x;
        sy = obj.map.start.y;
        gx = obj.map.goal.x;
        gy = obj.map.goal.y;

        GameObject start = (GameObject)Instantiate(tile, new Vector3(sx/2+0.5f, sy/2+0.5f, 1f), Quaternion.identity);
        start.transform.parent = tileParent.transform;
        start.name = "start";
        String target = "start" + "/Canvas/RawImage";
        RawImage image = GameObject.Find(target).GetComponent<RawImage>();
        image.texture = Resources.Load<Texture2D>("start");

        GameObject goal = (GameObject)Instantiate(tile, new Vector3(gx/2+0.5f, gy/2+0.5f, 1f), Quaternion.identity);
        goal.transform.parent = tileParent.transform;
        goal.name = "goal";
        target = "goal" + "/Canvas/RawImage";
        image = GameObject.Find(target).GetComponent<RawImage>();
        image.texture = Resources.Load<Texture2D>("goal");

        string imagePath = obj.map.tile.texture.ToString();
        for (int x = 0; x < max_x; x++)
        {
            for (int y = 0; y < max_y; y++)
            {
                GameObject ti = (GameObject)Instantiate(tile, new Vector3(x+0.5f,y+0.5f, 1f), Quaternion.identity);
                ti.transform.parent = tileParent.transform;
                ti.name = "tile" + "_" + x.ToString()+"_"+ y.ToString();
                target = "tile" + "_" + x.ToString() + "_" + y.ToString() + "/Canvas/RawImage";
                image = GameObject.Find(target).GetComponent<RawImage>();
                string path = imagePath + ((x + y) % 2).ToString();
                image.texture = Resources.Load<Texture2D>(path);
            }
        }
        power_line = new int[(max_x * 2 + 1),(max_y * 2 + 1), 4];
        distance = new int[(max_x * 2 + 1), (max_y * 2 + 1)];
        board = new bool[max_x,max_y];
        for (int x = 0; x < max_x; x++)
        {
            for (int y = 0; y < max_y; y++)
            {
                board[x, y] = false;
            }
        }
    }
    

    public void OnUpdate()
    {
        DeleteLightning();
        Bfs();
    }

    void DeleteLightning()
    {
        foreach (Transform child in lightningParent.transform)
        {
            Destroy(child.gameObject);
        }
    }


    void Bfs()
    {
        int[] vx = { 0, 1, 0, -1 };
        int[] vy = { 1, 0, -1, 0 };
        power_line = new int[(max_x * 2 + 1), (max_y * 2 + 1), 4];
        for (int x = 0; x < max_x * 2 + 1; x++)
        {
            for (int y = 0; y < max_y * 2 + 1; y++)
            {
                distance[x, y] = -1;
            }
        }
        foreach (Piece pi in pieceScriptList)
        {
            if (pi.dragging)
            {
                continue;
            }
            for(int i = 0; i < pi.cells.Count; i++)
            {
                int roundX = (int)pi.cells[i].transform.position.x;
                int roundY = (int)pi.cells[i].transform.position.y;
                if (roundX < 0 || roundX >= max_x || roundY < 0 || roundY >= max_y) break;
                for(int j = 0; j < 4; j++)
                {
                    if (pi.cellScripts[i].wireInterfase[(j+pi.rotate)%4])
                    {
                        power_line[roundX * 2 + 1, roundY * 2 + 1, j] = 1;
                        power_line[roundX * 2 + 1 + vx[j], roundY * 2 + 1 + vy[j], (j+2)%4] = 1;

                    }
                }
            }
        }

        distance[sx,sy] = 0;
        Queue<Tuple<int, int>> tq = new Queue<Tuple<int, int>>();
        tq.Enqueue(Tuple.Create(sx, sy));
        while (0 < tq.Count)
        {
            var q = tq.Dequeue();
            int x = q.Item1;
            int y = q.Item2;
            for (int i = 0; i < 4; i++)
            {
                int nx = x + vx[i];
                int ny = y + vy[i];
                if ((0 <= nx && nx <= max_x * 2) && (0 <= ny && ny <= max_y * 2) && power_line[x, y,i] == 1 && distance[nx, ny] == -1)
                {
                    GameObject bolt = (GameObject)Instantiate(lightning, new Vector3(0, 0, 0), Quaternion.identity);
                    bolt.transform.parent = lightningParent.transform;
                    DigitalRuby.LightningBolt.LightningBoltScript boltScript = bolt.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
                    boltScript.StartPosition = new Vector3(x/2f, y/2f, -3f);
                    boltScript.EndPosition = new Vector3(nx/2f, ny/2f, -3f);
                    distance[nx, ny] = distance[x, y] + 1;
                    tq.Enqueue(Tuple.Create(nx, ny));
                }
            }
        }
        if (distance[gx, gy] != -1)
        {
            Debug.Log("Clear");
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        data obj = jsonsettings.GetComponent<JsonSettings>().loadSettings();
        initializeMap(obj);
        initializePiece(obj);

        Debug.Log(obj.map.tile.texture);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
