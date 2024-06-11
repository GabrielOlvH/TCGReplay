using System.IO;
using UnityEngine;

public class ConfigReader : MonoBehaviour
{
    [System.Serializable]
    public class Config
    {
        public string token;
    }

    public static Config ReadConfig()
    {
        string path = Path.Combine(Application.dataPath, "config.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var config = JsonUtility.FromJson<Config>(json);
            return config;
        }
        else
        {
            Debug.LogError("Config file not found at " + path);
            return null;
        }
    }

    void Start()
    {
        var config = ReadConfig();
        if (config != null)
        {
            Debug.Log("Token: " + config.token);
        }
    }
}