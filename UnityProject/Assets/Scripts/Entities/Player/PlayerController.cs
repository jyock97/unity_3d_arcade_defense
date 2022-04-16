using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : EntityController
{
    
    public int money;
    
    [SerializeField] private float mouseRaycastDistance;
    [SerializeField] private GameObject staff;

    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isBuilding;

    private Camera _mainCamera;
    private Vector3 _mousePosition;
    private Ray _mouseScreenToWorldRay;

    protected override void Start()
    {
        base.Start();
        
        _mainCamera = Camera.main;
    }

    public RaycastHit RaycastMouseToGround(out RaycastHit hit)
    {
        _mousePosition = Input.mousePosition;
        _mouseScreenToWorldRay = _mainCamera.ScreenPointToRay(_mousePosition);
            
        Physics.Raycast(_mouseScreenToWorldRay, out hit, mouseRaycastDistance, TagsLayers.GroundLayerMask);

        return hit;
    }

    protected override void Die()
    {
        base.Die();
        
        staff.transform.SetParent(null);
        staff.GetComponent<Rigidbody>().useGravity = true;
        staff.GetComponent<BoxCollider>().enabled = true;

        GameController.Instance.StopScore();
        StartCoroutine(GameOverAfterSeconds());
    }

    private IEnumerator GameOverAfterSeconds()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOver");
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int newMoney)
    {
        money = newMoney;
    }
}
