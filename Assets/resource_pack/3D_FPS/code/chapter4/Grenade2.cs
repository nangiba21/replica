using UnityEngine;
using System.Collections;

public class Grenade2 : MonoBehaviour
{
    public float grenadeTimer;
    public bool grenadeTimerStatrted;
    public float grenadeTimerLimit;
    public bool grenadeExplode;
  
    private float radius = 5.0f;
    private float power = 500.0f;
    private float timer;
    private float explosionTime;
    private bool hasExploded;


    // Use this for initialization
    void Start()
    {
        timer = 0.0f; explosionTime = 2.0f;
        hasExploded = false;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= explosionTime)
        {
            if (hasExploded == false)
            {
                Vector3 explosionPos = gameObject.transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.GetComponent<Rigidbody>() != null && colliders[i].gameObject.tag != "Player")
                    {
                        GameObject objectTargeted = colliders[i].gameObject;
                        if (objectTargeted.tag == "enemy") objectTargeted.GetComponent<ManageNPC2>().gotHitByGrenade();
                    }
                }
                //GameObject.Instantiate (explosion, transform.position, Quaternion.identity);
                hasExploded = true;
                Destroy(gameObject);
            }

        }

    }
}