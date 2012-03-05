using System;

namespace HighwaySimulation
{
	/// <summary>
	/// 
	/// </summary>
	public class CallEvent : EventBase
	{
		#region Private fields
		readonly Action<uint> _addNextCallCallBack;
		readonly Station _baseStation;
		readonly Action _blocked;
		readonly Action _callstart;
		readonly CallData _data;
		readonly Action<uint, CallData> _newCallHandoverEventCallBack;
		readonly Action<uint, CallData> _newCallHangupEventCallBack;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="CallEvent"/> class.
		/// </summary>
		/// <param name="baseStation">The base station.</param>
		/// <param name="data">The data.</param>
		/// <param name="blocked">The blocked.</param>
		/// <param name="callstart">The callstart.</param>
		/// <param name="addNextCallCallBack">The add next call call back.</param>
		/// <param name="newCallHangupEventCallBack">The new call hangup event call back.</param>
		/// <param name="newCallHandoverEventCallBack">The new call handover event call back.</param>
		/// <param name="triggerTime">The trigger time.</param>
		public CallEvent(
			Station baseStation,
			CallData data,
			Action blocked,
			Action callstart,
			Action<uint> addNextCallCallBack,
			Action<uint, CallData> newCallHangupEventCallBack,
			Action<uint, CallData> newCallHandoverEventCallBack,
			uint triggerTime )
			: base( triggerTime )
		{
			_baseStation = baseStation;
			_data = data;
			_blocked = blocked;
			_callstart = callstart;
			_addNextCallCallBack = addNextCallCallBack;
			_newCallHangupEventCallBack = newCallHangupEventCallBack;
			_newCallHandoverEventCallBack = newCallHandoverEventCallBack;
		}

		/// <summary>
		/// The action to run when this event triggers.
		/// </summary>
		public override void Action()
		{
			_callstart();
			if( _baseStation.ClaimChannel( false ) )
				if( _baseStation.PositionIsInRange( _data.EndPosition ) )
					_newCallHangupEventCallBack( _data.EndTime, _data );
				else
					_newCallHandoverEventCallBack( _data.GetAbsoluteTimeForPosition( _baseStation.EndPosition ), _data );
			else
				_blocked();
			_addNextCallCallBack( _data.StartTime );
		}
	}
}