using TMPro;
using UnityEngine;

public class PushData : MonoBehaviour
{
    public TMP_InputField screenName;

    private PostData post;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendData()
    {
        if (screenName.text != "")
        {
            post.SetupPlayerData(screenName.text);
        }
    }
}
