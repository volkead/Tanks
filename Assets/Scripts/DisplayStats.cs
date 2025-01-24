using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image healthBar;       // Barre de vie
    [SerializeField] private Text ammoCountText;    // Texte pour les munitions
    [SerializeField] private Text mineCountText;    // Texte pour les mines
    [SerializeField] private Text powerUpText;      // Texte pour le pouvoir actif

    [Header("Player Info")]
    [SerializeField] private EPlayer playerToDisplay; // Joueur à afficher

    private TankBehavior targetTank;

    private void Start()
    {
        // Recherche du tank correspondant au joueur à afficher
        TankBehavior[] allTanks = FindObjectsOfType<TankBehavior>();
        foreach (TankBehavior tank in allTanks)
        {
            if (tank.playerID == playerToDisplay)
            {
                targetTank = tank;
                break;
            }
        }

        if (targetTank != null)
        {
            // S'abonner à l'événement pour les changements de pouvoir actif
            targetTank.OnPowerUpChanged += UpdatePowerUpUI;
        }
        else
        {
            Debug.LogWarning($"Aucun tank trouvé pour le joueur {playerToDisplay}.");
            gameObject.SetActive(false); // Cache l'UI si aucun tank n'est trouvé
        }
    }

    private void Update()
    {
        if (targetTank != null)
        {
            // Mise à jour de l'interface
            UpdateHealthBar();
            UpdateAmmoCount();
            UpdateMineCount();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)targetTank.health / targetTank.maxHealth;
    }

    private void UpdateAmmoCount()
    {
        ammoCountText.text = targetTank.ammo.ToString();
    }

    private void UpdateMineCount()
    {
        mineCountText.text = targetTank.mineAmmo.ToString();
    }

    private void UpdatePowerUpUI(string powerUpName)
    {
        if (string.IsNullOrEmpty(powerUpName))
        {
            powerUpText.text = "Power-Up: None";
        }
        else
        {
            powerUpText.text = $"Power-Up: {powerUpName}";
        }
    }

    private void OnDestroy()
    {
        if (targetTank != null)
        {
            targetTank.OnPowerUpChanged -= UpdatePowerUpUI;
        }
    }
}
