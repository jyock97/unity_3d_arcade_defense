using UnityEngine;

public static class TagsLayers
{
    public static readonly string PlayerTag = "Player";
    public static readonly string EnemyTag = "Enemy";
    public static readonly string NexoTag = "Nexo";

    public static readonly LayerMask GroundLayerMask = LayerMask.GetMask("Ground");
    public static readonly LayerMask PlayerLayerMask = LayerMask.GetMask("Player");
    public static readonly LayerMask EnemyLayerMask = LayerMask.GetMask("Enemy");
    public static readonly LayerMask NexoLayerMask = LayerMask.GetMask("Nexo");
    public static readonly LayerMask Player_NexoLayerMask = LayerMask.GetMask("Player", "Nexo");
    
//    public static readonly int PlayerLayerMaskIndex = LayerMask.NameToLayer("Player");
}
