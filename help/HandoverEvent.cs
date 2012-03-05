using System;

namespace HighwaySimulation
{
	/// <summary>
	/// A handover event occurs when a call needs to be transfered from one station to another.
	/// </summary>
	internal class HandoverEvent : EventBase
	{
		#region Private fields
		readonly CallData _data;
		readonly Action _dropped;
		readonly Station _fromStation;
		readonly Action<uint, CallData> _newCallHandoverEventCallBack;
		readonly Action<uint, CallData> _newCallHangupEventCallBack;
		readonly Station _toStation;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="HandoverEvent"/> class.
		/// </summary>
		/// <param name="fromStation">The station that the call is being transferred from.</param>
		/// <param name="toStation">The station that the call is being transferred to.</param>
		/// <param name="dropped">Action that records if a call is dropped.</param>
		/// <param name="newCallHangupEventCallBack">Call back that adds a new hangu event to action queue.</param>
		/// <param name="newCallHandoverEventCallBack">Call back that adds a new handover event to action queue.</param>
		/// <param name="triggerTime">The trigger time for this event.</param>
		/// <param name="data">The call data associated with.</param>
		public HandoverEvent(
			Station fromStation,
			Station toStation,
			Action dropped,
			Action<uint, CallData> newCallHangupEventCallBack,
			Action<uint, CallData> newCallHandoverEventCallBack,
			uint triggerTime,
			CallData data )
			: base( triggerTime )
		{
			_fromStation = fromStation;
			_toStation = toStation;
			_dropped = dropped;
			_newCallHangupEventCallBack = newCallHangupEventCallBack;
			_newCallHandoverEventCallBack = newCallHandoverEventCallBack;
			_data = data;
		}

		/// <summary>
		/// The action to run when this event triggers.
		/// </summary>
		public override void Action()
		{
			_fromStation.ReleaseChannel();
			if( _toStation.ClaimChannel( true ) )
				if( _toStation.PositionIsInRange( _data.EndPosition ) )
					_newCallHangupEventCallBack( _data.EndTime, _data );
				else
					_newCallHandoverEventCallBack( _data.GetAbsoluteTimeForPosition( _toStation.EndPosition ), _data );
			else
				_dropped();
		}
	}
}