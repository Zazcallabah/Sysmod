using HighwaySimulation;

namespace RandomContainer
{
	/// <summary>
	/// Interface for the randomfactory
	/// </summary>
	public interface IRandomFactory
	{
		/// <summary>
		/// Creates a randomcallgenerator.
		/// </summary>
		/// <returns></returns>
		IRandomCallGenerator Create();
	}
}