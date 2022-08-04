using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// 数据管理类
/// </summary>
public class PlayDataManager : MonoBehaviour
{
    public static PlayDataManager instance;

    /// <summary>
    /// 文件保存路径
    /// </summary>
    string path;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            path = Application.streamingAssetsPath;
        }
        else
        {
            path = Application.persistentDataPath;
        }
    }
    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dataName">const 数据名称</param>
    /// <returns></returns>
    public Data LoadData(string dataName)
    {
        //有这个路径的话
        if (File.Exists(path + "/" + dataName))
        {
            using (FileStream fileStream = File.Open(path + "/" + dataName, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Data TempData = (Data)bf.Deserialize(fileStream);
                return TempData;
            }
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dataName"></param>
    public void SaveData(Data data, string dataName)
    {
        //先销毁原来的数据
        if (File.Exists(path + "/" + dataName))
        {
            File.Delete(path + "/" + dataName);
        }
        using (FileStream file = File.Create(path + "/" + dataName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            //序列化新数据到路径中
            bf.Serialize(file, data);
        }
    }
}
