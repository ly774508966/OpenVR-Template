using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas infoMenu;
    public Button startText;
    public Button exitText;
    public Button infoText;

	// Use this for initialization
	void Start () {

        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        infoText = infoText.GetComponent<Button>();

        infoMenu = infoMenu.GetComponent<Canvas>();

        quitMenu.enabled = false;
		infoMenu.enabled = false;
	
	}

    public void ExitPress()
    {

        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;

        infoText.enabled = false;

        infoMenu.enabled = false;

    }

    public void NoPress()
    {

        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;

        infoText.enabled = true;

        infoMenu.enabled = false;
    }

    public void InfoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = false;
        exitText.enabled = false;

        infoText.enabled = false;

        infoMenu.enabled = true;
    }

    public void StartLevel()
    {

        Application.LoadLevel(1);

    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
	
	// Update is called once per frame
	void Update () {
	
	}
}
