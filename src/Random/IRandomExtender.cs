namespace RandomContainer
{
	/// <summary>
	/// Interface for a Random-class extension.
	/// Defines functions for retreiving standard-, uniformly, expontentially distributed values. 
	/// </summary>
	public interface IRandomExtender
	{
		/// <summary>
		/// Get a uniformly distributed value betveen 0(inclusive) and 1(exclusive)
		/// </summary>
		/// <returns>The random value.</returns>
		double NextUniform();

		/// <summary>
		/// Get a normal distributed value  centered around the parameter mean and with the given deviation
		/// </summary>
		/// <param name="mean">The mean value of the distribution</param>
		/// <param name="deviation">The deviation of the distribution</param>
		/// <returns>The random value.</returns>
		double NextNormal( double mean, double deviation );

		/// <summary>
		/// Get an exponentially distrubuted value with parameter mean as  1/lambda
		/// </summary>
		/// <param name="mean">The mean value of the distribution</param>
		/// <returns>The random value.</returns>
		double NextExponential( double mean );

		/// <summary>
		/// Get a triangularly distributed value with set parameters.
		/// </summary>
		/// <param name="start">The start point of the distribution.</param>
		/// <param name="end">The end point of the destribution.</param>
		/// <param name="peak">The peak point of the distribution.</param>
		/// <returns></returns>
		double NextTriangular( double start, double end, double peak );
	}
}