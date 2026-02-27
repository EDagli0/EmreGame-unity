using TMPro;
using UnityEngine;

public class WaveUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text nextWaveText;

    public void SetWave(int wave)
    {
        if (waveText != null)
            waveText.text = $"Wave: {wave}";
    }

    public void SetNextWaveSeconds(float seconds)
    {
        if (nextWaveText == null) return;

        seconds = Mathf.Max(0f, seconds);
        nextWaveText.text = $"Diðer wave {seconds:0}s sonra:";
    }

    public void ClearNextWave()
    {
        if (nextWaveText != null)
            nextWaveText.text = "";
    }
}