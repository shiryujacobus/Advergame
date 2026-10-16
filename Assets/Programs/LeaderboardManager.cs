
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
}

public class LeaderboardManager : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public Transform leaderboardContent;
    public GameObject leaderboardRowPrefab;
    public Sprite defaultAvatar;
    public int maxEntries = 10;

    private const string LeaderboardKey = "CafeLeaderboard";

    [Serializable]
    private class LeaderboardData
    {
        public List<LeaderboardEntry> entries =
            new List<LeaderboardEntry>();
    }

    void Start()
    {
        DisplayLeaderboard();
    }

    public static void SaveScore(string playerName, int score)
    {
        LeaderboardData data = LoadData();

        data.entries.Add(new LeaderboardEntry
        {
            playerName = string.IsNullOrWhiteSpace(playerName)
                ? "Player"
                : playerName.Trim(),
            score = score
        });

        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > 10)
            data.entries.RemoveRange(10, data.entries.Count - 10);

        PlayerPrefs.SetString(
            LeaderboardKey,
            JsonUtility.ToJson(data)
        );

        PlayerPrefs.Save();
    }

    public void DisplayLeaderboard()
    {
        if (leaderboardContent == null ||
            leaderboardRowPrefab == null)
        {
            Debug.LogError(
                "Leaderboard Content atau Row Prefab belum diisi!"
            );
            return;
        }

        // Hapus baris lama agar tidak muncul duplikat.
        foreach (Transform child in leaderboardContent)
            Destroy(child.gameObject);

        LeaderboardData data = LoadData();

        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        int count = Mathf.Min(maxEntries, data.entries.Count);

        for (int i = 0; i < count; i++)
        {
            LeaderboardEntry entry = data.entries[i];

            GameObject row = Instantiate(
                leaderboardRowPrefab,
                leaderboardContent
            );

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>(true);

            foreach (TMP_Text text in texts)
            {
                if (text.name == "RankText")
                    text.text = (i + 1).ToString();

                else if (text.name == "PlayerNameText")
                    text.text = entry.playerName;

                else if (text.name == "ScoreText")
                    text.text = entry.score.ToString();
            }

            Transform avatarTransform = row.transform.Find("AvatarImage");

            if (avatarTransform != null)
            {
                Image avatarImage = avatarTransform.GetComponent<Image>();

                if (avatarImage != null && defaultAvatar != null)
                    avatarImage.sprite = defaultAvatar;
            }
        }

        if (count == 0)
            Debug.Log("Belum ada skor di leaderboard.");
    }

    public static void ResetLeaderboard()
    {
        PlayerPrefs.DeleteKey(LeaderboardKey);
        PlayerPrefs.Save();

        Debug.Log("Leaderboard berhasil di-reset!");
    }

    private static LeaderboardData LoadData()
    {
        string json = PlayerPrefs.GetString(LeaderboardKey, "");

        if (string.IsNullOrEmpty(json))
            return new LeaderboardData();

        LeaderboardData data =
            JsonUtility.FromJson<LeaderboardData>(json);

        return data ?? new LeaderboardData();
    }
}
