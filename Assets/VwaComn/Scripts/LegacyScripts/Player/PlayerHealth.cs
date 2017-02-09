using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;

    public Image damageImage;

    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


	public bool isDead { get; private set; }
    bool damaged;


    void Awake ()
    {
        currentHealth = startingHealth;
		isDead = false;
    }


    void Update ()
    {
        //if(damaged)
        //{
        //    damageImage.color = flashColour;
        //}
        //else
        //{
        //    damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        //}
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

		//Commenting out for Alpha
//        Debug.Log(String.Format("player attacked for {0} damage! HP: {1}", amount, currentHealth.ToString()));
        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;
		//Commenting out for Alpha
//		Debug.Log("player died!");
		//RestartLevel();

		// stop player movement, but allow looking around
		var movement = gameObject.GetComponent<FirstPersonController>();
		movement.movementEnabled = false;

		// tell the logic controller that player is dead
		var logicGameObj = GameObject.FindGameObjectWithTag ("GameLogicController");
		//Debug.AssertFormat(logicGameObj != null, "the scene does not have a GameLogicController object");
        if (logicGameObj == null)
            return;
        
		var logicController = logicGameObj.GetComponent<IGameLogicController> ();
		//Debug.AssertFormat(logicController != null, "this GameLogicController object does not have GameLogicController interface script");
        if (logicController != null)
        {
            logicController.OnPlayerLose (this.gameObject);
        }
    }


}
