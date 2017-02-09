using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using System.Linq;

public class EventManager : MonoBehaviour
{
    /*
States:
1 - both players entered (up to 30 sec) (possibly skip this time), -
    no enemies, don't spawn any/spawn 1 enemy (30-60 sec)
    (edit: spawn one tutorial enemy, ) 
2 - spawn waves of enemies(60-90 sec/1-1.5 min ) (possibly skip this state)
3 - more enemies(150 sec/ 1.5 min - 2.5 min)
4 - boss (2.5 min - 4.5 min)
5 - end scene (4.5 min to 5 min)
*/
    //Declare which states we'd like use
    //CAUTION: These are used in the exact order shown and are using the int backed index

    public enum States
    {
        MonoBehaviorScriptAwakeWorkAroundState,
        Init,
        Start,
        Wave1,
        Wave2,
        Boss,
        End,
        Done
    }
    public States statesAvail;

    //A map/dictionary/hashtable would be a more appropriate way to lookup these values
    public double[] stateTimeouts =
    {

        0.0,    // MonoBehaviourScriptAwakeWorkAroundState
        0.0,    // Init
        45.0,   // Start
        30.0,   // Wave1
        60.0,   // Wave2
        120.0,  // Boss
        15.0,   // End 
        0.0     // Done (ends with 0 seconds so as to not continue to any next state)
    };

    public double curStateTime;
    public double curStateTimeout;
    public double curStateEnterTime;
    public double gameEnterTime;
    public double gameTime;
    public double sinceStartEnterTime;
    public double sinceStartTime;

    public int killedEnemies;
    public int queuedEnemies;

    public int autoStartAfterClientsJoin = 999999999;
    public StateMachine<States> fsm;

    public static EventManager Instance;
    public float transparent = 0;
    public float opacity = 255;
    public float fadeTime = 2f;
    public float waitTime = 5f;

    public void addKilledEnemy(bool isBoss)
    {
        if (!isBoss)
            killedEnemies++;

        //only allow it in these states since queued count and killed would be 0 and equal during states w/o spawning
        if (fsm.State != States.Init && fsm.State != States.End && fsm.State != States.Done)
        {
            if (isBoss)
                fsm.ChangeState(States.End);
            else if (killedEnemies == queuedEnemies)
                fsm.ChangeState((States)(((int)fsm.State) + 1)); //Uses implicitly integer backed enum
        }


    }
    public void setQueuedEnemies(int enemies)
    {
        queuedEnemies = enemies;
    }
    private void Awake()
    {
        Status.SetTitle(false); //sets virtual world logo turns it off 

        if (ServerDiscovery.Instance.isServer)
        {


        }
        Instance = this;
        //Initialize State Machine Engine		
        fsm = StateMachine<States>.Initialize(this, States.MonoBehaviorScriptAwakeWorkAroundState);
        //fsm = StateMachine<States>.Initialize(this, States.Init);
        autoStartAfterClientsJoin = 999999999;
        //Used to set next state times
        fsm.Changed += change;

    }
    private void Start()
    {
        //Initialize game times once
        gameEnterTime = Time.realtimeSinceStartup;
        gameTime = 0;
    }

    // Handle resetting times for next states
    private void change(States newState)
    {
        curStateTimeout = stateTimeouts[(int)newState];
        curStateEnterTime = Time.realtimeSinceStartup;
        curStateTime = 0.0;
    }

    void Update()
    {

        curStateTime = Time.realtimeSinceStartup - curStateEnterTime;
        gameTime = Time.realtimeSinceStartup - gameEnterTime;

        if (sinceStartEnterTime > 0)
            sinceStartTime = Time.realtimeSinceStartup - sinceStartEnterTime;

        if (curStateTimeout > 0 && curStateTime > curStateTimeout)
            fsm.ChangeState((States)(((int)fsm.State) + 1)); //Uses implicitly integer backed enum


    }

    public string status = "";
    public string appendtxt = "";

#if UNITY_STANDALONE || UNITY_EDITOR
    void OnGUI()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        //Example of polling state 
        var state = fsm.State;
        string txt = "";

        txt += "State: " + state + "\n"
            + string.Format("Game   | Enter: {0,5:##0.0} Time: {1,5:##0.0}\n", gameEnterTime, gameTime)
            + string.Format("fromStart| Enter: {0,5:##0.0} Time: {1,5:##0.0}\n", sinceStartEnterTime, sinceStartTime)
            + string.Format("curState| Enter: {0,5:##0.0} Time: {1,5:##0.0} Timeout: {2,5:##0.0}\n", curStateEnterTime, curStateTime, curStateTimeout)
            + PhotonNetwork.playerList.Length + " Players Connected: \n"
            + PhotonNetwork.playerList.Aggregate<PhotonPlayer, string>("", (s, p) => s + p.name + "\n")
            + "QueuedEnemies: " + queuedEnemies + " killedEnemies: " + killedEnemies + "\n" + appendtxt;


        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.alignment = TextAnchor.UpperLeft;

        GUILayout.BeginArea(new Rect(50, 10, Screen.width / 2, Screen.height - 10));
        GUILayout.BeginHorizontal();


        //No players have joined yet


        if (fsm.State != States.Done)
        {

            //Do not prepend force to button when in Start state, via short circuiting
            if (GUILayout.Button((fsm.State + 1 != States.Start ? "Force " : "") + (States)(((int)fsm.State) + 1)))
                fsm.ChangeState((States)((int)fsm.State) + 1);

        }
        else
        {
            if (GUILayout.Button("Reset to init"))
                fsm.ChangeState(States.Init);
        }

        if (GUILayout.Button("Kill Knights"))
            killKnights();

        if (GUILayout.Button("Kill Ogres"))
            killOgres();


        if (GUILayout.Button("Spawn Ogre"))
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Instantiate("PF_Ogre_01_PhotonRsrc", Vector3.zero, Quaternion.identity, 0);
            }
        }
        if (GUILayout.Button("Destroy Ogre"))
            clearOgres();

        if (GUILayout.Button("bossSpawner"))
        {
            if (PhotonNetwork.isMasterClient)
            {
                GameObject.FindObjectOfType<bossSpawner>().spawnBoss();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.TextArea(txt, style);
        GUILayout.TextArea(status, style);
        GUILayout.EndArea();
    }
    void LateUpdate()
    {
        //appendtxt = "";
    }
#endif

    /* Init State
       Timeout: None
       Waits until two players (player count of 3) until starting the game (Start state)
    */
    private void Init_Enter()
    {

        curStateTime = 0;
        curStateTimeout = 0;
        curStateEnterTime = Time.realtimeSinceStartup;

        sinceStartEnterTime = 0;
        sinceStartTime = 0;
        killedEnemies = 0;

        status = "Waiting in init for more than " + autoStartAfterClientsJoin + " players...";
        StartCoroutine(WaitAndFade());
    }
    IEnumerator WaitAndFade()
    {
        yield return new WaitForSeconds(5);
        Status.SetBlackScreenAlpha(transparent, fadeTime);
    }

    //treating this as an init, since the state machine will be initialized to this
    //attempting to change state while in the initialized state's _Enter seems to crash it
    //also this is to wait for any other callbacks to be registered in awake or start for the state changes
    private void MonoBehaviorScriptAwakeWorkAroundState_Update()
    {
        fsm.ChangeState(States.Init);
    }
    private void Init_Update()
    {
        if (PhotonNetwork.playerList.Length > autoStartAfterClientsJoin) fsm.ChangeState(States.Start);

    }
    //private IEnumerator Init_Exit()
    //{

    //    status = "Starting in 3...";
    //    yield return new WaitForSeconds(1f);
    //    status = "Starting in 2...";
    //    yield return new WaitForSeconds(1f);
    //    status = "Starting in 1...";
    //    yield return new WaitForSeconds(1f);
    //}

    /*  Start State (30-60 sec)
        Timeout: 30 seconds
        both players entered (up to 30 sec) (possibly skip this time),
        no enemies, don't spawn any/spawn 1 enemy (30-60 sec)
    */
    private void Start_Enter()
    {
        sinceStartEnterTime = Time.realtimeSinceStartup;
        status = "Now in start";
    }

    private void Wave1_Enter()
    {
        status = "Now in Wave1";
    }

    private void Wave2_Enter()
    {
        status = "Now in Wave2";
    }

    private void Boss_Enter()
    {

        GameObject.FindObjectOfType<bossSpawner>().spawnBoss();
        status = "Now in Boss";
    }
    private void killKnights()
    {

        enemyKnightAIVerSimpler[] enemies = GameObject.FindObjectsOfType<enemyKnightAIVerSimpler>();
        //kill All knights
        foreach (enemyKnightAIVerSimpler e in enemies)
        {
            e.hitPoints = 0;
        }

    }
    private void killOgres()
    {
        SimpleEnemyOgreBossAI[] enemyOgres = GameObject.FindObjectsOfType<SimpleEnemyOgreBossAI>();
        //kill All knights
        foreach (SimpleEnemyOgreBossAI e in enemyOgres)
        {
            e.hitPoints = 0;
        }
    }

    private void killLightning()
    {
        LightningCloudBehavior[] enemyLightning = GameObject.FindObjectsOfType<LightningCloudBehavior>();
        //kill All knights
        foreach (LightningCloudBehavior e in enemyLightning)
        {

            PhotonNetwork.Destroy(e.gameObject);
        }
    }

    private void killWizard()
    {
        WizardMoveAndCast[] enemyWizard = GameObject.FindObjectsOfType<WizardMoveAndCast>();
        //kill All knights
        foreach (WizardMoveAndCast e in enemyWizard)
        {
            PhotonNetwork.Destroy(e.gameObject);
        }
    }

    private void clearOgres()
    {
        SimpleEnemyOgreBossAI[] enemyOgres = GameObject.FindObjectsOfType<SimpleEnemyOgreBossAI>();
        //kill All knights
        foreach (SimpleEnemyOgreBossAI e in enemyOgres)
        {
            PhotonNetwork.Destroy(e.gameObject);
        }
    }
    private void End_Enter()
    {
        killKnights();
        killOgres();
        killWizard();
        killLightning();
        status = "Now in End: Killing all Knights and Ogres";
    }


    private void Done_Enter()
    {
        status = "Now in start";
    }
}
