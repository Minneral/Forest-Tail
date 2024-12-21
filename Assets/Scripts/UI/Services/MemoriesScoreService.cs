using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MemoriesScoreService : MonoBehaviour
{
    public Actor actor;
    TextMeshProUGUI field;
    public enum Actor
    {
        PLAYER,
        BOT
    }

    private void Start()
    {
        field = GetComponent<TextMeshProUGUI>();

        if (field == null)
        {
            this.enabled = false;
            return;
        }
    }

    private void Update()
    {
        string pattern = @"\d+";

        field.text = Regex.Replace(field.text, pattern, match =>
        {
            string score = "";

            switch (actor)
            {
                case Actor.PLAYER:
                    score = Memories.instance.GetScore().PlayerScore.ToString();
                    break;

                case Actor.BOT:
                    score = Memories.instance.GetScore().BotScore.ToString();
                    break;
                default:
                    score = "";
                    break;
            }

            return score;
        });
    }
}
