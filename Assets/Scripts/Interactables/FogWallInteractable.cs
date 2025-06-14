using System.Collections;
using FMODUnity;
using UnityEngine;

public class FogWallInteractable : Interactable
{
    [Header("Fog")]
    [SerializeField] GameObject[] fogGameObjects;

    [Header("Collider")]
    [SerializeField] Collider fogWallCollider;

    [Header("ID")]
    public int fogWallID;

    [Header("Active")]
    public bool isActive = false;

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.right);
        player.transform.rotation = targetRotation;

        AllowPlayerToPassThrough(player);
        player.playerAnimatorManager.PlayTargetActionAnimation("Pass_Through_01", true);

        // play sfx
        RuntimeManager.PlayOneShot("event:/Environment/Fog/Fog door");

    }

    private void AllowPlayerToPassThrough(PlayerManager player)
    {
        StartCoroutine(DisableCollisionTemporarily(player));
    }

    private IEnumerator DisableCollisionTemporarily(PlayerManager player)
    {
        Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

        yield return new WaitForSeconds(3f);

        Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
    }

    protected override void Awake()
    {
        WorldObjectManager.instance.AddFogWall(this);
    }

    private void FixedUpdate()
    {
        foreach (GameObject fog in fogGameObjects)
        {
            fog.SetActive(isActive);
        }
    }
}
