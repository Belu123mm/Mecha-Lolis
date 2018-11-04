using System;
using System.Collections;
using UnityEngine;

namespace Utility.Timers
{
	/// <summary>
	/// Esta clase se encarga permite ejecutar una acción al terminar el contador específicado.
	/// </summary>
	[Serializable]
	public class CountDownTimer : Timer
	{
		/// <summary>
		/// This action is executed when the count down is over.
		/// </summary>
		public Action OnTimesUp = delegate { };
		public MonoBehaviour _mono;
		public float CoolDown;

		//Constructores
		public CountDownTimer() { }
		public CountDownTimer(GameObject MonoObject, float CoolDown)
		{
			this.CoolDown = CoolDown;
			_mono = MonoObject.GetComponent<MonoBehaviour>();
		}
		/// <summary>
		/// Allows to Set the ammount of times that the courrutine is executed per second.
		/// </summary>
		/// <param name="Presition">The fraction of time that takes the function to be executed, in one second.</param>
		public CountDownTimer SetPrestion(float Presition)
		{
			this.Presition = Presition;
			return this;
		}

		public override void StartCount()
		{
			if (isReady)
			{
				isReady = false;
				_mono.StartCoroutine(CountDown());
			}
		}
		public override void StartCount( float From)
		{
			if (isReady)
			{
				isReady = false;
				_currentTime = CoolDown;
				_currentTime -= From;
				_mono.StartCoroutine(CountDown());
			}
		}
		public override void Reset()
		{
			isReady = true;
			_currentTime = CoolDown;
			_mono.StopCoroutine(CountDown());
		}

		IEnumerator CountDown()
		{
			//CountDown.
			while( _currentTime >= 0)
			{
				_currentTime -= Presition;
				yield return new WaitForSeconds(Presition);
			}

			isReady = true;
			_currentTime = CoolDown;
			OnTimesUp?.Invoke();
		}
	}
}
