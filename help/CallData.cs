using System;

namespace HighwaySimulation
{
	/// <summary>
	/// Class that holds data for a call and some methods for accessing that data.
	/// </summary>
	public class CallData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CallData"/> class.
		/// </summary>
		/// <param name="speed">The speed.</param>
		/// <param name="startposition">The startposition.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="starttime">The starttime.</param>
		public CallData(
			uint speed, uint startposition, uint duration, uint starttime )
		{
			if( speed > 400000 )
				throw new ArgumentOutOfRangeException( "speed", Messages.ArgumentNegative );
			Speed = speed;
			StartPosition = startposition;
			Duration = duration;
			StartTime = starttime;
		}

		/// <summary>
		/// Gets or sets the speed the call travels at.
		/// </summary>
		/// <value>The speed.</value>
		public uint Speed { get; private set; }

		/// <summary>
		/// Gets the position along the highway where the call starts.
		/// </summary>
		public uint StartPosition { get; private set; }

		/// <summary>
		/// Gets or sets the expected duration of the call, unless it will be blocked or dropped.
		/// </summary>
		public uint Duration { get; private set; }

		/// <summary>
		/// Gets the start time of the call
		/// </summary>
		public uint StartTime { get; private set; }

		/// <summary>
		/// Gets the time at which the call would normally end, unless it will be blocked or dropped.
		/// </summary>
		public uint EndTime
		{
			get { return StartTime + Duration; }
		}

		/// <summary>
		/// Gets the position for where the call should end, unless it will be blocked or dropped.
		/// </summary>
		public uint EndPosition
		{
			get { return StartPosition + ( Speed * Duration ); }
		}

		/// <summary>
		/// Gets the position the call will be in at a given time.
		/// </summary>
		/// <param name="absoluteTime">The time. This is not the relative time compared to the start of the call.</param>
		/// <returns>The position</returns>
		public uint GetPositionForAbsoluteTime( uint absoluteTime )
		{
			if( absoluteTime < StartTime || absoluteTime > EndTime )
				throw new ArgumentOutOfRangeException( "absoluteTime", Messages.CallPositionForInvalidTime );

			return StartPosition + ( Speed * ( absoluteTime - StartTime ) );
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
			if( obj.GetType() != typeof( CallData ) )
				return false;
			return Equals( (CallData) obj );
		}

		/// <summary>
		/// Gets the absolute time for position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns></returns>
		public uint GetAbsoluteTimeForPosition( uint position )
		{
			if( position < StartPosition || position > EndPosition )
				throw new ArgumentOutOfRangeException( "position", Messages.CallTimeForInvalidPosition );

			return StartTime + ( ( position - StartPosition ) / Speed );
		}

		public bool Equals( CallData other )
		{
			if( ReferenceEquals( null, other ) )
				return false;
			if( ReferenceEquals( this, other ) )
				return true;
			return other.Speed == Speed && other.StartPosition == StartPosition && other.Duration == Duration && other.StartTime == StartTime;
		}

		/// <summary>
		/// Signals that the Call will wrap around the Highway.
		/// </summary>
		/// <param name="handoverTime">The time at which the handover will happen.</param>
		/// <returns>
		/// A cloned calldata object with parameters modified to look like the call starts at position 0 at the time of the handover, and has duration shortened by the time since the call began.
		/// </returns>
		public CallData Wrap( uint handoverTime )
		{
			uint newduration = Duration - ( handoverTime - StartTime );

			return new CallData( Speed, 0, newduration, handoverTime );
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
				int result = Speed.GetHashCode();
				result = ( result * 397 ) ^ StartPosition.GetHashCode();
				result = ( result * 397 ) ^ Duration.GetHashCode();
				result = ( result * 397 ) ^ StartTime.GetHashCode();
				return result;
			}
		}
	}
}