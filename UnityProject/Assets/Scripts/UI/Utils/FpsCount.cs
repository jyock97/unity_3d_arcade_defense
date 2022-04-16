using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Util used to display FPS on Screen
/// </summary>
public class FpsCount : MonoBehaviour
{
    public TextMeshProUGUI fpsCount;

    private void Start()
    {
        StartCoroutine(CountFps());
    }

    private IEnumerator CountFps()
    {
        while (Application.isPlaying)
        {
            fpsCount.text = $"FPS: {(int) (1 / Time.unscaledDeltaTime)}";
            yield return new WaitForSeconds(0.3f);
        }
    }
}