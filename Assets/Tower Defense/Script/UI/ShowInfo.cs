using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInfo : MonoBehaviour
{
    public Text unitName;
    public Image firstIcon;
    public Text firstText;
    public Image secondIcon;
    public Text secondText;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("UserClick", UserClick);
        HideUnitInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
	{
        EventManager.StopListening("UserClick", UserClick);
	}

    public void HideUnitInfo()
	{
        unitName.text = firstText.text = secondText.text = "";
        firstIcon.gameObject.SetActive(false);
        secondIcon.gameObject.SetActive(false);
        gameObject.SetActive(false);
	}

    public void ShowUnitInfo(UnitInfo info)
	{
        gameObject.SetActive(true);
        unitName.text = info.unitName;
        firstText.text = info.firstText;
        secondText.text = info.secondText;
        if (info.firstIcon != null)
		{
            firstIcon.sprite = info.firstIcon;
            firstIcon.gameObject.SetActive(true);
		}
        if (info.secondIcon != null)
		{
            secondIcon.sprite = info.secondIcon;
            secondIcon.gameObject.SetActive(true);
		}
	}

    private void UserClick(GameObject obj, string str)
	{
        HideUnitInfo();
        if (obj != null)
		{
            UnitInfo unitInfo = obj.GetComponent<UnitInfo>();
            if (unitInfo != null)
			{
                ShowUnitInfo(unitInfo);
			}
		}
	}
}
