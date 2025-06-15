using UnityEngine;
using System.Collections;
using FMODUnity;

public class BonfireInteractable : Interactable
{
    [Header("Bonfire Info")]
    [SerializeField] int bonfireID;

    [Header("VFX")]
    [SerializeField] GameObject bonfireVFX;

    [Header("Active")]
    public bool isActivated = false;

    [Header("Interaction Text")]
    [SerializeField] private string unactivatedText = "Light the Flame";
    [SerializeField] private string activatedText = "Rest";

    [Header("Respawn")]
    public Transform respawnPosition;

    protected override void Start()
    {
        base.Start();

        WorldObjectManager.instance.AddBonfire(this);

        if (WorldSaveGameManager.instance.currentCharacterData.bonfiresLit.ContainsKey(bonfireID))
        {
            isActivated = WorldSaveGameManager.instance.currentCharacterData.bonfiresLit[bonfireID];
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.bonfiresLit.Add(bonfireID, isActivated);
        }

        if (isActivated)
        {
            bonfireVFX.SetActive(true);
            interactableText = activatedText;
        }
        else
        {
            interactableText = unactivatedText;
        }
    }

    private void LightFlame(PlayerManager player)
    {
        isActivated = true;
        WorldSaveGameManager.instance.currentCharacterData.bonfiresLit[bonfireID] = isActivated;
        WorldSaveGameManager.instance.currentCharacterData.lastBonfireRestedAt = bonfireID;

        player.playerAnimatorManager.PlayTargetActionAnimation("Light_Bonfire", true, false);
        // hide weapon


        // add a small delay before the bonfire lights up
        bonfireVFX.SetActive(true);
        interactableText = activatedText;

        // play sfx
        RuntimeManager.PlayOneShot("event:/Environment/Bonfire/Bonfire Ignite", transform.position);

        // send popup
        PlayerUIManager.instance.playerUIPopUpManager.SendBonfireLitPopUp();
        StartCoroutine(WaitToRestoreCollider());
    }

    private void RestAtBonfire(PlayerManager player)
    {
        WorldSaveGameManager.instance.currentCharacterData.lastBonfireRestedAt = bonfireID;

        // refill flasks

        StartCoroutine(WaitToRestoreCollider()); // need to change this so it comes back after getting up
        player.health = player.playerStatsManager.CalculateHealthBasedOnVitalityLevel(player.vitality);
        player.stamina = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(player.endurance);
        player.playerUIHudManager.SetNewHealthValue(player.health);
        player.playerUIHudManager.SetNewStaminaValue(player.stamina);
        //player.playerAnimatorManager.PlayTargetActionAnimation("Rest_Bonfire", true, false);
        PlayerUIManager.instance.playerUIBonfireManager.OpenMenu();

        WorldAIManager.instance.ResetAllCharacters();

    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        if (!isActivated)
        {
            LightFlame(player);
        }
        else
        {
            RestAtBonfire(player);
        }
    }

    private IEnumerator WaitToRestoreCollider()
    {
        yield return new WaitForSeconds(3f);
        interactableCollider.enabled = true;
    }

}
