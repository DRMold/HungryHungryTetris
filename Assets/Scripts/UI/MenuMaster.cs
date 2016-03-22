using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Scripts/UI/Menu Master")]
public class MenuMaster : MonoBehaviour
{
	public GameObject mainPanel, readyPanel, optionsPanel;

	void Start ()
	{
		//mainPanel = GameObject.Find ("MainMenuPanel");
		mainPanel.SetActive (true);
		//readyPanel = GameObject.Find ("ReadyPanel");
		readyPanel.SetActive (false);
		//optionsPanel = GameObject.Find ("OptionsPanel");
		optionsPanel.SetActive (false);
	}

	public void showMainMenu() {
		mainPanel.SetActive (true);
		readyPanel.SetActive (false);
		optionsPanel.SetActive (false);
	}

	public void showReadyMenu() {
		mainPanel.SetActive (false);
		readyPanel.SetActive (true);
		optionsPanel.SetActive (false);
	}

	public void showOptionsMenu() {
		mainPanel.SetActive (false);
		readyPanel.SetActive (false);
		optionsPanel.SetActive (true);
	}
}

