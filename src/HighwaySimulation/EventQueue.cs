using System;
using System.Collections.Generic;
using System.Linq;

namespace HighwaySimulation
{
	/// <summary>
	/// An event queue
	/// </summary>
	public class EventQueue : IEventQueue
	{
		#region Private fields
		readonly IDataGatherer _dataGatherer;
		readonly IRandomCallGenerator _generator;
		internal readonly SortedList<uint, IEvent> _innerQueue;
		readonly uint _stationRangeDiameter;
		internal readonly StationList _stations;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="EventQueue"/> class.
		/// </summary>
		public EventQueue(
			IDataGatherer dataGatherer,
			IRandomCallGenerator generator,
			uint numberOfStations,
			uint highwayLength,
			uint channelsPerStation,
			uint reservedChannelsPerStation )
		{
			if( dataGatherer == null )
				throw new ArgumentNullException( "dataGatherer" );
			if( generator == null )
				throw new ArgumentNullException( "generator" );

			_dataGatherer = dataGatherer;
			_generator = generator;
			_stations = new StationList(
				numberOfStations,
				highwayLength,
				channelsPerStation,
				reservedChannelsPerStation );
			_innerQueue = new SortedList<uint, IEvent>();
			_stationRangeDiameter = highwayLength / numberOfStations;
		}

		/// <summary>
		/// Seed the EventQueue by adding the first element.
		/// </summary>
		public void Poke()
		{
			AddCallEvent( 0 );
		}

		/// <summary>
		/// Removes the next event from the event queue and runs it.
		/// </summary>
		/// <returns>The time at which the event happened.</returns>
		public uint PerformNextEvent()
		{
			// take first event from queue
			IEvent first = _innerQueue.Values.First();
			_innerQueue.RemoveAt( 0 );

			// perform event
			first.Action();

			return first.TriggerTime;
		}

		/// <summary>
		/// Adds an event to the queue.
		/// </summary>
		/// <param name="triggertime">The trigger time for this event.</param>
		/// <param name="event">The event to add.</param>
		void AddEvent( uint triggertime, IEvent @event )
		{
			try
			{
				// innerqueue is implemented as a SortedList object, it will hold itself sorted by a key value, which in our case is triggertime.
				_innerQueue.Add( triggertime, @event );
			}
			// since event queue sorts events based on trigger time, trigger time must be unique.
			// in the somewhat unlikely case that we get duplicate trigger times, we just retry with next millisecond
			catch( ArgumentException )
			{
				AddEvent( triggertime + 1, @event );
			}
		}

		/// <summary>
		/// Adds the call event.
		/// </summary>
		/// <param name="lastcallstarttime">The lastcallstarttime.</param>
		internal void AddCallEvent( uint lastcallstarttime )
		{
			CallData newcall = _generator.GenerateRandomCall( lastcallstarttime );

			// add the new call event
			AddEvent(
				newcall.StartTime,
				new CallEvent(
					_stations.GetStationForPosition( newcall.StartPosition ),
					newcall,
					_dataGatherer.SignalCallBlocked,
					_dataGatherer.SignalCallStarted,
					AddCallEvent, // callback for adding new call events
					AddHangupEvent, // callback for adding hangup events
					AddHandoverEvent, // callback for adding handover events
					newcall.StartTime ) );
		}

		/// <summary>
		/// Adds the hangup event.
		/// </summary>
		/// <param name="triggertime">The triggertime.</param>
		/// <param name="data">The data.</param>
		internal void AddHangupEvent( uint triggertime, CallData data )
		{
			// Add the hang up event to queue
			AddEvent(
				triggertime,
				new HangupEvent(
					_stations.GetStationForPosition( data.EndPosition, false ),
					triggertime,
					_dataGatherer.SignalCallHangup // callback for call hangup
					) );
		}

		/// <summary>
		/// Adds the handover event.
		/// </summary>
		/// <param name="triggertime">The triggertime.</param>
		/// <param name="data">The data.</param>
		internal void AddHandoverEvent( uint triggertime, CallData data )
		{
			// get the position of the call at triggertime
			uint currentPosition = data.GetPositionForAbsoluteTime( triggertime );

			// find out which station we handover from, ( modify argument to add a small buffer, in case of rounding errors)
			Station handoverFromStation = _stations.GetStationForPosition( (uint) Math.Abs( currentPosition - ( (long) _stationRangeDiameter / 2 ) ) );

			// find out which station we handover to, ( modify argument to add small buffer in case of rounding errors)
			Station handoverToStation = _stations.GetStationForPosition( currentPosition + ( _stationRangeDiameter / 4 ), true );

			// Add the handover event to queue
			//		if handover is made from last station to first, modify call data to reflect this change. (using data.Wrap())
			AddEvent(
				triggertime,
				new HandoverEvent(
					handoverFromStation,
					handoverToStation,
					_dataGatherer.SignalCallDropped, // callback for if call is dropped
					AddHangupEvent, // callback to add a hangup event
					AddHandoverEvent, // callback to add a handover event
					triggertime,
					handoverToStation.Equals( _stations.First )
						? data.Wrap( triggertime )
						: data ) );
		}
	}
}