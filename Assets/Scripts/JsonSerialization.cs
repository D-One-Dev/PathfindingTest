using UnityEngine;
using Zenject;

public class JsonSerialization
{
    [Inject(Id = "SavePath")]
    private readonly string _savePath;
    public void WriteSave(GameSave save)
    {
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        string path = Application.persistentDataPath + _savePath;
        System.IO.File.WriteAllText(path, json);
    }

    public GameSave ReadSave()
    {
        string path = Application.persistentDataPath + _savePath;
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            return JsonUtility.FromJson<GameSave>(json);
        }
        return null;
    }
}