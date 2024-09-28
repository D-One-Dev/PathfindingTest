using UnityEngine;

public class SaveController : MonoBehaviour
{
    private readonly string SAVE_PATH = "/GameSave.savefile";
    public void EraseSaveFile()
    {
        string path = Application.persistentDataPath + SAVE_PATH;
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}