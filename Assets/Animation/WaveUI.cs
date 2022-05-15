using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {

	[SerializeField]
	wave_spawner spawner;

	[SerializeField]
	Animator waveAnimator;

	[SerializeField]
	Text waveCountdownText;

	[SerializeField]
	Text waveCountText;

	private wave_spawner.SpawnState previousState;

	// Use this for initialization
	void Start () {
		if (spawner == null)
		{
			Debug.LogError("No spawner referenced!");
			this.enabled = false;
		}
		if (waveAnimator == null)
		{
			Debug.LogError("No waveAnimator referenced!");
			this.enabled = false;
		}
		if (waveCountdownText == null)
		{
			Debug.LogError("No waveCountdownText referenced!");
			this.enabled = false;
		}
		if (waveCountText == null)
		{
			Debug.LogError("No waveCountText referenced!");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (spawner.State)
		{
			case wave_spawner.SpawnState.COUNTING:
				UpdateCountingUI();
				break;
			case wave_spawner.SpawnState.SPAWNING:
				UpdateSpawningUI();
				break;
        }

		previousState = spawner.State;
	}

	void UpdateCountingUI ()
	{
		if (previousState != wave_spawner.SpawnState.COUNTING)
		{
			waveAnimator.SetBool("WaveIncoming", false);
			waveAnimator.SetBool("WaveCountDown", true);
		    Debug.Log("COUNTING");
		}
		waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
	}

	void UpdateSpawningUI()
	{
		if (previousState != wave_spawner.SpawnState.SPAWNING)
		{
			waveAnimator.SetBool("WaveCountDown", false);
			waveAnimator.SetBool("WaveIncoming", true);

			waveCountText.text = spawner.NextWave.ToString();

		    Debug.Log("SPAWNING");
		}
	}
}