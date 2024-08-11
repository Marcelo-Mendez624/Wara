using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1.0f;

    void Awake()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        // Activa la transición
        transitionAnimator.SetTrigger("StartTransition");

        // Espera a que la animación de transición termine
        yield return new WaitForSeconds(transitionTime);

        // Carga la nueva escena
        SceneManager.LoadScene(sceneName);
    }
}