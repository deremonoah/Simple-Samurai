using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] Text tutorialText;
    [SerializeField] GameObject SensaiSprite;
    private bool _tutorialing;
    private TutorialState _tutorialState = TutorialState.tohold;
    private enemy TrainingDummy;
    private EnemysManager _enemyManager;
    private SoundManager _soundManager;

    bool buttonInput => Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0);

    private Image _tutorialImage;
    [SerializeField] List<Color> tutorialColors;

    IEnumerator Start()
    {
        _tutorialImage = tutorialPanel.GetComponent<Image>();
        _enemyManager = GetComponent<EnemysManager>();
        _soundManager = FindObjectOfType<SoundManager>();

        if (_tutorialing)
        {
            _enemyManager.enemyWaves.Insert(0, Resources.Load<EnmWave>("Waves/Tutorial Wave"));
            FindObjectOfType<VillageDefense>().Turotialing = true;
            Debug.Log("if");
        }
        else
        {
            yield break;
        }
        Debug.Log("started tutorial");

        yield return new WaitForSeconds(1);
        tutorialPanel.SetActive(true);
        SensaiSprite.SetActive(true);
        tutorialText.text = "Hold Down the Space bar or Left Mouse button";
        _soundManager.PlaySound("sensei");
        
        while(_tutorialState == TutorialState.tohold)
        {
            while (buttonInput == false)
            { 
                yield return null; 
            }
            _tutorialState = TutorialState.toRelease;
            _tutorialImage.color = tutorialColors[0];
            tutorialText.text = "Now release the button while that sword icon is over the strike area ... the red shape";
            _soundManager.PlaySound("sensei");
        }
        while (_tutorialState == TutorialState.toRelease)
        {
            while (!(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)))
            {
                yield return null;
            }
            TrainingDummy = _enemyManager.aliveEnemys[0];
            _tutorialImage.color = tutorialColors[1];
            tutorialText.text = "The further right the pointer goes the more damage you do";
            _soundManager.PlaySound("sensei");
            _tutorialState = TutorialState.toBlock;
        }
        yield return new WaitForSeconds(6.5f);
        _tutorialImage.color = tutorialColors[2];
        tutorialText.text = "now block this attack";
        _soundManager.PlaySound("sensei");
        yield return new WaitForSeconds(1.5f);

        TrainingDummy.AttackUI();
        yield return new WaitForSeconds(4f);

        _tutorialImage.color = tutorialColors[3];
        tutorialText.text = "Lay low your enemy!";
        _soundManager.PlaySound("sensei");
        List<WeaponEffect> noneEffects = new List<WeaponEffect>();
        noneEffects.Add(WeaponEffect.none);
        TrainingDummy.damgEnemy(900, noneEffects);

        
        while (_enemyManager.aliveEnemys.Count > 0)
        {
            yield return null;
            continue;
        }
        _tutorialState = TutorialState.congrats;


        _tutorialImage.color = tutorialColors[4];
        tutorialText.text = "you seem got the hang of it right?";
        _soundManager.PlaySound("sensei");
        yield return new WaitForSeconds(4.2f);
        tutorialText.text = "well you are this villages only hope for survival so don't die";
        _soundManager.PlaySound("sensei");
        yield return new WaitForSeconds(7f);
        _tutorialState = TutorialState.done;

        if (_tutorialState == TutorialState.done)
        {
            _tutorialing = false;
            tutorialPanel.SetActive(false);
            SensaiSprite.SetActive(false);
            FindObjectOfType<VillageDefense>().Turotialing = false;
        }
    }


    public void yesToTutorial()
    {
        _tutorialing = true;
    }
}
public enum TutorialState { tohold,toRelease,toBlock,congrats,done}

