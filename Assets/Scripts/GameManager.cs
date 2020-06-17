using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static bool gameEnded
        ;
    public GameObject GameOverUI;


    void Start()
    {
        gameEnded = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameEnded)
            return;
        if (PlayerStats.Health <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;
        GameOverUI.SetActive(true);
        //LoadScene?
    }
}
