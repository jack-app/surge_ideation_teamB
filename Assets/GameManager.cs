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
    // vertical_length,horizontal_lengthはjsonから入手したい
    // power_lineとかdistanceの配列の大きさとかは大きい値で初期化かも
    public GameObject jsonsettings;
    int vertical_length = 0;
    int horizontal_length = 0;
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
    public List<GameObject> blockList;
    public List<GameObject> pieceList;
    public List<GameObject> wireList;
    public RawImage image;

    void initializePiece(data obj)
    {
        string imagePath = new string("Null");
        int block_cnt = 0;
        
        for(int i = 0;i<obj.pieces.Count;i++){
            int init_x = horizontal_length+3+(i%2)*4;
            int init_y = vertical_length-2-((int)i/2)*2;

            pieceList.Add((GameObject)Instantiate(piece, new Vector3(init_x,init_y,0), Quaternion.identity));
            pieceList[i].name = "Mino" + i.ToString();
            pieceList[i].AddComponent<Piece>();
            Piece pieceScript = pieceList[i].GetComponent<Piece>();
            pieceScript.max_x = horizontal_length;
            pieceScript.max_y = vertical_length;
            pieceScript.initialPosition = new Vector2(init_x, init_y);


            for (int j = 0; j < obj.pieces[i].cells.Count;j++){
                
                Vector3 pos = new Vector3(init_x+obj.pieces[i].cells[j].x,init_y+obj.pieces[i].cells[j].y,0.0f);
                blockList.Add((GameObject)Instantiate(block, pos, Quaternion.identity));
                blockList.Last().transform.parent = pieceList[i].transform;
                blockList.Last().name = "Block" + (block_cnt + j).ToString();
                PieceCell pieceCellScript = blockList.Last().GetComponent<PieceCell>();

                imagePath = obj.pieces[i].cells[j].texture.ToString();
                //imagePath = imagePath.Replace(".png","");
                //Debug.Log(imagePath);
                
                String target = "Block"+(block_cnt+j).ToString()+"/Canvas/RawImage";
                RawImage image = GameObject.Find(target).GetComponent<RawImage>();

                image.texture = Resources.Load<Texture2D>(imagePath);

                for(int k = 0;k<obj.pieces[i].cells[j].wireInterface.Count;k++){
                    pieceCellScript.wireInterfase.Add(obj.pieces[i].cells[j].wireInterface[k]);
                    if (obj.pieces[i].cells[j].wireInterface[k]){
                        Debug.Log(k);
                        switch(k){
                            case 0:
                                //GameObject thumb = (GameObject)Instantiate(wire, pos , Quaternion.Euler(0f,0f,0f));
                                //wireList.Add(thumb);
                                //thumb.transform.parent = blockList[i-1].transform;
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, 0.25f, -1f), Quaternion.Euler(0f,0f,0f)));
                                break;
                            case 1:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0.25f, 0f, -1f), Quaternion.Euler(0f,0f,90f)));
                                break;
                            case 2:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, -0.25f, -1f), Quaternion.Euler(0f,0f,180f)));
                                break;
                            case 3:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(-0.25f,0, -1f), Quaternion.Euler(0f,0f,270f)));
                                break;
                            default:
                                break;
                        
                        }

                        wireList.Last().transform.parent = blockList.Last().transform;
                    }
                }
            }
            block_cnt += obj.pieces[i].cells.Count;
        }
        //Debug.Log("ok");
                
    }

    void initializeMap(data obj)
    {
        horizontal_length = obj.map.size.x;
        vertical_length = obj.map.size.y;
        sx = obj.map.start.x;
        sy = obj.map.start.y;
        gx = obj.map.goal.x;
        gy = obj.map.goal.y;
        power_line = new int[(horizontal_length * 2 + 1),(vertical_length * 2 + 1), 4];
        distance = new int[horizontal_length,vertical_length];
        board = new bool[horizontal_length,vertical_length];
        for (int i = 0; i < horizontal_length * 2 + 1; i++)
        {
            for (int j = 0; j < vertical_length * 2 + 1; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    power_line[i, j, k] = 0;
                }
            }
        }
        for (int x = 0; x < horizontal_length; x++)
        {
            for (int y = 0; y < vertical_length; y++)
            {
                distance[x, y] = -1;
            }
        }
        for (int x = 0; x < horizontal_length; x++)
        {
            for (int y = 0; y < vertical_length; y++)
            {
                board[x, y] = false;
            }
        }
    }
    

    void OnUpdate()
    {

    }

    void Bfs()
    {
        Queue<Tuple<int, int>> tq = new Queue<Tuple<int, int>>();
        tq.Enqueue(Tuple.Create(sx, sy));
        int[] vx = { -1, 0, 1, 0 };
        int[] vy = { 0, 1, 0, -1 };
        while (0 < tq.Count)
        {
            var q = tq.Dequeue();
            int x = q.Item1;
            int y = q.Item2;

            for (int i = 0; i < 4; i++)
            {
                int nx = x + vx[i];
                int ny = y + vy[i];
                if ((0 <= nx && nx <= vertical_length * 2) && (0 <= ny && ny <= horizontal_length * 2 + 1) && power_line[nx, ny,i] == 1 && distance[nx, ny] == -1)
                {
                    distance[nx, ny] = distance[x, y] + 1;
                    tq.Enqueue(Tuple.Create(nx, ny));
                }
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        data obj = jsonsettings.GetComponent<JsonSettings>().loadSettings();
        initializeMap(obj);
        initializePiece(obj);
        //デバッグに表示する。
        Debug.Log(obj.map.start.x);

        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
