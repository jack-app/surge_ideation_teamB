using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

public class GameManager : MonoBehaviour
{
    int vertical_length = 4, horizontal_length = 10;
    int[,] power_line = new int[(vertical_length * 2 + 1) * (horizontal_length * 2 + 1),0];
    Array.Initialize(power_line, 0);
    int[,] map = new int[vertical_length, horizontal_length]
    Array.Initialize(map, 0);

    int[] Bfs()
    {
        int[] dist = new int[(vertical_length * 2 + 1) * (horizontal_length * 2 + 1)]
        Array.Initialize(map, -1);
        Array.Initialize(check, 0);
        var que = new Queue<int>();
        que.Enqueue(0);
        while (!que.Count > 0)
        {
            int q = que.Peek(); que.Dequeue();
            for (auto nq: power_line[q])
            {
                if (dist[nq] == -1)
                {
                    dist[nq] = dist[q] + 1;
                    que.push(nq);
                }
            }
        }
        return dist;
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期配置に応じて家電を配置



    }

    // Update is called once per frame
    void Update()
    {

    }
}
