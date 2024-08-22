using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [SerializeField] private bool useEncryption;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<IData> dataObjects;

    public static DataManager instance {get; private set;}

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        this.dataObjects = FindAllDataObjects();
        LoadGame();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one distance,...");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data found. Initializing new game...");
            NewGame();
        }

        foreach(IData dataObj in dataObjects)
        {
            dataObj.LoadData(gameData);
        }

        Debug.Log("Coins loaded: " + gameData.coinCount);
    }

    public void SaveGame()
    {
        foreach(IData dataObj in dataObjects)
        {
            dataObj.SaveData(ref gameData);
        }

        Debug.Log("Coins saved: " + gameData.coinCount);

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IData> FindAllDataObjects()
    {
        IEnumerable<IData> foundDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();

        return new List<IData>(foundDataObjects);
    }
}
