using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoseScript : MonoBehaviour
{
    
    [Header("References")]
    public Text text;
    public Collider collider;
    // Start is called before the first frame update
    void Start()
    { 
        text.enabled = false;
        collider = GetComponent<Collider>();
        Debug.Log("Hello World");
    }

    // On Collision no funciona debido a los navmesh teletransportando los rigidbody, no moviendolos con fuerzas
    // ( no registran como colisiones)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent")) {
            text.enabled = true;
            StartCoroutine(ReloadAfterDelay(2f));
        }
    }
    

    IEnumerator ReloadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
