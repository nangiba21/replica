using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageWeapons : MonoBehaviour
{
    private const int WEAPON_GUN = 0;
    private const int WEAPON_AUTO_GUN = 1;
    private const int WEAPON_GRENADE = 2;

    private float timer;
    private bool timerStarted;
    private bool canShoot = true;
    private int currentWeapon = WEAPON_GUN;

    private int[] ammos;
    private int[] maxAmmos;
    private int[] baseMaxAmmos;
    private float[] reloadTime;
    private float[] baseReloadTimes;
    private string[] weaponName;

    Camera playersCamera;
    Ray rayFromPlayer;
    RaycastHit hit;

    public GameObject grenade;

    
    public Text userInfoText;
    public Text gunAmmoText;
    public Text healthText;
    public Text maxAmmoInfoText;
    public Text pauseInfoText;
    public Dropdown maxAmmoDropdown;
    public Dropdown reloadDropdown;

    private bool isPaused = true;
    private int playerHealth = 100;

    void Start()
    {
        ammos = new int[3];
        maxAmmos = new int[3];
        baseMaxAmmos = new int[3];
        reloadTime = new float[3];
        baseReloadTimes = new float[3];
        weaponName = new string[3];

        weaponName[WEAPON_GUN] = "GUN";
        weaponName[WEAPON_AUTO_GUN] = "AUTOMATIC GUN";
        weaponName[WEAPON_GRENADE] = "GRENADE";

       
        ammos[WEAPON_GUN] = 10;
        ammos[WEAPON_AUTO_GUN] = 10;
        ammos[WEAPON_GRENADE] = 10;

        
        baseMaxAmmos[WEAPON_GUN] = 20;
        baseMaxAmmos[WEAPON_AUTO_GUN] = 20;
        baseMaxAmmos[WEAPON_GRENADE] = 10;

        maxAmmos[WEAPON_GUN] = baseMaxAmmos[WEAPON_GUN];
        maxAmmos[WEAPON_AUTO_GUN] = baseMaxAmmos[WEAPON_AUTO_GUN];
        maxAmmos[WEAPON_GRENADE] = baseMaxAmmos[WEAPON_GRENADE];

        
        baseReloadTimes[WEAPON_GUN] = 2.0f;
        baseReloadTimes[WEAPON_AUTO_GUN] = 0.5f;
        baseReloadTimes[WEAPON_GRENADE] = 3.0f;

        reloadTime[WEAPON_GUN] = baseReloadTimes[WEAPON_GUN];
        reloadTime[WEAPON_AUTO_GUN] = baseReloadTimes[WEAPON_AUTO_GUN];
        reloadTime[WEAPON_GRENADE] = baseReloadTimes[WEAPON_GRENADE];

        playersCamera = GetComponent<Camera>();

       
        Time.timeScale = 0f;
        isPaused = true;
        if (pauseInfoText != null)
            pauseInfoText.text = "Game Paused - Press ESC or P to Start";
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

        
        HandleUIPreferences();
        UpdateUIDisplays();

        
        if (isPaused) return;

        if (timerStarted)
        {
            timer += Time.deltaTime;
            if (timer >= reloadTime[currentWeapon])
            {
                timerStarted = false;
                canShoot = true;
            }
        }

        if (playersCamera != null)
        {
            rayFromPlayer = playersCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Debug.DrawRay(rayFromPlayer.origin, rayFromPlayer.direction * 100, Color.red);
        }

        
        if (Input.GetKey(KeyCode.F))
        {
            if ((currentWeapon == WEAPON_GUN || currentWeapon == WEAPON_AUTO_GUN) && ammos[currentWeapon] >= 1 && canShoot)
            {
                ammos[currentWeapon]--;
                if (GetComponent<AudioSource>() != null)
                    GetComponent<AudioSource>().Play();

                if (Physics.Raycast(rayFromPlayer, out hit, 100))
                {
                    if (hit.collider.gameObject.tag == "target")
                    {
                        hit.collider.gameObject.GetComponent<ManageNPC>().gotHit();
                    }
                }
                canShoot = false;
                timer = 0.0f;
                timerStarted = true;
            }

            if (currentWeapon == WEAPON_GRENADE && ammos[WEAPON_GRENADE] >= 1 && canShoot)
            {
                ammos[currentWeapon]--;

                Vector3 spawnPos;
                Quaternion spawnRot;
                GameObject launcher = GameObject.Find("launcher");
                if (launcher != null)
                {
                    spawnPos = launcher.transform.position;
                    spawnRot = launcher.transform.rotation;
                }
                else
                {
                    spawnPos = transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;
                    spawnRot = transform.rotation;
                }

                if (grenade != null)
                {
                    GameObject grenadeF = (GameObject)Instantiate(grenade, spawnPos, spawnRot);
                    Rigidbody rb = grenadeF.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearDamping = 0.05f; 
                        rb.AddForce(transform.forward * 300f, ForceMode.Impulse);
                    }
                }

                canShoot = false;
                timer = 0.0f;
                timerStarted = true;
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeapon = (currentWeapon + 1) % 3;
            print("Current Weapon: " + weaponName[currentWeapon] + " (" + ammos[currentWeapon] + ")");
        }
    }

    void HandleUIPreferences()
    {
        
        if (maxAmmoDropdown != null)
        {
            int multiplier = 2;
            if (maxAmmoDropdown.value == 1) multiplier = 5;
            else if (maxAmmoDropdown.value >= 2) multiplier = 10;

            for (int i = 0; i < 3; i++)
            {
                maxAmmos[i] = baseMaxAmmos[i] * multiplier;
            }
        }

        
        if (reloadDropdown != null)
        {
            float timeMultiplier = 1.0f;
            if (reloadDropdown.value == 0) timeMultiplier = 0.5f;
            else if (reloadDropdown.value == 1) timeMultiplier = 1.0f;
            else if (reloadDropdown.value >= 2) timeMultiplier = 2.0f;

            for (int i = 0; i < 3; i++)
            {
                reloadTime[i] = baseReloadTimes[i] * timeMultiplier;
            }
        }
    }

    void UpdateUIDisplays()
    {
        if (userInfoText != null)
            userInfoText.text = weaponName[currentWeapon] + " (" + ammos[currentWeapon] + "/" + maxAmmos[currentWeapon] + ")";

        if (gunAmmoText != null)
            gunAmmoText.text = "Gun Ammo: " + ammos[WEAPON_GUN] + " / " + maxAmmos[WEAPON_GUN];

        if (healthText != null)
            healthText.text = "Health: " + playerHealth;

        if (maxAmmoInfoText != null)
            maxAmmoInfoText.text = "Max Ammos -> Gun: " + maxAmmos[WEAPON_GUN] + " | Auto: " + maxAmmos[WEAPON_AUTO_GUN] + " | Grenade: " + maxAmmos[WEAPON_GRENADE];
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseInfoText != null)
        {
            pauseInfoText.text = isPaused ? "Game Paused - Press ESC or P to Resume" : "";
        }
    }

    public void manageCollisions(ControllerColliderHit hit)
    {
        string tagOfTheOtherObject = hit.collider.gameObject.tag;
        if (tagOfTheOtherObject == "ammo_gun" || tagOfTheOtherObject == "ammo_automatic_gun" || tagOfTheOtherObject == "ammo_grenade")
        {
            int indexOfAmmoBeingUpdated = 0;
            if (tagOfTheOtherObject == "ammo_gun") indexOfAmmoBeingUpdated = WEAPON_GUN;
            if (tagOfTheOtherObject == "ammo_automatic_gun") indexOfAmmoBeingUpdated = WEAPON_AUTO_GUN;
            if (tagOfTheOtherObject == "ammo_grenade") indexOfAmmoBeingUpdated = WEAPON_GRENADE;

            
            ammos[indexOfAmmoBeingUpdated] += 5;

            Destroy(hit.collider.gameObject);
        }
    }
}