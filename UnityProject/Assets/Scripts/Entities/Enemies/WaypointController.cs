using UnityEngine;
using UnityEngine.UI;

public class WaypointController : MonoBehaviour
{
    [SerializeField] private Image wayPoint;

    private Camera _mainCamera;
    private Rect _wayPointSize;

    public void Start()
    {
        _mainCamera = Camera.main;
        _wayPointSize = wayPoint.GetPixelAdjustedRect();
    }
    
    public void Update()
    {
        if (wayPoint.gameObject.activeSelf)
        {
            
            float minX = _wayPointSize.width / 2;
            float minY = _wayPointSize.height / 2;
            float maxX = Screen.width - minX;
            float maxY = Screen.height - minY;
            
            Vector2 newPos = _mainCamera.WorldToScreenPoint(transform.position);
            if (Vector3.Dot(transform.position - _mainCamera.transform.position, _mainCamera.transform.forward) < 0)
            {
                newPos.x = newPos.x < (float) Screen.width / 2 ? maxX : minX;
            }
            
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            wayPoint.transform.position = newPos;

            Vector2 viewPortPosition = _mainCamera.WorldToViewportPoint(transform.position);
            if (viewPortPosition.x >= 0 && viewPortPosition.x <= 1 && viewPortPosition.y >= 0 && viewPortPosition.y <= 1)
            {
                wayPoint.gameObject.SetActive(false);
            }
        }
        else
        {
            Vector2 viewPortPosition = _mainCamera.WorldToViewportPoint(transform.position);
            if (viewPortPosition.x < 0 || viewPortPosition.x > 1 || viewPortPosition.y < 0 || viewPortPosition.y > 1)
            {
                wayPoint.gameObject.SetActive(true);
            }
        }
    }
}
