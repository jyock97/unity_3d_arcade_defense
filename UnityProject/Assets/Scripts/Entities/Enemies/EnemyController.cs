using UnityEngine;

public class EnemyController : EntityController
{
    [SerializeField] private int moneyGain;
    
    private PlayerController _playerController;

    protected override void Start()
    {
        base.Start();

        _playerController = FindObjectOfType<PlayerController>();
    }

    protected override void Die()
    {
        base.Die();
        
        _playerController.money += moneyGain;
        GameController.Instance.ReduceTotalEnemies();
        
        Destroy(gameObject);
    }
}
