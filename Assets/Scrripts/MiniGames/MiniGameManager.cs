using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> UiElementsToDisable;//because when you work everything else fades away
    //states: outOfMiniGames, start of minigames, scoring/endSlide
    //which mini game is loaded (enabled to only take in that input)
    [SerializeField] List<GameObject> MiniGames;//have an enum refrencing correct order of games
    private MiniGame currentGame;
    //needs to deal with flow manager
    [Header("Day's Work Timer")]
    [SerializeField] float WorkDayDuration;
    [SerializeField] Transform sun;
    [SerializeField] Transform SunStartPos;
    [SerializeField] Transform SunEndPos;
    private Coroutine dayRoutine;

    void Start()
    {
        //starts out of mini game normally, but I might start in it just to test
        //OpenMiniGames();//here for now to test sun
    }

    // Update is called once per frame
    public void OpenMiniGames(int mg)
    {
        Debug.Log("MiniGameButton presesed " + mg);
        //enables the corect minigame, probably needs to be from a panel
        //but for now will just enable blacksmith game
        EnableHiddenUI(false);
        MiniGames[mg].SetActive(true);
        currentGame = MiniGames[mg].GetComponent<MiniGame>();
        Debug.Log("should have set mini game"); 

        if (dayRoutine==null)
        {
            Debug.Log("in day routine");
            dayRoutine = StartCoroutine(DaysWorkRoutine());
        }
        else { Debug.Log("DayRoutine is NOT null, so we didn't set it"); }
        
    }

    public IEnumerator DaysWorkRoutine()
    {
        //lerp sun from left to right, and tell curent miniGame to be done, at the end
        float timeElapsed = 0f;
        sun.position = SunStartPos.position;
        while(sun.position!=SunEndPos.position)
        {
            //oh I need it to move in an arch
            //can I use rotation of another Transform
            float t = timeElapsed / WorkDayDuration;
            sun.position = Vector2.Lerp(SunStartPos.position, SunEndPos.position, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //end the work day, so get score,
        //then drop down the scene, back to can spend money probably
        Debug.Log("past sun position");
        float scoreToReward = currentGame.CalculateScore();
        ResolveReward(scoreToReward);
        dayRoutine = null;
    }

    private void ResolveReward(float score)
    {
        //grab which mini game it is, index and do something?
        //check who its from, (sensai, blacksmith, etc) and give them a matching score

        //so an if statement
        EnableHiddenUI(true);
        var gfm=FindObjectOfType<GameFlowManager>();
        gfm.villageStillOpen();
        currentGame.gameObject.SetActive(false);
    }

    private void EnableHiddenUI(bool yee)
    {
        foreach(GameObject ui in UiElementsToDisable)
        {
            ui.SetActive(yee);
        }
    }
}
public enum miniGame { blackSmithHammer,plantSeeds,dustShelf}