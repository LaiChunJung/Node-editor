using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DNE;
using UnityEngine.UI;

public class DialogSystemTest : MonoBehaviour
{

	public BuildObject DialogData;
	public Text DialogText;
	public AudioSource Source;
	public Animator CharacterAnimator;

	private bool mIsFinished = false;

	private void Start()
	{
		FirstDialog();
	}

	private void FirstDialog()
	{
		//DialogData = Resources.Load("Builds/Build") as BuildObject;
		DialogData = DialogData.Get();
		//DialogData.Init();
		setText();
		setAudio();
		mIsFinished = false;
	}

	private void initDiaLogData()
	{
		if (DialogData != null)
		{
			DialogData = Resources.Load("Builds/Build") as BuildObject;
			DialogData = DialogData.Get();
			setText();
			setAudio();
			//CharacterAnimator.Play(DialogData.GetCurrent().AnimatorState);
			mIsFinished = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q))&& DialogData != null && !mIsFinished)
		{
			//CharacterAnimator.Play(DialogData.GetCurrent().AnimatorState);
			OnButtonClick(DialogData.GetCurrent().Triggers[0]);
		}
	}

	private void setText()
	{
		DialogText.text = DialogData.GetCurrent().Text;
	}

	private void setAudio()
	{
		Source.clip = DialogData.GetCurrent().Clip;
		Source.Play();
	}

	private void OnButtonClick(string trigger)
	{
		BuildNode next = DialogData.Next(trigger);
		if (next != null)
		{
			setText();
			setAudio();
		}
		else
		{
			mIsFinished = true;
			initDiaLogData();
		}
	}
}
