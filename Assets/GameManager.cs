using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
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
    public GameObject explosion;
    public List<GameObject> pieceList;
    public List<Piece> pieceScriptList;
    public List<GameObject> wireList;
    public List<GameObject> elecList;
    public List<Piece> elecScriptList;
    public RawImage image;
    public List<Tuple<int, int>> elecposList = new List<Tuple<int, int>>();
    public Camera mainCamera;

    void initializePiece(data obj)
    {
        string imagePath = new string("Null");
        int block_cnt = 0;

        for (int i = 0; i < obj.pieces.Count; i++) {
            //sssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            max_x = obj.map.size.x;
            max_y = obj.map.size.y;

            float ratio = Math.Max(Math.Max(max_x/20f,max_y/8f),1f);

            //float init_x = -3 + i * 2;
            float init_x = max_x * 0.5f + (i - obj.pieces.Count/2+2f)*2*ratio;

            //float init_y = -3;
            float init_y = max_y/2 - 5 * ratio;

            float init_z = -(0.1f * i);
            if (obj.pieces[i].type == "electronics")
            {
                init_x = 0.5f;
                init_y = 0.5f;
                init_z = -1;
                elecposList.Add(Tuple.Create(obj.pieces[i].cells[0].x, obj.pieces[i].cells[0].y));
            }

            pieceList.Add((GameObject)Instantiate(piece, new Vector3(init_x, init_y, init_z), Quaternion.identity));
            pieceList[i].name = "Mino" + i.ToString();
            pieceList[i].AddComponent<Piece>();
            pieceScriptList.Add(pieceList[i].GetComponent<Piece>());
            pieceScriptList[i].max_x = max_x;
            pieceScriptList[i].max_y = max_y;
            pieceScriptList[i].initialPosition = new Vector2(init_x, init_y);
            if (obj.pieces[i].type == "electronics")
            {
                pieceScriptList[i].canDrug = false;
                board[obj.pieces[i].cells[0].x, obj.pieces[i].cells[0].y] = true;
            }


            for (int j = 0; j < obj.pieces[i].cells.Count; j++) {

                Vector3 pos = new Vector3(init_x + obj.pieces[i].cells[j].x, init_y + obj.pieces[i].cells[j].y, 0);
                pieceScriptList[i].cells.Add((GameObject)Instantiate(block, pos, Quaternion.identity));
                pieceScriptList[i].cells.Last().transform.parent = pieceList[i].transform;
                pieceScriptList[i].cells.Last().name = "Block" + (block_cnt + j).ToString();
                PieceCell pieceCellScript = pieceScriptList[i].cells.Last().GetComponent<PieceCell>();
                pieceScriptList[i].cellScripts.Add(pieceCellScript);

                imagePath = obj.pieces[i].cells[j].texture.ToString();
                //imagePath = imagePath.Replace(".png","");
                //Debug.Log(imagePath);

                String target = "Block" + (block_cnt + j).ToString() + "/Canvas/RawImage";
                RawImage image = GameObject.Find(target).GetComponent<RawImage>();

                image.texture = Resources.Load<Texture2D>(imagePath);

                for (int k = 0; k < obj.pieces[i].cells[j].wireInterface.Count; k++) {
                    pieceCellScript.wireInterfase.Add(obj.pieces[i].cells[j].wireInterface[k]);
                    if (obj.pieces[i].cells[j].wireInterface[k] && obj.pieces[i].type == "mino")
                    {
                        switch (k) {
                            case 0:
                                //GameObject thumb = (GameObject)Instantiate(wire, pos , Quaternion.Euler(0f,0f,0f));
                                //wireList.Add(thumb);
                                //thumb.transform.parent = blockList[i-1].transform;
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, 0.25f, -0.05f), Quaternion.Euler(0f, 0f, 0f)));
                                break;
                            case 1:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0.25f, 0f, -0.05f), Quaternion.Euler(0f, 0f, 90f)));
                                break;
                            case 2:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(0f, -0.25f, -0.05f), Quaternion.Euler(0f, 0f, 180f)));
                                break;
                            case 3:
                                wireList.Add((GameObject)Instantiate(wire, pos + new Vector3(-0.25f, 0, -0.05f), Quaternion.Euler(0f, 0f, 270f)));
                                break;
                            default:
                                break;

                        }

                        wireList.Last().transform.parent = pieceScriptList[i].cells.Last().transform;
                    }
                }
            }
            block_cnt += obj.pieces[i].cells.Count;
            if (obj.pieces[i].type == "mino")
            {
                pieceScriptList[i].MoveToInitialPosition();
            }
        }
    }

    void initializeMap(data obj)
    {
        max_x = obj.map.size.x;
        max_y = obj.map.size.y;
        sx = obj.map.start.x;
        sy = obj.map.start.y;
        gx = obj.map.goal.x;
        gy = obj.map.goal.y;
        power_line = new int[(max_x * 2 + 1), (max_y * 2 + 1), 4];
        distance = new int[(max_x * 2 + 1), (max_y * 2 + 1)];
        board = new bool[max_x, max_y];
        for (int x = 0; x < max_x; x++)
        {
            for (int y = 0; y < max_y; y++)
            {
                board[x, y] = false;
            }
        }

        GameObject start = (GameObject)Instantiate(tile, new Vector3(sx / 2 + 0.5f, sy / 2 + 0.5f, 1f), Quaternion.identity);
        start.transform.parent = tileParent.transform;
        start.name = "start";
        String target = "start" + "/Canvas/RawImage";
        RawImage image = GameObject.Find(target).GetComponent<RawImage>();
        image.texture = Resources.Load<Texture2D>("start");

        GameObject goal = (GameObject)Instantiate(tile, new Vector3(gx / 2 + 0.5f, gy / 2 + 0.5f, 1f), Quaternion.identity);
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
                GameObject ti = (GameObject)Instantiate(tile, new Vector3(x + 0.5f, y + 0.5f, 1f), Quaternion.identity);
                ti.transform.parent = tileParent.transform;
                ti.name = "tile" + "_" + x.ToString() + "_" + y.ToString();
                target = "tile" + "_" + x.ToString() + "_" + y.ToString() + "/Canvas/RawImage";
                image = GameObject.Find(target).GetComponent<RawImage>();
                string path = imagePath + ((x + y) % 2).ToString();
                image.texture = Resources.Load<Texture2D>(path);
            }
        }

        float ratio = Math.Max(Math.Max(max_x/20f,max_y/8f),1f);
        mainCamera.gameObject.transform.position = new Vector3(max_x/2f,max_y/2f+1f,-10f);
        mainCamera.orthographicSize = 18*ratio;
        //Image backimage = GameObject.Find("Main Camera/Images/カミナリくん　全体イメージ_4").GetComponent<Image>();
        //backimage.rectTransform.scale = new Vector2(ratio*backimage.rectTransform.scale.x,ratio*backimage.rectTransform.scale.y);
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
            for (int i = 0; i < pi.cells.Count; i++)
            {
                int roundX = (int)pi.cells[i].transform.position.x;
                int roundY = (int)pi.cells[i].transform.position.y;
                if (roundX < 0 || roundX >= max_x || roundY < 0 || roundY >= max_y) break;
                for (int j = 0; j < 4; j++)
                {
                    if (pi.cellScripts[i].wireInterfase[(j + pi.rotate) % 4])
                    {
                        power_line[roundX * 2 + 1, roundY * 2 + 1, j] = 1;
                        power_line[roundX * 2 + 1 + vx[j], roundY * 2 + 1 + vy[j], (j + 2) % 4] = 1;

                    }
                }
            }
        }

        distance[sx, sy] = 0;
        Queue<Tuple<int, int>> tq = new Queue<Tuple<int, int>>();
        tq.Enqueue(Tuple.Create(sx, sy));
        bool clear = true;
        while (0 < tq.Count)
        {
            var q = tq.Dequeue();
            int x = q.Item1;
            int y = q.Item2;
            for (int i = 0; i < 4; i++)
            {
                int nx = x + vx[i];
                int ny = y + vy[i];
                if ((0 <= nx && nx <= max_x * 2) && (0 <= ny && ny <= max_y * 2) && power_line[x, y, i] == 1 && distance[nx, ny] == -1)
                {
                    GameObject bolt = (GameObject)Instantiate(lightning, new Vector3(0, 0, 0), Quaternion.identity);
                    bolt.transform.parent = lightningParent.transform;
                    DigitalRuby.LightningBolt.LightningBoltScript boltScript = bolt.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
                    boltScript.StartPosition = new Vector3(x / 2f, y / 2f, -2f);
                    boltScript.EndPosition = new Vector3(nx / 2f, ny / 2f, -2f);
                    distance[nx, ny] = distance[x, y] + 1;
                    tq.Enqueue(Tuple.Create(nx, ny));
                }
            }
        }
        for (int i = 0; i < elecposList.Count; i++)
        {
            if (distance[elecposList[i].Item1 * 2 + 1, elecposList[i].Item2 * 2 + 1] == -1)
            {
                clear = false;
            }
        }
        if (distance[gx, gy] != -1 && clear)
        {
            Debug.Log("Clear");
            for (int i = 0; i < elecposList.Count; i++)
            {
                Instantiate(explosion, new Vector3(elecposList[i].Item1 + 0.5f, elecposList[i].Item2 + 0.5f, -20f), Quaternion.identity);
            }
            StartCoroutine(SceneChange());
        }
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("ResultScene",LoadSceneMode.Additive);
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
