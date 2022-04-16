using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls a default HUD healthBar
/// </summary>
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private bool shouldFollowCamera;
    [SerializeField] private Image healthBar;

    private GameObject _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main.gameObject;
    }

    private void Update()
    {
        if (shouldFollowCamera)
        {
            Vector3 foward = (_mainCamera.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(foward);
        }
    }

    public void SetValue(float val)
    {
        healthBar.fillAmount = val;
    }
}