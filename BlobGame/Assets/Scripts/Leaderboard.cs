using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static FetchData;

public class Leaderboard : MonoBehaviour
{
    public string apiUrl = "http://localhost:3000/player"; 
    public TextMeshProUGUI leaderboardText;
   
    List<PlayerData> playerList;
    PlayerData player;

    void Start()
    {
        StartCoroutine(GetTopPlayers());
    }

    IEnumerator GetTopPlayers()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching leaderboard: " + request.error);
                leaderboardText.text = "Failed to load leaderboard.";
            }
            else
            {
                string json = request.downloadHandler.text;
                PlayerData[] players = JsonHelper.FromJson<PlayerData>(json);

                if (players.Length > 0)
                {
                    DisplayLeaderboard(players);
                }
                else
                {
                    leaderboardText.text = "No players found.";
                }
            }
        }
    }

    void DisplayLeaderboard(PlayerData[] players)
    {
        Debug.Log("Updating Leaderboard UI...");
        leaderboardText.text = "<b>Leaderboard (Top 10)</b>\n\n";

        for (int i = 0; i < Mathf.Min(10,players.Length); i++)
        {
            leaderboardText.text += $"{i + 1}. {players[i].userName} - Mass: {players[i].highestMass}\n";
        }
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
