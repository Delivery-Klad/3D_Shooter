using UnityEngine;
using UnityEngine.UI;

public class AccountStats : MonoBehaviour
{
    public Text Games;
    public Text Kills;
    public Text Deaths;
    public Text Wins;
    public Text Items;
    public Text Windows;
    public Text Distance;

    void Start()
    {
        if (PlayerPrefs.HasKey("TotalGames"))
        {
            Games.text = PlayerPrefs.GetInt("TotalGames").ToString();
        }
        if (PlayerPrefs.HasKey("TotalKills"))
        {
            Kills.text = PlayerPrefs.GetInt("TotalKills").ToString();
        }
        if (PlayerPrefs.HasKey("TotalDeaths"))
        {
            Deaths.text = PlayerPrefs.GetInt("TotalDeaths").ToString();
        }
        if (PlayerPrefs.HasKey("TotalWins"))
        {
            Wins.text = PlayerPrefs.GetInt("TotalWins").ToString();
        }
        if (PlayerPrefs.HasKey("TotalItems"))
        {
            Items.text = PlayerPrefs.GetInt("TotalItems").ToString();
        }
        if (PlayerPrefs.HasKey("TotalWindows"))
        {
            Windows.text = PlayerPrefs.GetInt("TotalWindows").ToString();
        }
        if (PlayerPrefs.HasKey("TotalDistance"))
        {
            Distance.text = PlayerPrefs.GetFloat("TotalDistance").ToString();
        }
    }
}
