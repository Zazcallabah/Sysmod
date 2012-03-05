namespace HighwaySimulation
{
	/// <summary>
	/// interface for simulation data gatherer
	/// </summary>
	public interface IDataGatherer
	{
		/// <summary>
		/// Signals to the data gatherer that data should start being recorded.
		/// </summary>
		void Record();

		/// <summary>
		/// Signals that a call started.
		/// </summary>
		void SignalCallStarted();

		/// <summary>
		/// Signals that a call was blocked.
		/// </summary>
		void SignalCallBlocked();

		/// <summary>
		/// Signals that a call was dropped.
		/// </summary>
		void SignalCallDropped();

		/// <summary>
		/// Signals that a call was ended.
		/// </summary>
		void SignalCallHangup();
	}
}