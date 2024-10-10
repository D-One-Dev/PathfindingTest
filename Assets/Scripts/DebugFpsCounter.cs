using TMPro;
using UnityEngine;
using Zenject;

public class DebugFpsCounter : MonoBehaviour
{
    [Inject(Id = "CurrentFPSText")]
    private readonly TMP_Text FPSText;
    [Inject(Id = "MinFPSText")]
    private readonly TMP_Text minFPSText;

    private int minFPS = 50000;

    private void Start()
    {
        Application.targetFrameRate = 50000;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        int fps = (int)(1 / Time.deltaTime);
        if (fps < minFPS)
        {
            minFPS = fps;
            minFPSText.text = "Min FPS: " + minFPS.ToString();
        }

        FPSText.text = "FPS: " + fps.ToString();
    }

    public void ClearMinFPS()
    {
        minFPS = 50000;
    }
}