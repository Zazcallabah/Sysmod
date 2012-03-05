namespace HighwaySimulation
{
	/// <summary>
	/// interface for generating random calls
	/// </summary>
	public interface IRandomCallGenerator
	{
		/// <summary>
		/// Generates a random call.
		/// </summary>
		/// <param name="lastCallStartTime">The start time of the last call.</param>
		/// <returns>
		/// A calldata object filled with random data
		/// </returns>
		CallData GenerateRandomCall( uint lastCallStartTime );
	}
}