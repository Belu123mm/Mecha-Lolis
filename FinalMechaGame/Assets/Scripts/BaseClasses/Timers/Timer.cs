using System;
using UnityEngine;


namespace Utility.Timers
{
	public abstract class Timer
	{
		[SerializeField][Tooltip("False if the timer is currently being executed")]
		public bool isReady = true;
		protected float _currentTime;
		[SerializeField]
		[Tooltip("The number of times the count is executed in fraction of seconds")]
		protected float Presition;

		/// <summary>
		/// Returns the time in seconds that has passed since the start of the count down.
		/// </summary>
		public float CurrentTime { get { return _currentTime; } set { _currentTime = value; } }

		public virtual void StartCount() { }
		public virtual void StartCount(float From) { }
        /// <summary>
        /// Reestarts all the settings of the Timer to it´s default state.
        /// </summary>
        public virtual void Reset() { }
	}
}
