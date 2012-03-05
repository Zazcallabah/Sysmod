using System;

namespace HighwaySimulation
{
	/// <summary>
	/// Class representing a base station positioned along the highway.
	/// </summary>
	public class Station
	{
		#region Private fields
		readonly uint _length;
		readonly uint _reservedChannels;
		readonly uint _start;
		readonly uint _totalChannels;
		int _currentBusyChannels;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Station"/> class.
		/// </summary>
		/// <param name="totalChannels">The total channels held by this station.</param>
		/// <param name="reservedChannels">The number och channels in this station that should be held as reserved channels.</param>
		/// <param name="start">The start position along the highway of this station's range.</param>
		/// <param name="length">The length along the highway of this stations range.</param>
		public Station( uint totalChannels, uint reservedChannels, uint start, uint length )
		{
			if( reservedChannels > totalChannels )
				throw new ArgumentException( Messages.TooManyReservedChannels, "reservedChannels" );

			_totalChannels = totalChannels;
			_reservedChannels = reservedChannels;
			_start = start;
			_length = length;
		}

		internal uint StartPosition
		{
			get { return _start; }
		}

		internal uint EndPosition
		{
			get { return _start + _length; }
		}

		internal int CurrentBusyChannels
		{
			get { return _currentBusyChannels; }
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <paramref name="obj"/> parameter is null.
		/// </exception>
		public override bool Equals( object obj )
		{
			if( ReferenceEquals( null, obj ) )
				return false;
			if( ReferenceEquals( this, obj ) )
				return true;
			if( obj.GetType() != typeof( Station ) )
				return false;
			return Equals( (Station) obj );
		}

		/// <summary>
		/// Tries to claim a channel.
		/// </summary>
		/// <param name="handover">if set to <c>true</c>, allow claiming reserved channels</param>
		/// <return>true, if channel was claimed successfully, false otherwise</return>
		public bool ClaimChannel( bool handover )
		{
			return (
				( handover ) ? _totalChannels - _currentBusyChannels : _totalChannels - ( _reservedChannels + _currentBusyChannels )
				// only count reserved channels if handover is true
				) > 0
				? ClaimChannel()
				: false;
		}

		/// <summary>
		/// Atomic operation that claims a channel and returns true.
		/// </summary>
		/// <returns>true</returns>
		bool ClaimChannel()
		{
			_currentBusyChannels++;
			return true;
		}

		/// <summary>
		/// Releases a claimed channel.
		/// </summary>
		public void ReleaseChannel()
		{
			if( _currentBusyChannels <= 0 )
				throw new InvalidOperationException( Messages.ReleasedChannelNotHeld );
			_currentBusyChannels--;
		}

		/// <summary>
		/// Returns true if given position along the highway is in the range of this Station, false otherwise
		/// </summary>
		/// <param name="position">The position along the highway.</param>
		public bool PositionIsInRange( uint position )
		{
			return position >= _start && position < _start + _length;
		}

		/// <summary>
		/// Equalses the specified other.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Equals( Station other )
		{
			if( ReferenceEquals( null, other ) )
				return false;
			if( ReferenceEquals( this, other ) )
				return true;
			return other._length == _length && other._reservedChannels == _reservedChannels && other._start == _start && other._totalChannels == _totalChannels
				&& other._currentBusyChannels == _currentBusyChannels;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				int result = _length.GetHashCode();
				result = ( result * 397 ) ^ (int) _reservedChannels;
				result = ( result * 397 ) ^ _start.GetHashCode();
				result = ( result * 397 ) ^ (int) _totalChannels;
				result = ( result * 397 ) ^ _currentBusyChannels;
				return result;
			}
		}
	}
}