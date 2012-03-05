using System;
using System.Collections.Generic;
using System.Linq;

namespace HighwaySimulation
{
	/// <summary>
	/// Class that represents all stations along the highway.
	/// </summary>
	public class StationList
	{
		#region Private fields
		readonly uint _stationRange;
		readonly IList<Station> _stations;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="StationList"/> class and creates all stations.
		/// </summary>
		/// <param name="numberOfStations">The number of stations.</param>
		/// <param name="highwayLength">Length of the highway.</param>
		/// <param name="channelsPerStation">The number of channels per station.</param>
		/// <param name="reservedChannelsPerStation">The number of reserved channels per station.</param>
		public StationList(
			uint numberOfStations,
			uint highwayLength,
			uint channelsPerStation,
			uint reservedChannelsPerStation )
		{
			if( reservedChannelsPerStation > channelsPerStation )
				throw new ArgumentException( Messages.TooManyReservedChannels, "reservedChannelsPerStation" );
			if( highwayLength <= 0 )
				throw new ArgumentOutOfRangeException( "highwayLength", Messages.HighwayLengthNegative );
			if( numberOfStations == 0 )
				throw new ArgumentOutOfRangeException( "numberOfStations", Messages.NumberOfStationsZero );

			_stationRange = highwayLength / numberOfStations;
			_stations = new List<Station>();
			for( uint i = 0; i < numberOfStations; i++ )
				_stations.Add( new Station( channelsPerStation, reservedChannelsPerStation, i * _stationRange, _stationRange ) );
		}

		/// <summary>
		/// Gets the first station along the highway.
		/// </summary>
		public Station First
		{
			get { return _stations.First(); }
		}

		/// <summary>
		/// Gets the station covering a given position. This overload sets allowcloserangewrap to false.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>
		/// The station that covers the given position.
		/// </returns>
		public Station GetStationForPosition( uint position )
		{
			return GetStationForPosition( position, false );
		}

		/// <summary>
		/// Gets the station covering a given position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="allowCloseRangeWrap">if set to <c>true</c>, a position located just beyond the last position will return the first station.</param>
		/// <returns>
		/// The station that covers the given position.
		/// </returns>
		public Station GetStationForPosition( uint position, bool allowCloseRangeWrap )
		{
			if( position >= _stations.Last().EndPosition + ( allowCloseRangeWrap ? _stationRange / 2 : 0 ) )
				throw new ArgumentOutOfRangeException( "position", Messages.GetStationForInvalidPosition );
			if( position >= _stations.Last().EndPosition )
				return First;
			return _stations.First( s => s.PositionIsInRange( position ) );
		}
	}
}