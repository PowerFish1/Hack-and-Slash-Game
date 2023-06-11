using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TypeWriter : MonoBehaviour
{
    public GameObject fadeOut;
    public AudioClip TypeSound;
    AudioSource SFX;

    public float delay;

    [Multiline]
    public string text;

    Text thisText;

    // Start is called before the first frame update
    void Start()
    {
        SFX = GetComponent<AudioSource>();
        thisText = GetComponent<Text>();

        StartCoroutine(TypeWrite());
    }

    IEnumerator TypeWrite()
    {
        foreach (char i in text)
        {
            thisText.text += i.ToString();

            SFX.pitch = Random.Range(0.8f, 1.2f);
            SFX.PlayOneShot(TypeSound);
            yield return new WaitForSeconds(delay);

        }
        yield return new WaitForSeconds(2f);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
