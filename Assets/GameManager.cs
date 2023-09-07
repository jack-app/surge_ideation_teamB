using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

public class GameManager : MonoBehaviour
{
    // vertical_length,horizontal_lengthはjsonから入手したい
    // power_lineとかdistanceの配列の大きさとかは大きい値で初期化かも
    int vertical_length = 4;
    int horizontal_length = 10;
    int sx = 0;
    int sy = 0;
    int[,,] power_line;
    int[,] distance;
    bool[,] board;

    void initialize()
    {

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
        initialize();

        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
