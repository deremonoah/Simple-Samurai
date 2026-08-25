using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanting : MiniGame
{
    [SerializeField] Transform player;
    private Vector3 posToReturn;
    [SerializeField] float durationInEachRow;
    [SerializeField] GameObject seed;
    [SerializeField] List<float> yPosToPlantRows;//list for different hights of the rows
    [SerializeField] Vector2 xPosesToSwap;//when you go from one row to the next, like left most x & right most x
    private Vector3 lastSeedPlanted;//will score based on their distance to eachother
    [SerializeField] int seedsToUse;//you have a limited number of seeds
    private int maxRefSeeds;
    private Coroutine PlantRoutine;
    List<GameObject> seedsPlanted = new();
    [SerializeField]private List<float> PlantScores;//distances between the plants. we probably want a particular average, and just compare if you are within certain ranges of it
    //I need a way to help players get the seeds a certain distance apart
    //would be cool if the music helped them space the seeds apart
    //like tapping along on the down beat? gets you close
    //so working songs often sung so, not sure about a down beat or how to get it to sound like singing with instruments or keep it 
    //indistinct and have a down beat?

    void Awake()
    {
        posToReturn = player.position;
        maxRefSeeds = seedsToUse;
    }

    void OnEnable()
    {
        ResetMiniGame();
        
        PlantRoutine = StartCoroutine(WalkingRoutine());
    }

    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))&&seedsToUse>0)//should really have set input button or buttons & handle based on platform
        {
            GameObject se=Instantiate(seed, player.position, player.rotation);//could make rotation random
            seedsPlanted.Add(se);
            seedsToUse--;//player pos right now = new plant pose
            if(lastSeedPlanted!=Vector3.zero)
            {
                PlantScores.Add(Vector3.Distance(lastSeedPlanted, player.position));
            }

            lastSeedPlanted = player.position;
        }
    }

    private IEnumerator WalkingRoutine()//once we hit end of day we can stop the coroutine
    {
        int rowOn = 0;
        Vector3 targetPos = new Vector3(xPosesToSwap.y, yPosToPlantRows[rowOn]);
        Vector3 startPos = new Vector3(xPosesToSwap.x, yPosToPlantRows[rowOn]);
        player.position = startPos;
        float timeElapsed = 0;
        while (seedsToUse>0 && player.position!=targetPos)//moves left across the first row
        {
            float t = timeElapsed / durationInEachRow;
            player.position = Vector3.Lerp(startPos,targetPos,t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        rowOn++;
        //now has to move down to the second row

        targetPos = new Vector3(xPosesToSwap.y, yPosToPlantRows[rowOn]);
        startPos = new Vector3(xPosesToSwap.y, yPosToPlantRows[rowOn-1]);//yeah hard coded oh well
        timeElapsed = 0;
        player.gameObject.GetComponent<SpriteRenderer>().flipX = false;

        while (seedsToUse > 0 && player.position != targetPos)//moves left across the first row
        {
            float t = timeElapsed / 1f;//sure 1 second between rows
            player.position = Vector3.Lerp(startPos, targetPos, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //second row
        targetPos = new Vector3(xPosesToSwap.x, yPosToPlantRows[rowOn]);
        startPos = new Vector3(xPosesToSwap.y, yPosToPlantRows[rowOn]);
        timeElapsed = 0;

        while (seedsToUse > 0 && player.position != targetPos)//moves left across the first row
        {
            float t = timeElapsed / durationInEachRow;
            player.position = Vector3.Lerp(startPos, targetPos, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        rowOn++;

        //move down to row 3
        targetPos = new Vector3(xPosesToSwap.x, yPosToPlantRows[rowOn]);
        startPos = new Vector3(xPosesToSwap.x, yPosToPlantRows[rowOn - 1]);//yeah hard coded oh well
        timeElapsed = 0;
        player.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        while (seedsToUse > 0 && player.position != targetPos)//moves left across the first row
        {
            float t = timeElapsed / 1f;//sure 1 second between rows
            player.position = Vector3.Lerp(startPos, targetPos, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //walk row 3
        targetPos = new Vector3(xPosesToSwap.y, yPosToPlantRows[rowOn]);
        startPos = new Vector3(xPosesToSwap.x, yPosToPlantRows[rowOn]);
        timeElapsed = 0;
        while (seedsToUse > 0 && player.position != targetPos)//moves left across the first row
        {
            float t = timeElapsed / durationInEachRow;
            player.position = Vector3.Lerp(startPos, targetPos, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //just realized I could have used moveTowards, and just had points, that would be better
    }

    public override float CalculateScore()
    {
        float score = 0;

        foreach(float sco in PlantScores)
        {
            //starts as distance .2=80. to do that we get (1-|sco-1|)*100
            //1-.02 distance then multiply by 100
            score += (1-Mathf.Abs(sco-1))*100;
        }
        score = score / PlantScores.Count;

        foreach (GameObject se in seedsPlanted)
        {
            Destroy(se);
        }

        return score;
    }

    private void ResetMiniGame()
    {
        //object pooling probably better
        PlantScores.Clear();
        seedsPlanted.Clear();

        player.position = posToReturn;
        seedsToUse = maxRefSeeds;
    }
}
