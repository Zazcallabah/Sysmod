using System;
using System.Globalization;
using HighwaySimulation;

namespace SimHighway
{
	/// <summary>
	/// Standard impl. of data gatherer.
	/// </summary>
	internal class DataGatherer : IDataGatherer
	{
		#region Private fields
		readonly uint _id;
		bool _recording;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="DataGatherer"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public DataGatherer( uint id )
		{
			_recording = false;
			_id = id;
		}

		/// <summary>
		/// Gets or sets the call started.
		/// </summary>
		/// <value>The call started.</value>
		public ulong CallStarted { get; private set; }

		/// <summary>
		/// Gets or sets the call blocked.
		/// </summary>
		/// <value>The call blocked.</value>
		public ulong CallBlocked { get; private set; }

		/// <summary>
		/// Gets or sets the call dropped.
		/// </summary>
		/// <value>The call dropped.</value>
		public ulong CallDropped { get; private set; }

		/// <summary>
		/// Gets or sets the call hangup.
		/// </summary>
		/// <value>The call hangup.</value>
		public ulong CallHangup { get; private set; }

		#region IDataGatherer Members
		/// <summary>
		/// Signals to the data gatherer that data should start being recorded.
		/// </summary>
		public void Record()
		{
			CallStarted = CallBlocked = CallDropped = CallHangup = 0;
			_recording = true;
		}

		/// <summary>
		/// Signals that a call started.
		/// </summary>
		public void SignalCallStarted()
		{
			CallStarted++;
		}

		/// <summary>
		/// Signals that a call was blocked.
		/// </summary>
		public void SignalCallBlocked()
		{
			CallBlocked++;
		}

		/// <summary>
		/// Signals that a call was dropped.
		/// </summary>
		public void SignalCallDropped()
		{
			CallDropped++;
		}

		/// <summary>
		/// Signals that a call was ended.
		/// </summary>
		public void SignalCallHangup()
		{
			CallHangup++;
		}
		#endregion

		/// <summary>
		/// Implements the operator +. This returns a new data gatherer containing all the data from the two operands.
		/// The new id is copied from the operand with the largest id, that way the id holds the total number of gatherers.
		/// </summary>
		/// <param name="gathererfirst">The first gatherer.</param>
		/// <param name="gatherersecond">The second gatherer.</param>
		/// <returns>The result of the operator.</returns>
		public static DataGatherer operator +( DataGatherer gathererfirst, IDataGatherer gatherersecond )
		{
			if( gatherersecond is DataGatherer )
				return gathererfirst + (DataGatherer) gatherersecond;
			throw new InvalidOperationException();
		}

		/// <summary>
		/// Implements the operator +. This returns a new data gatherer containing all the data from the two operands.
		/// The new id is copied from the operand with the largest id, that way the id holds the total number of gatherers.
		/// </summary>
		/// <param name="gathererfirst">The first gatherer.</param>
		/// <param name="gatherersecond">The second gatherer.</param>
		/// <returns>The result of the operator.</returns>
		public static DataGatherer operator +( DataGatherer gathererfirst, DataGatherer gatherersecond )
		{
			return new DataGatherer( Math.Max( gathererfirst._id, gatherersecond._id ) )
			{
				CallBlocked = gathererfirst.CallBlocked + gatherersecond.CallBlocked,
				CallDropped = gathererfirst.CallDropped + gatherersecond.CallDropped,
				CallHangup = gathererfirst.CallHangup + gatherersecond.CallHangup,
				CallStarted = gathererfirst.CallStarted + gatherersecond.CallStarted,
				_recording = false
			};
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return _recording
				? string.Format(
					CultureInfo.CurrentCulture,
					@"replication number = {0}
total calls = {1}
blocked calls = {2}
dropped calls = {3}
percent blocked calls = {4:F4}%
percent dropped calls = {5:F4}%
-------------------------------------------
",
					_id,
					CallStarted,
					CallBlocked,
					CallDropped,
					CallBlocked * 100.0 / CallStarted,
					CallDropped * 100.0 / CallStarted )
				: string.Format(
					CultureInfo.CurrentCulture,
					@"average total calls = {0:F4}
average blocked calls = {1:F4}
average dropped calls = {2:F4}
percent blocked calls = {3:F4}%
percent dropped calls = {4:F4}%",
					(double) CallStarted / _id,
					(double) CallBlocked / _id,
					(double) CallDropped / _id,
					( (double) CallBlocked / _id ) * 100.0 / ( (double) CallStarted / _id ),
					( (double) CallDropped / _id ) * 100.0 / ( (double) CallStarted / _id ) );
		}
	}
}