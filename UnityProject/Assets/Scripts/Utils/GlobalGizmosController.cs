using UnityEngine;

/// <summary>
/// Feature switch class used to hide or show Gizmos
/// </summary>
public class GlobalGizmosController : MonoBehaviour
{
    [SerializeField] private bool disableAll;
    [SerializeField] private bool enableAll;

    //------------Player Gizmos------------------
    [Header("Player Gizmos")]
    public bool player;
    public static bool Player;
    // PlayerMovement
    public bool leftMouseClick;
    public static bool LeftMouseClick;
    // PlayerDash
    public bool dashDirection;
    public static bool DashDirection;
    // PlayerAttack
    public bool attackHitBoxAlways;
    public static bool AttackHitBoxAlways;
    public bool attackHitBoxOnHit;
    public static bool AttackHitBoxOnHit;
    
    //-------------Enemy Gizmos------------------
    public bool enemies;
    public static bool Enemies;
    // EnemyStateMachine
    public bool sightRadius;
    public static bool SightRadius;
    public bool attackRadius;
    public static bool AttackRadius;
    // Enemy01StateMachine
    public bool enemy01AttackHitBoxAlways;
    public static bool Enemy01AttackHitBoxAlways;
    public bool enemy01AttackHitBoxOnHit;
    public static bool Enemy01AttackHitBoxOnHit;
    // Enemy02StateMachine
    public bool enemy02AttackTarget;
    public static bool Enemy02AttackTarget;
    // AreaProjectileController
    public bool areaProjectile;
    public static bool AreaProjectile;
    // AreaDamageController
    public bool areaDamagePosition;
    public static bool AreaDamagePosition;
    public bool areaDamageOnHit;
    public static bool AreaDamageOnHit;
    
    //-------------Tower Gizmos------------------
    [Header("Tower Structure")]
    public bool towerAttackRange;
    public static bool TowerAttackRange;
    public bool towerAttackHitBoxOnHit;
    public static bool TowerAttackHitBoxOnHit;

    private void SyncValues()
    {
        Player = player;
        LeftMouseClick = leftMouseClick;
        DashDirection = dashDirection;
        AttackHitBoxAlways = attackHitBoxAlways;
        AttackHitBoxOnHit = attackHitBoxOnHit;

        Enemies = enemies;
        SightRadius = sightRadius;
        AttackRadius = attackRadius;
        Enemy01AttackHitBoxAlways = enemy01AttackHitBoxAlways;
        Enemy01AttackHitBoxOnHit = enemy01AttackHitBoxOnHit;
        Enemy02AttackTarget = enemy02AttackTarget;
        AreaProjectile = areaProjectile;
        AreaDamagePosition = areaDamagePosition;
        AreaDamageOnHit = areaDamageOnHit;

        TowerAttackRange = towerAttackRange;
        TowerAttackHitBoxOnHit = towerAttackHitBoxOnHit;
    }
    
    private void SetAll(bool flag)
    {
        player = flag;
        leftMouseClick = flag;
        dashDirection = flag;
        attackHitBoxAlways = flag;
        attackHitBoxOnHit = flag;
        
        enemies = flag;
        sightRadius = flag;
        attackRadius = flag;
        enemy01AttackHitBoxAlways = flag;
        enemy01AttackHitBoxOnHit = flag;
        enemy02AttackTarget = flag;
        areaProjectile = flag;
        areaDamagePosition = flag;
        areaDamageOnHit = flag;

        towerAttackRange = flag;
        towerAttackHitBoxOnHit = flag;
    }
    
    private void OnValidate()
    {
        ProcessFlags();
    }

    private void Update()
    {
        ProcessFlags();
    }

    private void ProcessFlags()
    {
        if (disableAll)
        {
            SetAll(false);
            disableAll = false; // TODO make this Buttons with Custom Editor
        }

        if (enableAll)
        {
            SetAll(true);
            enableAll = false; // TODO make this Buttons with Custom Editor
        }
        
        SyncValues();
    }
}
