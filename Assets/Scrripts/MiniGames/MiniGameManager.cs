using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> UiElementsToDisable;//because when you work everything else fades away
    //states: outOfMiniGames, start of minigames, scoring/endSlide
    //which mini game is loaded (enabled to only take in that input)
    [SerializeField] List<GameObject> MiniGames;//have an enum refrencing correct order of games
    //needs to deal with flow manager

    void Start()
    {
        //starts out of mini game normally, but I might start in it just to test
    }

    // Update is called once per frame
    public void OpenMiniGames()
    {
        //enables the corect minigame, probably needs to be from a panel
        //but for now will just enable blacksmith game
    }
}
