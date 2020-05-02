using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    public float coolDownTime = 10f;
    public GameObject actionPrefab;
    public GameObject highLightIcon;
    public GameObject coolDownIcon;
    public Text coolDownText;
    private enum MyState
	{
        Active,
        HighLight,
        CoolDown
	}
    private MyState myState = MyState.Active;
    private GameObject userAction;
    private float coolDownCounter;

    void OnEnable()
	{
        EventManager.StartListening("UserUIClick", UserUIClick);
        EventManager.StartListening("ActionStart", ActionStart);
        EventManager.StartListening("ActionCancel", ActionCancel);
	}

    // Start is called before the first frame update
    void Start()
    {
        StopCoolDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (myState == MyState.CoolDown)
        {
            if (coolDownCounter > 0f)
            {
                coolDownCounter -= Time.deltaTime;
                UpdateCoolDownText();
            }
            else if (coolDownCounter <= 0f)
            {
                StopCoolDown();
            }
        }
    }

    void OnDisable()
	{
        EventManager.StopListening("UserUIClick", UserUIClick);
        EventManager.StopListening("ActionStart", ActionStart);
        EventManager.StopListening("ActionCancel", ActionCancel);
    }

    private void StopCoolDown()
	{
        myState = MyState.Active;
        coolDownCounter = 0f;
        coolDownIcon.gameObject.SetActive(false);
        coolDownText.gameObject.SetActive(false);
	}

    private void StartCoolDown()
	{
        myState = MyState.CoolDown;
        coolDownCounter = coolDownTime;
        coolDownIcon.gameObject.SetActive(true);
        coolDownText.gameObject.SetActive(true);
	}

    private void UpdateCoolDownText()
	{
        coolDownText.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
	}

    private void UserUIClick(GameObject obj, string str)
	{
        if (obj == gameObject)
		{
            if (myState == MyState.Active)
			{
                highLightIcon.SetActive(true);
                userAction = Instantiate(actionPrefab);
                myState = MyState.HighLight;
			}
		}
        else if (myState == MyState.HighLight)
		{
            highLightIcon.SetActive(false);
            myState = MyState.Active;
		}
	}

    private void ActionStart(GameObject obj, string str)
	{
        if (obj == userAction)
		{
            userAction = null;
            highLightIcon.SetActive(false);
            StartCoolDown();
		}
	}

    private void ActionCancel(GameObject obj, string str)
	{
        if (obj == userAction)
		{
            userAction = null;
            highLightIcon.SetActive(false);
            myState = MyState.Active;
		}
	}
}
