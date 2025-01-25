//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI; // Assurez-vous d'ajouter cette directive pour manipuler les UI
//using UnityEngine.SceneManagement; // Pour gérer les scènes
//using TMPro;

//public class EndMenu : MonoBehaviour
//{
//    [SerializeField] private TMP_Text winMessage; // Text UI pour afficher le message de victoire
//    [SerializeField] private GameObject endMenuUI; // Le panel du menu de fin

//    private PlayerManager playerManager; // Référence au PlayerManager

//    // Action lorsque l'événement de victoire est déclenché
//    public void OnWinEvent(EPlayer winner)
//    {
//        // Afficher un message de victoire
//        winMessage.text = $"Le joueur {winner} a gagné !";
//        endMenuUI.SetActive(true); // Afficher le menu de fin
//    }

//    // Action lorsque le bouton de replay est cliqué
//    public void ReplayLevel()
//    {
//        DestroyAllTanks();
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        StartCoroutine(InstantiatePlayersAfterLoad());
//    }

//    // Action lorsque le bouton de menu principal est cliqué
//    public void LoadMainMenu()
//    {
//        DestroyAllTanks();
//        SceneManager.LoadScene("MainMenu");
//    }

//    private void DestroyAllTanks()
//    {
//        TankBehavior[] allTanks = FindObjectsOfType<TankBehavior>();

//        foreach (var tank in allTanks)
//        {
//            Destroy(tank.gameObject);
//        }

//        Debug.Log("Tous les tanks ont été détruits.");
//    }

//    // Méthode pour instancier les joueurs après le chargement de la scène
//    private IEnumerator InstantiatePlayersAfterLoad()
//    {
//        // Attendre que la scène soit complètement chargée
//        yield return new WaitForSeconds(0.1f); // Un petit délai pour être sûr que la scène est prête


//        playerManager = FindObjectOfType<PlayerManager>();

//        if (playerManager != null)
//        {
//            playerManager.InstantiatePlayers();
//        }
//        else
//        {
//            Debug.LogWarning("PlayerManager n'a pas été trouvé dans la scène !");
//        }
//    }

//    private void Start()
//    {
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.OnWin += OnWinEvent; // S'abonner à l'événement OnWin
//        }
//        else
//        {
//            Debug.LogWarning("GameManager non trouvé !");
//        }

//        playerManager = FindObjectOfType<PlayerManager>();

//        if (playerManager != null)
//        {
//            playerManager.InstantiatePlayers();
//        }
//    }

//    private void OnDestroy()
//    {
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.OnWin -= OnWinEvent; // Se désabonner de l'événement OnWin
//        }
//    }
//}
