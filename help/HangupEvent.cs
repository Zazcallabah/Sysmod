using System;

namespace HighwaySimulation
{
	/// <summary>
	/// Hangupevent occurs when a call is ended normally
	/// </summary>
	internal class HangupEvent : EventBase
	{
		#region Private fields
		readonly Action _hangup;
		readonly Station _station;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="HangupEvent"/> class.
		/// </summary>
		/// <param name="station">The station.</param>
		/// <param name="triggerTime">The trigger time.</param>
		/// <param name="hangup">The hangup.</param>
		public HangupEvent( Station station, uint triggerTime, Action hangup )
			: base( triggerTime )
		{
			_hangup = hangup;
			_station = station;
		}

		/// <summary>
		/// The action to run when this event triggers.
		/// </summary>
		public override void Action()
		{
			_hangup();
			_station.ReleaseChannel();
		}
	}
}