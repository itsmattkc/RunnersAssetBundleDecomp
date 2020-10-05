using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/GameMode/title1st")]
public class GameModeTitle1st : MonoBehaviour
{
	private enum EventSignal
	{
		SIG_ONTOUCHED = 100
	}

	private const int NextSceneIndex = 1;

	private float m_alpha = 1f;

	private Texture2D m_texture;

	private TinyFsmBehavior m_fsm;

	private void Start()
	{
		this.m_texture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
		this.m_texture.SetPixel(0, 0, Color.white);
		this.m_texture.Apply();
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
			description.onFixedUpdate = true;
			this.m_fsm.SetUp(description);
		}
		SoundManager.AddTitleCueSheet();
		SoundManager.BgmPlay("bgm_sys_title", "BGM", false);
	}

	private void OnDestroy()
	{
		if (this.m_fsm)
		{
			this.m_fsm.ShutDown();
			this.m_fsm = null;
		}
	}

	private void OnGUI()
	{
	}

	private void FixedUpdate()
	{
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFading)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateFading(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_alpha = 0f;
			return TinyFsmState.End();
		case 4:
			this.m_alpha = Mathf.Clamp(this.m_alpha + Time.deltaTime, 0f, 1f);
			if (this.m_alpha >= 1f)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(1);
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void OnTouchTitle()
	{
		if (this.m_fsm != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
			this.m_fsm.Dispatch(signal);
		}
	}
}
