using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject particlePrefab;
    [SerializeField] UnityEvent FunctionToExecute;
    [SerializeField] GameObject UIToDisable;

    [Header("Movement")]
    [SerializeField] Vector2 dir;
    [SerializeField] float movespeed;
    [SerializeField] float LifeSpan;


    private bool blocking;

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && blocking)
        {
            StartCoroutine(StartRoutine());
        }

        transform.Translate(dir * movespeed * Time.deltaTime);
        LifeSpan -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            blocking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        blocking = false;
    }

    IEnumerator StartRoutine()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        UIToDisable.SetActive(false);
        Instantiate(particlePrefab, this.transform);
        yield return new WaitForSeconds(1f);
        FunctionToExecute.Invoke();
    }
}
