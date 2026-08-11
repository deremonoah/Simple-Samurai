using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerDefense : MonoBehaviour
{
    //0= none 1=pit 2= palasade 3= spikes
    [Header("ui positions")]
    [SerializeField] List<Transform> inCombatPos= new();
    [SerializeField] List<Transform> inShopsPos = new();

    [Header("slot & equips")]
    public List<PlayerTrap> EquipedDefense;
    [SerializeField] int numberOfPlots;
    [SerializeField] List<GameObject> plotsForTraps;

    [Header("UI prefabs")]
    [SerializeField] Transform ParentToUI;
    [SerializeField] GameObject palisade;//need to be able to grab the image for fill amount. getcomponent in children?
    [SerializeField] GameObject pit;
    [SerializeField] GameObject spikes;

    [Header("prices & stuff")]
    [SerializeField] int pitCost;
    [SerializeField] int palisadeCost;
    [SerializeField] int spikesCost;
    [SerializeField] int IncreasePlotCost;
    //[SerializeField] List<Dragable> DefenseDragables;
    //[SerializeField] DropZone EquipedDefenseSlot;
    public GameObject DefenseButton;
    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        EquipedDefense = new();
        foreach(GameObject plot in plotsForTraps)
        {
            plot.SetActive(false);
            Image img = plot.transform.GetChild(0).GetComponent<Image>();
            img.color = new Color(1, 1, 1, 0);
        }
        
        DefenseButton.SetActive(false);
        UpdatePlotsEquipUI();
    }

    public bool isDefended()
    {
        for (int lcv = 0; lcv < numberOfPlots; lcv++)
        {
            if(EquipedDefense.Count==numberOfPlots)
            {
                if (EquipedDefense[lcv].isArmed())
                {
                    return true;
                }
            }
        }
            return false;
    }

    public void DefendPlayer(enemyStats enmy,float Damg)
    {
        
        for(int lcv =0; lcv<EquipedDefense.Count;lcv++)
        {
            if(EquipedDefense[lcv].isArmed())
            {
                Debug.Log(EquipedDefense[lcv].name + " defended player");
                EquipedDefense[lcv].DefendPlayer(Damg, enmy);
                break;//because they hit the first defense
            }
        }
    }

    public void RearmTraps()
    {
        for(int lcv=0;lcv<EquipedDefense.Count;lcv++)
        {
            EquipedDefense[lcv].ReArmTrap();
        }
    }

    public void inCombatHudUpdate(bool inC)//will be called by the gameflowManager
    {
        List<Transform> refrence = new();
        /*if (inC)
        {
            refrence = inCombatPos;
        }
        else
        {
            refrence = inShopsPos;
        }*/

        refrence = inShopsPos;//feel like this just reads better if you look down there

        for (int lcv = 0; lcv < EquipedDefense.Count && lcv < numberOfPlots; lcv++)
        {
            //set images on the plots based on what we have equiped
            EquipedDefense[lcv].gameObject.transform.position = refrence[lcv].position;
        }
    }

    //this is the same as the above that I made later
    public void IncreaseSlotsButton()
    {
        numberOfPlots++;
        //enable slot uis
        UpdatePlotsEquipUI();
    }

    public void TrapPressed(GameObject prefab)
    {
        var ui = Instantiate(prefab);
        ui.transform.SetParent(ParentToUI);
        PlayerTrap trap = ui.GetComponent<PlayerTrap>();
        EquipedDefense.Insert(0, trap);

        UpdatePlotsEquipUI();
    }

    public void TrapPurchase(int trapNum)
    {
        PlayerTrapType trap = (PlayerTrapType)trapNum;
        int cost = 0;
        GameObject prefab=null;
        if(trap==PlayerTrapType.pit)
        { cost = pitCost; prefab = pit; }

        else if(trap== PlayerTrapType.palisade)
        { cost = palisadeCost; prefab = palisade; }

        else if (trap == PlayerTrapType.spikes)
        { cost = spikesCost; prefab = spikes; }

        if (gm.canBuy(cost))//take away money & if you don't own it unlock it
        {
            gm.playerCoins -= cost;
            TrapPressed(prefab);
        }
    }

    private void UpdatePlotsEquipUI()//only called outside of combat
    {
        TrimIfListTooLong();
        //this is for the panel displaying how many plots (brown squares a player has)
        //what you have equiped in them
        for (int lcv=0;lcv<plotsForTraps.Count && lcv<numberOfPlots;lcv++)
        {
            plotsForTraps[lcv].SetActive(true);
        }
        
        for(int lcv=0;lcv<EquipedDefense.Count && lcv < numberOfPlots; lcv++)
        {
            //set images on the plots based on what we have equiped
            Image img =plotsForTraps[lcv].transform.GetChild(0).GetComponent<Image>();//have to write it this way cause getComponentInChildren will grab the component from parent if it has one
            img.sprite= EquipedDefense[lcv].displaySprite; ;
            img.color = new Color(1, 1, 1, 1);
        }
        inCombatHudUpdate(false);
    }

    private void TrimIfListTooLong()//we should never get more than 1 extra in the list
    {
        if(EquipedDefense.Count>numberOfPlots)//3>2
        {
            Destroy(EquipedDefense[EquipedDefense.Count - 1].gameObject);//destroy object
            EquipedDefense.RemoveAt(EquipedDefense.Count - 1);//clear empty spot
        }
    }

    public void EnableDefenseButton()
    {
        DefenseButton.SetActive(true);
    }

}
public enum PlayerTrapType { pit,palisade,spikes}