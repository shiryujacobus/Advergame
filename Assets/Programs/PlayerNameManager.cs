using UnityEngine;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;

    public void SavePlayerName()
    {
        if (playerNameInput == null)
        {
            Debug.LogError("Player Name Input belum di-assign!");
            return;
        }

        string playerName = playerNameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("Nama pemain disimpan: " + playerName);
    }
}