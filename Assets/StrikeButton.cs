using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrikeButton : MonoBehaviour
{
    bool inStrikeArea;
    [SerializeField] UnityEvent FunctionToExecute;
    [SerializeField] GameObject particlePrefab;

    private void Update()
    {
        if((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))&&inStrikeArea)
        {
            StartCoroutine(StartRoutine());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            inStrikeArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inStrikeArea = false;
    }

    IEnumerator StartRoutine()
    {
        Instantiate(particlePrefab, this.transform);
        yield return new WaitForSeconds(1f);
        FunctionToExecute.Invoke();
    }
}
