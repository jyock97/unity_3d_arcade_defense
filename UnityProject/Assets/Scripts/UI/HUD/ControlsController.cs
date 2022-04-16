using UnityEngine;

public class ControlsController : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;

    public void ShowHidePanel()
    {
        controlsPanel.SetActive(!controlsPanel.activeSelf);
    }
}
