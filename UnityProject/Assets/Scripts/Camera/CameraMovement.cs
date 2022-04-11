using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private Vector3 cameraOffset;
    
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag(TagsLayers.PlayerTag);
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.LookRotation(_player.transform.position);
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        transform.rotation = rotation;

        
        Vector3 newCamPos = new Vector3(0,0, Vector3.Distance(Vector3.zero, _player.transform.position));
        mainCamera.transform.localPosition = newCamPos + cameraOffset;
//        mainCamera.transform.rotation = rotation;
        
        
        
    }
}
