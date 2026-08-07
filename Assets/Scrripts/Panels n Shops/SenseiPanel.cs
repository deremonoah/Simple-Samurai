using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SenseiPanel : MonoBehaviour
{
    [Header("most recent score")]
    [SerializeField] float recentScore;//I plan to remove this

    [SerializeField] List<GameObject> styleUIs;
    [SerializeField] GameObject panelButton;
    int stylesKnown=1;
    //A list of the styles excluding simple so it isn't disable on start

    [Header("Helping rewards")]
    [SerializeField] int aproval;//for how much they like you
    [SerializeField] List<Item> itemRewards;
    //rewards for something else?

    private PlayerEquipedItemsManager pEquip;
    void Start()
    {
        panelButton.SetActive(false);
        foreach (GameObject style in styleUIs)
        {
            style.SetActive(false);
        }
        pEquip = FindObjectOfType<PlayerEquipedItemsManager>();//for giving player items
    }

    public void newStyles(int num)//should this not add them?
    {
        stylesKnown = num;
        
        FindObjectOfType<SoundManager>().PlaySound("sensei");
    }
    public void EnableButton()
    {
        panelButton.SetActive(true);
    }
    //so at certain points there should be new styles made available at current set up these being revealed in pairs or groups after like 3-5 waves
    //I likley want events to tell the player to visit the sensie panel which I should also disable the button while that isn't an option
    
    public int getNumberOfKnownStyles()
    {
        return stylesKnown;
    }

    public void RewardFromSensei(float score)
    {
        recentScore = score;
        aproval += 3;//might change amount to be vairable based ons score
        int rand = Random.Range(0, (int)score) + aproval;

        if (rand < 20)
        {
            //nothing given
            //increase liked more
            aproval += 5;
            return;
        }

        else if (rand >= 21 && rand <= 89)//with difficulty of the dusting maybe this is passed too often idk how to get better at it? other than faster clicking?
        {
            stylesKnown += 1;
        }
        else if (rand > 90)
        {
            //give player a hat of the quick if he doesn't have one
            rand = Random.Range(0, itemRewards.Count);
            pEquip.EquipItem(itemRewards[rand], false);
            //the premo stuff here
            //like increase pointer or get special curio or weapon
            //maybe style unlock here too?
        }
        //new style, pointer speed increase, universal pointer increase size, or maybe increase strike area?
        //maybe can give you specific curios like one that does something on blocking attacks
    }
}
