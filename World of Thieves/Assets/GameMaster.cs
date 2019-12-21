using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class GameMaster : MonoBehaviour {

    public GameObject IngameMenu;
    public Scrollbar VolumeSlider;
    public Toggle WindowToggle;
    public Dropdown ResolutionDropdown;

    public GameObject PlayerObj;
    public GameObject SlimeObj;
    public Text DeathMessage;
    public static GameObject Player;
    public static GameObject Slime;

    public Transform SpawnLocation;
    public Transform BossSpawnLocation;

    public GameObject TextMeshNotification;
    private static GameObject textMeshNotification;
    private static float textMeshNotificationCounter = 0;

    private const string savePath = @"Saves\save.dat";

    public static bool IsMenuOn { get; private set; } = false;
    private static int bossesBeaten = 0;
    public static int BossesBeaten {
        get { return bossesBeaten; }
        set {
            if (value > bossesBeaten)
                bossesBeaten = value;
        }
    }

    [HideInInspector]
    public static GameObject CurrentLavaRock;

    [HideInInspector]
    public readonly static List<Action> OnReset = new List<Action>();
    private readonly static List<Tuple<string, string>> SavedOrbsInfo = new List<Tuple<string, string>>();
    [HideInInspector]
    public readonly static List<Action> Loop = new List<Action>();

    private void Awake() {
        OnReset.Clear();
        SavedOrbsInfo.Clear();
    }
    private void Start() {
        textMeshNotification = Instantiate(TextMeshNotification, Camera.main.transform);

        InstantiatePlayer();

        InstantiateSlime();

        IngameMenu.SetActive(false);
        IsMenuOn = false;
        VolumeSlider.value = GameSettings.MasterVolume;
        WindowToggle.isOn = !Screen.fullScreen;
        for (int i = 0; i < ResolutionDropdown.options.Count; i++)
            if (ResolutionDropdown.options[i].text.Contains(Screen.width.ToString())) {
                ResolutionDropdown.value = i;
                return;
            }

    }

    private void Update() {
        if (Loop.Count > 0)
            for (int i = Loop.Count-1; i >=0; i--)
                try {
                    Loop[i]();
                } catch (Exception) {
                    Loop.RemoveAt(i);
                }
        //if (Player == null)
          //  GameObject.Find("Player");
        if (Player == null) {
            if (DeathMessage.enabled == false)
                DeathMessage.enabled = true;
        }else {
            if (DeathMessage.enabled == true)
                DeathMessage.enabled = false;
        }

        if ( Player == null && Input.GetKeyDown(KeyCode.M)) {
            InstantiatePlayer();
            Destroy(Slime);
            InstantiateSlime();

            foreach (var slimeSplash in FindObjectsOfType<SlimeSplashControl>())
                Destroy(slimeSplash.gameObject);

            foreach (var action in OnReset) {
                action();
            }
        }

        if (Input.GetKeyDown(KeyCode.N)) {
            if (Player != null)
                Destroy(Player);

            if (SceneManager.GetActiveScene().name == "SlimeBossRoom1")
                SceneManager.LoadScene("SlimeBossRoom2");
            if (SceneManager.GetActiveScene().name == "SlimeBossRoom2")
                SceneManager.LoadScene("SlimeBossRoom3");
            if (SceneManager.GetActiveScene().name == "SlimeBossRoom3")
                SceneManager.LoadScene("SlimeBossRoom1");
        }

        MessageLoop();


        // INGAME MENU
        if (Input.GetKeyDown(KeyCode.Escape)) {
            IngameMenu.SetActive(!IngameMenu.activeSelf);
            IsMenuOn = !IsMenuOn;
        }

    }
    private void InstantiatePlayer() {
        Player = Instantiate(PlayerObj);
        Player.transform.position = new Vector2(SpawnLocation.position.x, SpawnLocation.position.y);
    }
    private void InstantiateSlime() {
        Slime = Instantiate(SlimeObj);
        Slime.transform.position = new Vector2(BossSpawnLocation.position.x, BossSpawnLocation.position.y);
        if (SceneManager.GetActiveScene().name == "TutorialRoom") {
            Slime.GetComponent<DamageManager>().MaxHealth = 10000f;
            Slime.GetComponent<DamageManager>().Health = 10000f;
        }
    }

    public static void SaveOrbs(List<GameObject> orbs) {
        SavedOrbsInfo.Clear();

        foreach (var orb in orbs)
            SavedOrbsInfo.Add(new Tuple<string, string>(orb.name, orb.GetComponent<OrbControls>().Target.name));
    }

    public static List<Tuple<string,string>> GetSavedOrbs() {
        return SavedOrbsInfo;
    }

    public static void DisplayMessage(string message, float duration) {
        textMeshNotification.GetComponent<TextMesh>().text = message;
        textMeshNotificationCounter = duration;
    }

    public void MessageLoop() {
        if (textMeshNotificationCounter > 0) {
            if (textMeshNotification.activeSelf == false)
                textMeshNotification.SetActive(true);

            textMeshNotificationCounter -= Time.deltaTime;
        } else {
            if (textMeshNotification.activeSelf == true)
                textMeshNotification.SetActive(false);
        }
    }

    public void QuitGame() {
        SaveProgress();
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    private void SaveProgress() {
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");
        using (StreamWriter sw = File.CreateText(savePath)) {
            SavedData sd = new SavedData {
                BossesCleared = BossesBeaten,
                MasterVolume = GameSettings.MasterVolume,
                Resolution = new Vector2(Screen.width, Screen.height),
                IsWindowed = !Screen.fullScreen
            };

            sw.Write(JsonUtility.ToJson(sd));
        }
    }

    public static void LoadProgress() {
        if (!File.Exists(savePath))
            return;
        using (StreamReader sr = new StreamReader(savePath)) {
            SavedData sd = JsonUtility.FromJson<SavedData>(sr.ReadLine());
            BossesBeaten = sd.BossesCleared;
            Screen.fullScreen = !sd.IsWindowed;
            Screen.SetResolution((int)sd.Resolution.x, (int)sd.Resolution.y, Screen.fullScreen);
            GameSettings.MasterVolume = sd.MasterVolume;
        }
    }

    public void OnVolumeSliderChanged(float value) {
        GameSettings.MasterVolume = value;
    }

    public void OnResolutionDropdownChange(int option) {
        switch (option) {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
        }
    }

    public void OnWindowModeTick(bool isWindowMode) {
        if (isWindowMode)
            Screen.fullScreen = false;
        else
            Screen.fullScreen = true;
    }

    public void OnSceneSelectorButton(string scene) {
        if (scene.Contains("Tutorial") && !SceneManager.GetActiveScene().name.Contains("Tutorial"))
            SceneManager.LoadScene("TutorialRoom");
        if (scene.Contains("Beach") && !SceneManager.GetActiveScene().name.Contains("Room1"))
            SceneManager.LoadScene("SlimeBossRoom1");
        if (scene.Contains("Volcano") && !SceneManager.GetActiveScene().name.Contains("Room2"))
            SceneManager.LoadScene("SlimeBossRoom2");
        if (scene.Contains("Waste") && !SceneManager.GetActiveScene().name.Contains("Room3"))
            SceneManager.LoadScene("SlimeBossRoom3");
    }

    [Serializable]
    private class SavedData {
        public int BossesCleared;
        public float MasterVolume;
        public Vector2 Resolution;
        public bool IsWindowed;
    }

}

