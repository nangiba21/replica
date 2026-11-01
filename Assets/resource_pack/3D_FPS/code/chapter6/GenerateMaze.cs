using UnityEngine;
using System.Collections;

public class GenerateMaze : MonoBehaviour
{
    public GameObject wall, player, npc_guard;
    public GameObject healthPack, ammoGun, collectItem;

   
    private int[,] worldMap = new int[,]
    {
        {1,1,1,1,1,1,1,1,1,1},
        {1,2,0,0,0,0,0,0,3,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,1,1,0,0,0,0,1}, 
        {1,0,0,1,1,0,0,4,0,1}, 
        {1,0,0,0,0,0,0,5,0,1}, 
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,6,0,0,0,0,0,1}, 
        {1,3,0,0,0,0,0,0,3,1},
        {1,1,1,1,1,1,1,1,1,1},
    };

    void Start()
    {
        int i, j;
        for (i = 0; i < 10; i++)
        {
            for (j = 0; j < 10; j++)
            {
                Vector3 spawnPos = new Vector3(50 - i * 10, 1.5f, 50 - j * 10);
                GameObject t = null;

                if (worldMap[i, j] == 1)
                {
                    t = (GameObject)Instantiate(wall, spawnPos, Quaternion.identity);
                }
                else if (worldMap[i, j] == 2)
                {
                    t = (GameObject)Instantiate(player, spawnPos, Quaternion.identity);
                    t.name = "FPSController";
                }
                else if (worldMap[i, j] == 3)
                {
                    spawnPos.y = 0.5f; 
                    t = (GameObject)Instantiate(npc_guard, spawnPos, Quaternion.identity);
                }
                else if (worldMap[i, j] == 4 && collectItem != null)
                {
                    spawnPos.y = 1.0f;
                    t = (GameObject)Instantiate(collectItem, spawnPos, Quaternion.identity);
                    t.tag = "pick_me";
                }
                else if (worldMap[i, j] == 5 && healthPack != null)
                {
                    spawnPos.y = 1.0f;
                    t = (GameObject)Instantiate(healthPack, spawnPos, Quaternion.identity);
                    t.tag = "health_pack";
                }
                else if (worldMap[i, j] == 6 && ammoGun != null)
                {
                    spawnPos.y = 1.0f;
                    t = (GameObject)Instantiate(ammoGun, spawnPos, Quaternion.identity);
                    t.tag = "ammo_gun";
                }
            }
        }
    }
}