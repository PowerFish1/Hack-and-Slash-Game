using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingEdge : MonoBehaviour
{
    public GameObject fadeOut;
    public GameObject fellText;
    public GameObject levelAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(YouFellOff(collision.gameObject));
        }
        else if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator YouFellOff(GameObject player)
    {
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null)
        {
            fellText.SetActive(true);
            levelAudio.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            fadeOut.SetActive(true);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(1);
        }
    }
}
