using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UICharacterHPBar : UiStat_Bar
{
    private AICharacterManager character;
    [SerializeField] float defaultTimer = 3;
    [SerializeField] float timer = 0;
    [SerializeField] float currentDamageTaken = 0;
    [SerializeField] TextMeshProUGUI characterDamage;

    protected override void Awake()
    {
        base.Awake();
        character = GetComponentInParent<AICharacterManager>();
    }

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }

    public override void SetStat(float newValue)
    {
        slider.maxValue = character.maxHealth;

        float oldValue = slider.value;
        currentDamageTaken = currentDamageTaken + (oldValue - newValue); 
        if (currentDamageTaken < 0)
        {
            currentDamageTaken = Mathf.Abs(currentDamageTaken);
            characterDamage.text = "+ " + currentDamageTaken.ToString();
        }
        else
        {
            characterDamage.text = "- " + currentDamageTaken.ToString();
        }
        slider.value = newValue;

        if (character.health != character.maxHealth)
        {
            gameObject.SetActive(true);
            timer = defaultTimer;
        }
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        currentDamageTaken = 0;
        characterDamage.text = "";
    }



}
