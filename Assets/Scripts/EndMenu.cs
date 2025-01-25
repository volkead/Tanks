//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI; // Assurez-vous d'ajouter cette directive pour manipuler les UI
//using UnityEngine.SceneManagement; // Pour g�rer les sc�nes
//using TMPro;

//public class EndMenu : MonoBehaviour
//{
//    [SerializeField] private TMP_Text winMessage; // Text UI pour afficher le message de victoire
//    [SerializeField] private GameObject endMenuUI; // Le panel du menu de fin

//    private PlayerManager playerManager; // R�f�rence au PlayerManager

//    // Action lorsque l'�v�nement de victoire est d�clench�
//    public void OnWinEvent(EPlayer winner)
//    {
//        // Afficher un message de victoire
//        winMessage.text = $"Le joueur {winner} a gagn� !";
//        endMenuUI.SetActive(true); // Afficher le menu de fin
//    }

//    // Action lorsque le bouton de replay est cliqu�
//    public void ReplayLevel()
//    {
//        DestroyAllTanks();
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        StartCoroutine(InstantiatePlayersAfterLoad());
//    }

//    // Action lorsque le bouton de menu principal est cliqu�
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

//        Debug.Log("Tous les tanks ont �t� d�truits.");
//    }

//    // M�thode pour instancier les joueurs apr�s le chargement de la sc�ne
//    private IEnumerator InstantiatePlayersAfterLoad()
//    {
//        // Attendre que la sc�ne soit compl�tement charg�e
//        yield return new WaitForSeconds(0.1f); // Un petit d�lai pour �tre s�r que la sc�ne est pr�te


//        playerManager = FindObjectOfType<PlayerManager>();

//        if (playerManager != null)
//        {
//            playerManager.InstantiatePlayers();
//        }
//        else
//        {
//            Debug.LogWarning("PlayerManager n'a pas �t� trouv� dans la sc�ne !");
//        }
//    }

//    private void Start()
//    {
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.OnWin += OnWinEvent; // S'abonner � l'�v�nement OnWin
//        }
//        else
//        {
//            Debug.LogWarning("GameManager non trouv� !");
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
//            gameManager.OnWin -= OnWinEvent; // Se d�sabonner de l'�v�nement OnWin
//        }
//    }
//}
