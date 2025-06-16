using UnityEngine;
using TMPro;

public class UI_Boss_HP_Bar : UiStat_Bar
{
    [SerializeField] AIBossCharacterManager bossCharacter;

    public void EnableBossHPBar(AIBossCharacterManager boss)
    {
        if (boss == null) return;
        scaleBarLengthWithStats = false; 
        bossCharacter = boss;
        gameObject.SetActive(true);
        SetMaxStat(bossCharacter.maxHealth);
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
    }

    private void OnBossHPChanged(float newHP)
    {
        SetStat(newHP);
        
        if (newHP <= 0)
        {
            DisableBossHPBar(2.5f);
        }
    }

    public void DisableBossHPBar(float time)
    {
        Destroy(gameObject, time);
    }

    void FixedUpdate()
    {
        if (bossCharacter != null) OnBossHPChanged(bossCharacter.health);
    }

}
