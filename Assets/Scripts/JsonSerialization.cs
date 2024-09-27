using UnityEngine;

public class JsonSerialization
{
    private static readonly string SAVE_PATH = "/GameSave.savefile";
    public static void WriteSave(GameSave save)
    {
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        string path = Application.persistentDataPath + SAVE_PATH;
        System.IO.File.WriteAllText(path, json);
    }

    public static GameSave ReadSave()
    {
        string path = Application.persistentDataPath + SAVE_PATH;
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            return JsonUtility.FromJson<GameSave>(json);
        }
        return null;
    }
}