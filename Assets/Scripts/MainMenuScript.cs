using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public TMP_Text highScoreText;
    private string _gameViewSceneName;

    private void Start()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    public void LoadGameView()
    {
        _gameViewSceneName = "GameViewScene";
        SceneManager.LoadScene(_gameViewSceneName);
    }
}
