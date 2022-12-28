using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3 : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public Avatar[] avatar;
    public GameObject[] character;

    public GameObject mainUI;
    public Text exitText;
    public Text exitBtnText;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            character[i].SetActive(false);
        }

        character[GameUI.characterN].SetActive(true);
        player.GetComponent<Animator>().avatar = avatar[GameUI.characterN];

    }

    void Update()
    {
        mainUI.transform.localScale = new Vector3(Screen.width / 1366f, Screen.height / 768f, 1f);
        enemy.transform.position += (player.transform.position - enemy.transform.position) * Time.deltaTime / 2;
        enemy.transform.rotation = player.transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (string.Equals(other.tag, "enemy"))
        {
            player.SetActive(false);
            mainUI.SetActive(true);
            exitText.text = "Game Over";
            exitBtnText.text = "Restart";
        }
        else if (string.Equals(other.tag, "reward"))
        {
            player.SetActive(false);
            mainUI.SetActive(true);
            exitText.text = "Congratulations!";
            exitBtnText.text = "Next";
        }
    }

    public void BackBtnClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void RestartBtnClick()
    {
        if (string.Equals(exitBtnText.text, "Restart"))
        {
            SceneManager.LoadScene("Level3");
        }
        else if (string.Equals(exitBtnText.text, "Next"))
        {
            SceneManager.LoadScene("Level4");
        }
    }
}
