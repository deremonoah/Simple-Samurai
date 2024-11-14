using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenue : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void TutorialThenGame()
    {
        StartCoroutine(TutorialRoutine());
    }
    public IEnumerator TutorialRoutine()
    {
        yield return SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
        Debug.Log("got here");
        FindObjectOfType<TutorialManager>(true).yesToTutorial();
        yield return SceneManager.UnloadSceneAsync(0);
    }
}
