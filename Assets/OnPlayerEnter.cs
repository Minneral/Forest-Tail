using UnityEngine;

public class OnPlayerEnter : MonoBehaviour
{
    public Vector3 position;

    void Start()
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            Debug.Log("Player point: " + player.transform.position.ToString());
            player.transform.position = position + new Vector3(0, 5, 0);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = position + new Vector3(0, 5, 0);
            player.GetComponent<CharacterController>().enabled = true;
            Debug.Log("Player after: " + player.transform.position.ToString());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter trigger with Player");
            var player = other.gameObject;
            Debug.Log(player);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = position;
            player.GetComponent<CharacterController>().enabled = true;
            Debug.Log("Player moved in OnTriggerEnter");
        }
    }
}
