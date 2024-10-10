using UnityEngine;
using Zenject;

public class SaveController : MonoBehaviour
{
    [Inject(Id = "SavePath")]
    private readonly string _savePath;
    public void EraseSaveFile()
    {
        string path = Application.persistentDataPath + _savePath;
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}