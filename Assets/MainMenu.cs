using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider tankSlider;
    [SerializeField] TMP_Text nbTankText;

    [SerializeField] PlayerManager playerManager;


    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        nbTankText.text = tankSlider.value.ToString();
       // playerManager.nbPlayer = (int)tankSlider.value;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level");
    }
}
