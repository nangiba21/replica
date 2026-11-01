using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPCFSM : MonoBehaviour
{
    public Vector3 direction;
    public bool isInTheFieldOfView;
        public bool noObjectBetweenNPCAndPlayer = false;
    private GameObject gun;
    private float distance;
    private Animator anim;
    private Ray ray;
    private RaycastHit hit;
    private AnimatorStateInfo info;
    private string objectInSight;

    void Start()
    {
        
        Transform gunTransform = transform.Find("hand_gun");
        if (gunTransform != null)
        {
            gun = gunTransform.gameObject;
            gun.SetActive(false);
        }
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        direction = (GameObject.Find("FPSController").transform.position - transform.position).normalized;
        isInTheFieldOfView=(Vector3.Dot(transform.forward.normalized, direction) > 0.7);
        Debug.DrawRay(transform.position, direction * 100, Color.green);
        Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);
        if(Physics.Raycast(transform.position,direction*100,out hit))
        {
            if (hit.collider.gameObject.tag == "Player") noObjectBetweenNPCAndPlayer = true;
            else noObjectBetweenNPCAndPlayer=false;
        }
        if(noObjectBetweenNPCAndPlayer&&isInTheFieldOfView)
        {
            anim.SetBool("canSeePlayer", true);
            transform.LookAt(GameObject.Find("playerMiddle").transform);
        }
        else anim.SetBool("canSeePlayer", false);
        GameObject player = GameObject.Find("FPSController");
        if (player == null) return;

        distance = Vector3.Distance(transform.position, player.transform.position);
        bool withinReach = (distance < 1.5f);
        anim.SetBool("withinArmsReach", withinReach);

        if (Input.GetKeyDown(KeyCode.I)) anim.SetBool("canSeePlayer", true);
        if (Input.GetKeyDown(KeyCode.J)) anim.SetBool("canSeePlayer", false);

        ray.origin = transform.position + Vector3.up;
        ray.direction = transform.forward;
        info = anim.GetCurrentAnimatorStateInfo(0);
        
        objectInSight = "";
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit))
        {
            objectInSight = hit.collider.gameObject.tag;
            if (objectInSight == "Player")
            {
                anim.SetBool("canSeePlayer", true);
            }
        }

        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        
        if (info.IsName("IDLE"))
        {
            if (agent != null) agent.isStopped = true;
            if (gun != null) gun.SetActive(false);
        }
        else if (info.IsName("ATTACK_CLOSE_RANGE"))
        {
            if (agent != null) agent.isStopped = true;
            if (gun != null) gun.SetActive(false); 

            GameObject playerMiddle = GameObject.Find("playerMiddle");
            if (playerMiddle != null)
            {
                transform.LookAt(playerMiddle.transform);

               
                if (info.normalizedTime % 1.0f >= 0.90f && info.normalizedTime % 1.0f <= 0.99f)
                {
                    ManagePlayerHealth playerHealth = player.GetComponent<ManagePlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.decreaseHealth(5); 
                    }
                }
            }
        }
        else if (info.IsName("HIT"))
        {
            if (agent != null) agent.isStopped = true;
            if (gun != null) gun.SetActive(false);
        }
        else if (info.IsName("SHOOT"))
        {
            if (agent != null) agent.isStopped = true;
            if (gun != null) gun.SetActive(true);

            GameObject playerMiddle = GameObject.Find("playerMiddle");
            if (playerMiddle != null)
            {
                transform.LookAt(playerMiddle.transform);

                
                if (info.normalizedTime % 1.0f >= 0.90f && info.normalizedTime % 1.0f <= 0.99f)
                {
                    ManagePlayerHealth playerHealth = player.GetComponent<ManagePlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.decreaseHealth(5); 
                    }
                }
            }
        }
        else if (info.IsName("FOLLOW_PLAYER"))
        {
            if (gun != null) gun.SetActive(false);

            GameObject playerMiddle = GameObject.Find("playerMiddle");
            if (playerMiddle != null && agent != null)
            {
                agent.destination = playerMiddle.transform.position;
                agent.isStopped = false;
                transform.LookAt(playerMiddle.transform);
            }

            if (objectInSight != "Player" && distance > 3.0f)
            {
                anim.SetBool("canSeePlayer", false);
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime);
            }
        }
    }

    public void setGotHitParameter()
    {
        anim.SetTrigger("gotHit");
    }

    public void setLowHealthParameter()
    {
        anim.SetBool("lowHealth", true);
    }
}
