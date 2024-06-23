using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingAtk : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    private float rotateNum = 0;

    // Update is called once per frame
    void Update()
    {
        //making it rotate 0.5 for tea pot rn, i could also leave it at 0 if i don't want it to move but would prob look weird that way
        rotateNum += rotateSpeed;
        this.transform.rotation = Quaternion.Euler(Vector3.forward * rotateNum);
    }
}
