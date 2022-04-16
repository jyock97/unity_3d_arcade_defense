using UnityEngine;

public static class TagsLayers
{
    public static readonly string PlayerTag = "Player";
    public static readonly string Enemy01Tag = "Enemy01";
    public static readonly string Enemy02Tag = "Enemy02";
    public static readonly string Enemy03Tag = "Enemy03";
    public static readonly string NexoTag = "Nexo";
    public static readonly string SpikesTag = "Spikes";
    public static readonly string TorretTag = "Torret";

    public static readonly LayerMask GroundLayerMask = LayerMask.GetMask("Ground");
    public static readonly LayerMask PlayerLayerMask = LayerMask.GetMask("Player");
    public static readonly LayerMask EnemyLayerMask = LayerMask.GetMask("Enemy");
    public static readonly LayerMask NexoLayerMask = LayerMask.GetMask("Nexo");
    public static readonly LayerMask Player_NexoLayerMask = LayerMask.GetMask("Player", "Nexo");
    
    public static readonly int EnemyLayerMaskIndex = LayerMask.NameToLayer("Enemy");
}
