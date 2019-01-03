using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{

	public static UImanager _instanceUiManager = null;
	
	[SerializeField]private Text HpLabel;
	[SerializeField] private Text ElementLabel;
	string[] weaponTextList = new string[] {"火","電","氷"};
	
	private void Awake()
	{
		_instanceUiManager = this;
	}

	private void Start()
	{
		HpLabel.text = GameManager.instance.p_HP.ToString();
		
	}

	public void ChangeHpLabel(int value)//체력밸류UI
	{
		Debug.Log("hp 변경시도");
        if (value <= 0)
        {
            HpLabel.text = "0";
        }
        else
        {
            HpLabel.text = value.ToString();
        }
        
		
	}

	

	

	
}
