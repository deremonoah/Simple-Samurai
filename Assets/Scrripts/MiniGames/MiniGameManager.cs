using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> UiElementsToDisable;//because when you work everything else fades away
    //states: outOfMiniGames, start of minigames, scoring/endSlide
    //which mini game is loaded (enabled to only take in that input)
    [SerializeField] List<GameObject> MiniGames;//have an enum refrencing correct order of games
    [SerializeField] List<GameObject> miniGameButtonsInShops;
    private MiniGame currentGame;
    //needs to deal with flow manager
    [Header("Day's Work Timer")]
    [SerializeField] float WorkDayDuration;
    [SerializeField] Transform sun;
    [SerializeField] Transform SunStartPos;
    [SerializeField] Transform SunEndPos;
    private Coroutine dayRoutine;

    private SenseiPanel sensei;
    private BlackSmithShop smith;
    private FarmShop farmers;
    

    void Start()
    {
        //starts out of mini game normally, but I might start in it just to test
        //OpenMiniGames();//here for now to test sun
        sensei = FindObjectOfType<SenseiPanel>();
        smith = FindObjectOfType<BlackSmithShop>();
        farmers = FindObjectOfType<FarmShop>();
        
    }

    public void RollToSeeIfTheyNeedHelp()
    {
        int rand = Random.Range(0, 10);//to give 3/10 none need help. and can go up so even more can miss
        int randAm = Random.Range(0, 3);//for getting less than all 3 turned on at once
        rand -= 3;//idk man
        if(rand>0 && rand<miniGameButtonsInShops.Count)
        {
            for(int lcv = rand;lcv < miniGameButtonsInShops.Count && rand<rand+randAm;lcv++)
            {
                miniGameButtonsInShops[lcv].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    public void OpenMiniGames(int mg)
    {
        //Debug.Log("MiniGameButton presesed " + mg);
        //enables the corect minigame, probably needs to be from a panel
        //but for now will just enable blacksmith game

        foreach(GameObject button in miniGameButtonsInShops)
        {
            button.SetActive(false);
        }

        EnableHiddenUI(false);
        MiniGames[mg].SetActive(true);
        currentGame = MiniGames[mg].GetComponent<MiniGame>();
        //Debug.Log("should have set mini game"); 

        if (dayRoutine==null)
        {
            //Debug.Log("in day routine");
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
        //Debug.Log("past sun position");
        Debug.Log("currentGame before calculating " + currentGame.name);
        float scoreToReward = currentGame.CalculateScore();
        ResolveReward(scoreToReward);
        dayRoutine = null;
    }

    private void ResolveReward(float score)
    {
        //grab which mini game it is, index and do something?
        //check who its from, (sensai, blacksmith, etc) and give them a matching score

        EnableHiddenUI(true);
        var gfm=FindObjectOfType<GameFlowManager>();
        gfm.villageStillOpen();
        helpedWho rewarder = currentGame.RewardFrom;
        currentGame.gameObject.SetActive(false);

        //TODO:this is where I will add giving of some kind of reward
        
        //we need a list of rewards for each mini game, should it be on the mini game?

        //also should have the npc thank the player for the help, even if they aren't given anything right away
        //if they have a text box over their head they could always be saying something

        switch(rewarder)
        {
            case helpedWho.Farmer:
                farmers.RewardFromFarmer(score);
                break;
            case helpedWho.Blacksmith:
                smith.RewardFromBlacksmith(score);
                break;
            case helpedWho.Sensei:
                sensei.RewardFromSensei(score);
                break;
        }

        //TODO: have refrence to the help buttons & disable the others, so you can only help someone once between games as often as it happens
    }

    private void EnableHiddenUI(bool yee)
    {
        foreach(GameObject ui in UiElementsToDisable)
        {
            ui.SetActive(yee);
        }
    }

    public void OpenHelpPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void CloseHelpPanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
public enum miniGame { blackSmithHammer,plantSeeds,dustShelf}