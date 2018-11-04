using System;
using System.Collections;
using UnityEngine;

namespace Utility.Timers
{
    /// <summary>
    /// Esta clase permite ejecutar acciones al principio, durante y al final del contador.
    /// </summary>
    [Serializable]
    public class StateCountDownTimer : Timer
    {
        /// <summary>
		/// This action is executed when the count down starts.
		/// </summary>
		public Action OnTimesStart = delegate { };
        /// <summary>
		/// This action is executed during the count down.
		/// </summary>
		public Action<float,float> OnTimeUpdate = delegate { };
        /// <summary>
		/// This action is executed when the count down is over.
		/// </summary>
		public Action OnTimesEnd = delegate { };

        public float CoolDown;
        MonoBehaviour _mono;

        public StateCountDownTimer(GameObject MonoObject,float Presition = 0.01f)
        {
            this.Presition = Presition;
            _mono = MonoObject.GetComponent<MonoBehaviour>();
        }
        public StateCountDownTimer(GameObject MonoObject, float CoolDown, float Presition = 0.01f)
        {
            this.CoolDown = CoolDown;
            this.Presition = Presition;
            _mono = MonoObject.GetComponent<MonoBehaviour>();
        }

        public StateCountDownTimer SetCoolDown(float CoolDown)
        {
            this.CoolDown = CoolDown;
            return this;
        }

        /// <summary>
        /// Reestarts all the settings of the Timer to it´s default state.
        /// </summary>
        public override void Reset()
        {
            isReady = true;
            _currentTime = CoolDown;
            _mono.StopCoroutine(CountDown());
        }
        public override void StartCount()
        {
            if (isReady) _mono.StartCoroutine(CountDown());
        }
        /// <summary>
        /// Starts the CountDown from an off-setted time.
        /// </summary>
        /// <param name="From">The time in seconds where the timer starts to count.</param>
        public override void StartCount(float From)
        {
            if (isReady)
            {
                _currentTime = CoolDown;
                _currentTime -= From;
                _mono.StartCoroutine(CountDown());
            }
        }

        IEnumerator CountDown()
        {
            //CountDown.
            isReady = false;
            OnTimesStart();

            while (_currentTime >= 0)
            {
                _currentTime -= Presition;
                OnTimeUpdate(_currentTime,Presition);
                yield return new WaitForSeconds(Presition);
            }

            isReady = true;
            _currentTime = CoolDown;
            OnTimesEnd();
        }
    }
}
