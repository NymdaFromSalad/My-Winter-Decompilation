using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Automatically types into a string.")]
	public class StringTypewriter : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The string with the entire message to type out.")]
		public FsmString baseString;

		[Tooltip("The target result string (can be the same as the base string).")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString resultString;

		[Tooltip("The time between letters appearing.")]
		public FsmFloat pause;

		[Tooltip("True is realtime: continues typing while game is paused. False will subject time variable to the game's timeScale.")]
		public FsmBool realtime;

		[Tooltip("Send this event when finished typing.")]
		public FsmEvent finishEvent;

		[UIHint(UIHint.Description)]
		public string d1 = "     Optional Sounds Section:";

		[Tooltip("Check this to play sounds while typing.")]
		public bool useSounds;

		[Tooltip("Do not play a sound when it is a spacebar character.")]
		public bool noSoundOnSpaces;

		[ObjectType(typeof(AudioClip))]
		[Tooltip("The sound to play for each letter typed.")]
		public FsmObject typingSound;

		[Tooltip("The GameObject with an AudioSource component.")]
		public FsmOwnerDefault audioSourceObj;

		private int index;

		private int length;

		private float startTime;

		private float timer;

		private string message;

		private AudioSource audioSource;

		private AudioClip sound;

		public override void Reset()
		{
			baseString = null;
			resultString = null;
			pause = 0.05f;
			realtime = false;
			finishEvent = null;
			useSounds = false;
			noSoundOnSpaces = true;
			typingSound = null;
			audioSourceObj = null;
		}

		public override void OnEnter()
		{
			if (useSounds)
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(audioSourceObj);
				if (ownerDefaultTarget != null)
				{
					audioSource = ownerDefaultTarget.GetComponent<AudioSource>();
					if (audioSource == null)
					{
						Debug.Log("AudioSource component not found.");
						useSounds = false;
					}
					sound = typingSound.Value as AudioClip;
					if (sound == null)
					{
						Debug.Log("AudioClip not found.");
						useSounds = false;
					}
				}
				else
				{
					Debug.Log("AudioSource Object not found.");
					useSounds = false;
				}
			}
			message = baseString.Value;
			length = message.Length;
			resultString.Value = string.Empty;
			startTime = Time.realtimeSinceStartup;
		}

		public override void OnUpdate()
		{
			if (realtime.Value && Time.realtimeSinceStartup - startTime >= pause.Value)
			{
				DoTyping();
			}
			if (!realtime.Value)
			{
				timer += Time.deltaTime;
				if (timer >= pause.Value)
				{
					DoTyping();
				}
			}
		}

		public void DoTyping()
		{
			if (useSounds)
			{
				if (noSoundOnSpaces && message[index] != ' ')
				{
					audioSource.PlayOneShot(sound);
				}
				if (!noSoundOnSpaces)
				{
					audioSource.PlayOneShot(sound);
				}
			}
			resultString.Value += message[index];
			index++;
			timer = 0f;
			if (index >= length)
			{
				DoFinish();
			}
			startTime = Time.realtimeSinceStartup;
		}

		public void DoFinish()
		{
			Finish();
			if (finishEvent != null)
			{
				base.Fsm.Event(finishEvent);
			}
		}

		public override void OnExit()
		{
			resultString.Value = message;
		}
	}
}
