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
    int vertical_length = 4;
    int horizontal_length = 10;
    int sx = 0;
    int sy = 0;
    int[,,] power_line;
    int[,] distance;
    bool[,] board;
   
    public GameObject block;
    public GameObject piece;
    public GameObject wire;
    public List<GameObject> blockList;
    public List<GameObject> pieceList;
    public List<GameObject> wireList;
    public RawImage image;

    void initialize(data obj)
    {
        string imagePath = new string("Null");
        
        for(int i = 1;i<=obj.pieces.Count;i++){

            pieceList.Add((GameObject)Instantiate(piece, new Vector3((i-1)*5,0,0), Quaternion.identity));
            pieceList[i - 1].name = "Mino" + i.ToString();
            pieceList[i - 1].AddComponent<Piece>();

            for (int j = 1;j<=obj.pieces[i-1].cells.Count;j++){
                
                Vector3 pos = new Vector3(obj.pieces[i-1].cells[j-1].x,obj.pieces[i-1].cells[j-1].y,0.0f);
                blockList.Add((GameObject)Instantiate(block, pos, Quaternion.identity));
                blockList[j - 1].transform.parent = pieceList[i-1].transform;
                blockList[j - 1].name = "Block" + j.ToString();

                imagePath = obj.pieces[i-1].cells[j-1].texture.ToString();
                imagePath = imagePath.Replace(".png","");
                //Debug.Log(imagePath);
                
                String target = "Block"+j.ToString()+"/Canvas/RawImage";
                RawImage image = GameObject.Find(target).GetComponent<RawImage>();

                image.texture = Resources.Load<Texture2D>(imagePath);

                for(int k = 1;k<=obj.pieces[i-1].cells[j-1].wireInterface.Count;k++){
                    if(obj.pieces[i-1].cells[j-1].wireInterface[k-1]){
                        Debug.Log(k);
                        switch(k){
                            case 1:
                                //GameObject thumb = (GameObject)Instantiate(wire, pos , Quaternion.Euler(0f,0f,0f));
                                //wireList.Add(thumb);
                                //thumb.transform.parent = blockList[i-1].transform;
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, 0.25f, -1f), Quaternion.Euler(0f,0f,0f)));
                                break;
                            case 2:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0.25f, 0f, -1f), Quaternion.Euler(0f,0f,90f)));
                                break;
                            case 3:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, -0.25f, -1f), Quaternion.Euler(0f,0f,180f)));
                                break;
                            case 4:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(-0.25f, 0.25f, -1f), Quaternion.Euler(0f,0f,270f)));
                                break;
                            default:
                                break;
                        
                        }

                        wireList.Last().transform.parent = blockList[j - 1].transform;
                    }
                }
                
            }

            

        }
        //Debug.Log("ok");
                
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
        power_line = new int[(vertical_length * 2 + 1), (horizontal_length * 2 + 1), 4];
        distance = new int[vertical_length, horizontal_length];
        for (int i = 0; i < vertical_length * 2 + 1 ; i++)
        {
            for (int j = 0; j < horizontal_length * 2 + 1 ; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    power_line[i, j, k] = 0;
                }
            }
        }
        for (int x = 0; x < vertical_length; x++){
            for (int y = 0; y < horizontal_length; y++){
                distance[x, y] = -1;
            }
        }
        data obj = jsonsettings.GetComponent<JsonSettings>().loadSettings();
        initialize(obj);
        //デバッグに表示する。
        Debug.Log(obj.map.start.x);

        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
