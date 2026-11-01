using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageWeapons2 : MonoBehaviour
{
    private const int WEAPON_GUN = 0;
    private const int WEAPON_AUTO_GUN = 1;
    private const int WEAPON_GRENADE = 2;
    private int score;
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
        score = 0;
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
        if (playersCamera == null) playersCamera = GetComponentInChildren<Camera>();
        if (playersCamera == null) playersCamera = Camera.main;

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

                if (playersCamera != null && Physics.Raycast(rayFromPlayer, out hit, 100))
                {
                    print("Raycast hit: " + hit.collider.gameObject.name);
                    ManageNPC2 npcHealth = hit.collider.gameObject.GetComponent<ManageNPC2>();
                    if (npcHealth == null)
                    {
                        npcHealth = hit.collider.gameObject.GetComponentInParent<ManageNPC2>();
                    }

                    if (npcHealth != null)
                    {
                        if (currentWeapon == WEAPON_AUTO_GUN)
                        {
                            npcHealth.gotHitByAutoGun();
                        }
                        else
                        {
                            npcHealth.gotHit();
                        }
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
                if (baseMaxAmmos != null && maxAmmos != null && i < baseMaxAmmos.Length && i < maxAmmos.Length)
                {
                    maxAmmos[i] = baseMaxAmmos[i] * multiplier;
                }
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
                if (baseReloadTimes != null && reloadTime != null && i < baseReloadTimes.Length && i < reloadTime.Length)
                {
                    reloadTime[i] = baseReloadTimes[i] * timeMultiplier;
                }
            }
        }
    }

    void UpdateUIDisplays()
    {
     
        GameObject userInfoObj = GameObject.Find("userInfo");
        if (userInfoObj != null)
        {
            Text t = userInfoObj.GetComponent<Text>();
            if (t != null && weaponName != null && ammos != null && maxAmmos != null)
            {
                t.text = weaponName[currentWeapon] + " (" + ammos[currentWeapon] + "/" + maxAmmos[currentWeapon] + ")";
            }
        }
        else if (userInfoText != null)
        {
            userInfoText.text = weaponName[currentWeapon] + " (" + ammos[currentWeapon] + "/" + maxAmmos[currentWeapon] + ")";
        }

        // Update Gun Ammo Text dynamically
        GameObject gunAmmoObj = GameObject.Find("gunammo");
        if (gunAmmoObj != null)
        {
            Text t = gunAmmoObj.GetComponent<Text>();
            if (t != null && ammos != null && maxAmmos != null)
            {
                t.text = "Gun Ammo: " + ammos[WEAPON_GUN] + " / " + maxAmmos[WEAPON_GUN];
            }
        }
        else if (gunAmmoText != null)
        {
            gunAmmoText.text = "Gun Ammo: " + ammos[WEAPON_GUN] + " / " + maxAmmos[WEAPON_GUN];
        }

        // Update Health Text dynamically
        GameObject healthObj = GameObject.Find("healthInfo");
        if (healthObj != null)
        {
            Text t = healthObj.GetComponent<Text>();
            if (t != null)
            {
                t.text = "Health: " + playerHealth;
            }
        }
        else if (healthText != null)
        {
            healthText.text = "Health: " + playerHealth;
        }

        GameObject maxAmmoInfoObj = GameObject.Find("maxammo");
        if (maxAmmoInfoObj != null)
        {
            Text t = maxAmmoInfoObj.GetComponent<Text>();
            if (t != null && maxAmmos != null)
            {
                t.text = "Max Ammos -> Gun: " + maxAmmos[WEAPON_GUN] + " | Auto: " + maxAmmos[WEAPON_AUTO_GUN] + " | Grenade: " + maxAmmos[WEAPON_GRENADE];
            }
        }
        else if (maxAmmoInfoText != null)
        {
            maxAmmoInfoText.text = "Max Ammos -> Gun: " + maxAmmos[WEAPON_GUN] + " | Auto: " + maxAmmos[WEAPON_AUTO_GUN] + " | Grenade: " + maxAmmos[WEAPON_GRENADE];
        }

     
        GameObject pauseObj = GameObject.Find("final");
        if (pauseObj == null) pauseObj = GameObject.Find("pause");
        if (pauseObj != null)
        {
            Text t = pauseObj.GetComponent<Text>();
            if (t != null)
            {
                t.text = isPaused ? "Game Paused - Press ESC or P to Start" : "";
            }
        }
        else if (pauseInfoText != null)
        {
            pauseInfoText.text = isPaused ? "Game Paused - Press ESC or P to Start" : "";
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseInfoText != null)
        {
            pauseInfoText.text = isPaused ? "Game Paused - Press ESC or Button to Start" : "";
        }
    }

    public void manageCollisions(ControllerColliderHit hit)
    {
        if (hit == null || hit.collider == null) return;

        string tagOfTheOtherObject = hit.collider.gameObject.tag;

        if (tagOfTheOtherObject == "ammo_gun" || tagOfTheOtherObject == "ammo_automatic_gun" || tagOfTheOtherObject == "ammo_grenade")
        {
            int indexOfAmmoBeingUpdated = 0;
            if (tagOfTheOtherObject == "ammo_gun") indexOfAmmoBeingUpdated = WEAPON_GUN;
            else if (tagOfTheOtherObject == "ammo_automatic_gun") indexOfAmmoBeingUpdated = WEAPON_AUTO_GUN;
            else if (tagOfTheOtherObject == "ammo_grenade") indexOfAmmoBeingUpdated = WEAPON_GRENADE;

            if (ammos != null && indexOfAmmoBeingUpdated < ammos.Length)
            {
                ammos[indexOfAmmoBeingUpdated] += 5;
            }

            if (hit.collider.gameObject != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else if (tagOfTheOtherObject == "health_pack")
        {
            GameObject player = GameObject.Find("FPSController");
            if (player != null)
            {
                ManagePlayerHealth playerHealth = player.GetComponent<ManagePlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.increaseHealth(50);
                }
            }

            if (hit.collider.gameObject != null)
            {
                Destroy(hit.collider.gameObject);
            }
        }
        if (tagOfTheOtherObject == "pick_me")
        {
            Destroy(hit.collider.gameObject);
            score++;
            if (score >= 1)
                SceneManager.LoadScene("winScene");
        }
    }
}