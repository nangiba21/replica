using UnityEngine;
using System.Collections;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public GameObject target;

    [HideInInspector] public float launchForce = 1000f;   
    [HideInInspector] public float spawnInterval = 2.0f;

    float time;

    void Update()
    {
        if (target == null) return;

        time += Time.deltaTime;
        if (time >= spawnInterval)
        {
            time = 0;

            
            Vector3 targetPosition = target.transform.position + Vector3.up * 1.5f;
            transform.LookAt(targetPosition);

            Vector3 spawnPosition = transform.position + transform.forward * 2.0f;

            GameObject t = (GameObject)Instantiate(projectile, spawnPosition, Quaternion.identity);
            Destroy(t, 3);

            transform.LookAt(targetPosition);
            t.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce);
        }
    }
}