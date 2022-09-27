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
    bool buttonInput => Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0);

    IEnumerator Start()
    {
        _enemyManager = GetComponent<EnemysManager>();
        if (_tutorialing)
        {
            _enemyManager.enemyWaves.Insert(0, Resources.Load<EnmWave>("Waves/Tutorial Wave"));
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
        
        while(_tutorialState == TutorialState.tohold)
        {
            while (buttonInput == false)
            { 
                yield return null; 
            }
            _tutorialState = TutorialState.toRelease;
            tutorialText.text = "Now release the button while that moving sword icon is over the strike area ... the red shape";
        }
        while (_tutorialState == TutorialState.toRelease)
        {
            while (!(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)))
            {
                yield return null;
            }
            TrainingDummy = _enemyManager.aliveEnemys[0];
            tutorialText.text = "The further right the pointer goes the more damage you do";
            _tutorialState = TutorialState.toBlock;
        }
        yield return new WaitForSeconds(5f);
        tutorialText.text = "now block this attack";
        yield return new WaitForSeconds(1.5f);

        TrainingDummy.StrikeUI();
        yield return new WaitForSeconds(4f);
        
        tutorialText.text = "Lay low your enemy!";
        List<WeaponEffect> noneEffects = new List<WeaponEffect>();
        noneEffects.Add(WeaponEffect.none);
        TrainingDummy.damgEnemy(900, noneEffects);

        yield return new WaitForSeconds(6f);
        _tutorialState = TutorialState.done;

        if (_tutorialState == TutorialState.done)
        {
            _tutorialing = false;
            tutorialPanel.SetActive(false);
            SensaiSprite.SetActive(false);
        }
    }

 /*   void Update()
    {
        if (_tutorialing)
        {
            //SensaiSprite.SetActive(true);
            tutorialPanel.SetActive(true);
            tutorialText.text = "Hold Down the Space bar or Left Mouse button";
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) && _tutorialState == TutorialState.tohold)
            {
                _tutorialState = TutorialState.toRelease;
                tutorialText.text = "Now release the button while that moving sword icon is over the strike area ... the red shape";
                
            }
            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && _tutorialState == TutorialState.toRelease)
            {
                tutorialText.text = "pretty simple right?";
                _tutorialState = TutorialState.toBlock;
            }
            if (_tutorialState == TutorialState.toBlock)
            {
                _timer -= Time.deltaTime;
                if (_timer >= 1)
                {
                    tutorialText.text = "now block this attack";
                    TrainingDummy.StrikeUI();
                    _tutorialState = TutorialState.done;
                }
            }

            if (_tutorialState == TutorialState.done)
            {
                _tutorialing = false;
                tutorialPanel.SetActive(false);
                //SensaiSprite.SetActive(false);
            }
        }
    }*/


    public void yesToTutorial()
    {
        _tutorialing = true;
        Debug.Log("yes tutorial");
    }
}
public enum TutorialState { tohold,toRelease,toBlock, done}

