using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

public class HpAndTimeScale : MonoBehaviour
{
    [ReadOnly]public static bool hit,gotKill;

    public Canvas pauseCanvas,deathCanvas;
    public GameObject invulerabilityBar;
    public Text timeCanvas, scoreCanvas,deathScoreCanvas, deathScoreCanvas2;
    public Slider hpCanvas, mpCanvas,mpMinCanvas;
    public float hpScale,hpMax,hpCurrent, mpScale, mpMax, mpCurrent,mpMin,mpMinScale, timer, invulnerabilityTime,invulScale;
    public Entity player;
    static EntityManager entityManager;
    HpAndTimeRunTime playerComp;
    float timeElaspsed;
    bool timestopped;
    static int score;
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.Active.EntityManager;
        score = 0;
        //playerComp = entityManager.GetComponentData<HpAndTimeRunTime>(player);
        timer = 0;
        timeElaspsed = 0;
        pauseCanvas.gameObject.SetActive(PauseSystem.Paused);
    }

    // Update is called once per frame
    void Update()
    {
       
       // print(Time.deltaTime);
        GetReferences();
        if (!PauseSystem.Paused&&GetAlive())
        {
            if (!timestopped)
                timeElaspsed += Time.deltaTime;

            if (timer > 0)
                timer -= Time.deltaTime;

            if (timer < 0)
                timer = 0;
        }
        hpScale = hpCurrent / hpMax;
        mpScale = mpCurrent / mpMax;
        mpMinScale = mpMin / mpMax;
        invulScale = timer / invulnerabilityTime;

        mpMinCanvas.value = mpMinScale;
        hpCanvas.value = hpScale;
        mpCanvas.value = mpScale;

        if (hit)
            DamagePlayer();

        if (gotKill)
            IncreaseHealth();

        timeCanvas.text = string.Format("{0:F1}", timeElaspsed);
        scoreCanvas.text = "Score: " +  score ;
        deathScoreCanvas2.text = "Score: " + score;
        deathScoreCanvas.text  = "Score: " + score;

        var scale = invulerabilityBar.GetComponent<RectTransform>().localScale;
        invulerabilityBar.GetComponent<RectTransform>().localScale = new Vector3( invulScale, scale.y, scale.z);

        pauseCanvas.gameObject.SetActive(PauseSystem.Paused&&GetAlive());
        deathCanvas.gameObject.SetActive(GetDead());

    }

    public void GetReferences()
    {
        if(player!=null)
        playerComp = entityManager.GetComponentData<HpAndTimeRunTime>(player);

        timestopped = playerComp.timeStopped;

        hpMax = playerComp.hpMax;
        hpCurrent = playerComp.hpCurrent;

        mpMax = playerComp.mpMax;
        mpCurrent = playerComp.mpCurrent;
        mpMin = playerComp.mpMin;
    }

    public bool GetAlive()
    {
        return hpCurrent >= 0;
    }
    public bool GetDead()
    {
        return !(hpCurrent >= 0);
    }

    public static void HitPlayer()
    {
        hit = !hit;
        Readonlys.hit = hit;
    }
    public static void Scored()
    {
        gotKill = !gotKill;
    }

    public void DamagePlayer()
    {

        hit = false;
        Readonlys.hit = hit;
        GetReferences();
        if (timer <= 0)
        {
            entityManager.SetComponentData<HpAndTimeRunTime>(player, new HpAndTimeRunTime
            {
                hpCurrent = hpCurrent - 25,
                hpMax = hpMax,
                mpCurrent = mpCurrent,
                mpMax = mpMax,
                hps = playerComp.hps,
                mps = playerComp.mps,
                mpMin = playerComp.mpMin
            });
            timer = invulnerabilityTime;
        }
    }
    public void IncreaseHealth()
    {

        gotKill = false;
        GetReferences();

        entityManager.SetComponentData<HpAndTimeRunTime>(player, new HpAndTimeRunTime
        {
            hpCurrent = hpCurrent,
            hpMax = hpMax+1,
            mpCurrent = mpCurrent,
            mpMax = mpMax+2,
            hps = playerComp.hps,
            mps = playerComp.mps,
            mpMin = playerComp.mpMin
        });

    }
    public float getTimeElasped()
    {
        return timeElaspsed;
    }
    public static void incrementScore()
    {
        score++;
        Scored();
    }
}
public struct Readonlys
{
    [ReadOnly] public static bool hit;

    public static void HitPlayer()
    {
        HpAndTimeScale.HitPlayer();
    }
} 
