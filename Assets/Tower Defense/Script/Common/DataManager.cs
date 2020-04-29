using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//保存数据格式的版本，用于检测存储的数据格式是否等于实际的数据格式
public class DataVersion
{
    public int major;
    public int minor;
}

//存储游戏的进度
[System.Serializable]
public class GameProgressData
{
    //保存时间
    public System.DateTime saveTime;
    //最后一次完成的地图
    public string lastMap;
    //列出解锁的地图
    public List<string> openedMaps = new List<string>();
}

//从文件中保存和加载数据
public class DataManager : MonoBehaviour
{
    //实例
    public static DataManager instance;
    //游戏进度数据容器
    public GameProgressData gameProgressData = new GameProgressData();
    //保存数据格式版本的文件名
    private string dataVersionFile = "/DataVersion.dat";
    //保存游戏进度数据的文件名
    private string gameProgressDataFile = "/GameProgressData";
    //数据格式版本容器
    private DataVersion dataVersion = new DataVersion();
    //默认游戏进度数据的容器
    private GameProgressData defaultGameProgressData = new GameProgressData();
    
    //唤醒这个实例
    void Awake()
    {
        if (instance == null)
        {
            //数据格式版本
            dataVersion.major = 1;
            dataVersion.minor = 0;
            //处理游戏进度数据
            gameProgressData.saveTime = defaultGameProgressData.saveTime = DateTime.MinValue;
            gameProgressData.lastMap = defaultGameProgressData.lastMap = " ";
            instance = this;
            DontDestroyOnLoad(gameObject);
            UpdateDataVersion();
            LoadGameProgressData();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //保存游戏进度数据
    public void SaveGameProgressData()
    {
        BinaryFormatter bfCreate = new BinaryFormatter();
        FileStream fileCreate = File.Create(Application.persistentDataPath + gameProgressDataFile);
        gameProgressData.saveTime = DateTime.Now;
        bfCreate.Serialize(fileCreate, gameProgressData);
        fileCreate.Close();
    }

    //加载游戏进度数据
    public void LoadGameProgressData()
    {
        if (File.Exists(Application.persistentDataPath + gameProgressDataFile) == true)
        {
            BinaryFormatter bfOpen = new BinaryFormatter();
            FileStream fileOpen = File.Open(Application.persistentDataPath + gameProgressDataFile, FileMode.Open);
            gameProgressData = (GameProgressData)bfOpen.Deserialize(fileOpen);
            fileOpen.Close();
        }
    }

    //更新数据格式版本
    private void UpdateDataVersion()
    {
        if (File.Exists(Application.persistentDataPath + dataVersionFile) == true)
        {
            BinaryFormatter bfOpen = new BinaryFormatter();
            FileStream fileOpen = File.Open(Application.persistentDataPath + dataVersionFile, FileMode.Open);
            DataVersion version = (DataVersion) bfOpen.Deserialize(fileOpen);
            fileOpen.Close();

            switch (version.major)
            {
                case 1:
                    break;
            }
        }

        BinaryFormatter bfCreate = new BinaryFormatter();
        FileStream fileCreate = File.Create(Application.persistentDataPath + dataVersionFile);
        bfCreate.Serialize(fileCreate, dataVersion);
        fileCreate.Close();
    }

    //删除游戏进度数据
    private void DeleteGameProgressData()
    {
        File.Delete(Application.persistentDataPath + gameProgressDataFile);
    }
}
