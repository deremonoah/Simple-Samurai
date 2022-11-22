using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Dependancy")]
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] Text tutorialText;
    [SerializeField] GameObject SensaiSprite;
    [SerializeField] PanelTweening _panelTweening;

    [Header("Configuration")]
    [SerializeField] List<Color> tutorialColors;

    private bool _tutorialing;
    private TutorialState _tutorialState = TutorialState.tohold;
    private enemy TrainingDummy;
    private EnemysManager _enemyManager;
    private SoundManager _soundManager;

    bool buttonInput => Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0);

    private Image _tutorialImage;
    private int index;

    IEnumerator Start()
    {
        tutorialPanel.SetActive(false);
        _tutorialImage = tutorialPanel.GetComponent<Image>();
        _enemyManager = FindObjectOfType<EnemysManager>();
        _soundManager = FindObjectOfType<SoundManager>();

        if (_tutorialing)
        {
            _enemyManager.enemyWaves.Insert(0, Resources.Load<EnmWave>("Waves/Tutorial Wave"));
            FindObjectOfType<VillageDefense>().Tutorialing = true;
            Debug.Log("if");
        }
        else
        {
            yield break;
        }
        Debug.Log("started tutorial");

        SensaiSprite.SetActive(true);
        yield return new WaitForSeconds(1);
        tutorialPanel.SetActive(true);


        string text = "Hold Down the Space bar or Left Mouse button";
        UpdatePanel(tutorialColors[index++ % tutorialColors.Count], text);
        
       
        
        while(_tutorialState == TutorialState.tohold)
        {
            while (buttonInput == false)
            { 
                yield return null; 
            }
            _tutorialState = TutorialState.toRelease;
            
            text = "Now release the button while that sword icon is over the strike area ... the red shape";
            UpdatePanel(tutorialColors[index++ % tutorialColors.Count], text);
        }
        while (_tutorialState == TutorialState.toRelease)
        {
            while (!(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)))
            {
                yield return null;
            }
            TrainingDummy = _enemyManager.aliveEnemys[0];

            text = "The further right the pointer goes the more damage you do";
            UpdatePanel(tutorialColors[index++ % tutorialColors.Count], text);

            _tutorialState = TutorialState.toBlock;
        }
        
        yield return new WaitForSeconds(6.5f);

        
        text = "now block this attack";
        UpdatePanel(tutorialColors[index++ % tutorialColors.Count], text);

        yield return new WaitForSeconds(1.5f);

        TrainingDummy.AttackUI();
        yield return new WaitForSeconds(4f);

        
        text = "Lay low your enemy!";
        UpdatePanel(tutorialColors[index++ % tutorialColors.Count], text);

        List<WeaponEffect> noneEffects = new List<WeaponEffect>();
        noneEffects.Add(WeaponEffect.none);
        TrainingDummy.damgEnemy(900, noneEffects);

        
        while (_enemyManager.aliveEnemys.Count > 0)
        {
            yield return null;
            continue;
        }

        _tutorialState = TutorialState.congrats;


        
        text = "you seem to got the hang of it right?";
        UpdatePanel(tutorialColors[index++ % tutorialColors.Count], text);

        yield return new WaitForSeconds(4.2f);

        text = "well you are this villages only hope for survival so don't die";
        UpdatePanel(tutorialColors[index++% tutorialColors.Count], text);

        yield return new WaitForSeconds(7f);

        _tutorialState = TutorialState.done;

        if (_tutorialState == TutorialState.done)
        {
            _tutorialing = false;
            tutorialPanel.SetActive(false);
            SensaiSprite.SetActive(false);
            FindObjectOfType<VillageDefense>().Tutorialing = false;
        }
    }


    private void UpdatePanel(Color co, string text)
    {
        _tutorialImage.color = co;
        tutorialText.text = text;
        _soundManager.PlaySound("sensei");
        _panelTweening.ExecuteTween();
    }

    public void yesToTutorial()
    {
        _tutorialing = true;
    }
}
public enum TutorialState { tohold,toRelease,toBlock,congrats,done}

