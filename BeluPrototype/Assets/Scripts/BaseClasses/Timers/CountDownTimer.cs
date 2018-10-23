using System;
using System.Collections;
using UnityEngine;

namespace Utility.Timers
{
	[Serializable]
	public class CountDownTimer : Timer
	{
		public float CoolDown;
		MonoBehaviour _mono;

		//Constructores
		public CountDownTimer(GameObject MonoObject, float Presition = 0.01f)
		{
			this.Presition = Presition;
			_mono = MonoObject.GetComponent<MonoBehaviour>();
		}
		public CountDownTimer SetCoolDown(float CoolDown)
		{
			this.CoolDown = CoolDown;
			return this;
		}

		public override void StartCount()
		{
			if (isReady)
			{
				isReady = false;
				Time = CoolDown;
				_mono.StartCoroutine(CountDown());
			}
		}
		public override void StartCount( float From)
		{
			if (isReady)
			{
				isReady = false;
				Time = CoolDown;
				_mono.StartCoroutine(CountDown(From));
			}
		}

		public override void Reset()
		{
			isReady = true;
			Time = CoolDown;
			_mono.StopAllCoroutines();
		}
		public override void Pause()
		{
			_mono.StopAllCoroutines();
		}
		public override void Continue()
		{
			_mono.StartCoroutine(CountDown(Time));
		}

		IEnumerator CountDown()
		{
			while( Time >= 0)
			{
				Time -= Presition;
				yield return new WaitForSeconds(Presition);
			}
			if (Time <= 0)
			{
				isReady = true;
				Time = CoolDown;
				OnTimesUp();
				_mono.StopCoroutine(CountDown());
			}
		}
		IEnumerator CountDown(float From)
		{
			while ( Time >= 0)
			{
				Time = From;
				Time -= Presition;
				yield return new WaitForSeconds(Presition);
			}
			if (Time <= 0)
			{
				isReady = true;
				Time = CoolDown;
				OnTimesUp();
				_mono.StopCoroutine(CountDown(From));
			}
		}
	}
}
