using System;
using HighwaySimulation;

namespace SimHighway
{
	/// <summary>
	/// Class that represents a simulation run.
	/// </summary>
	public class Replication : IReplication
	{
		#region Private fields
		readonly IEventQueue _queue;
		readonly uint _replicationLength;
		readonly uint _startup;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Replication"/> class.
		/// </summary>
		/// <param name="replicationLength">Length of the replication.</param>
		/// <param name="dataGatherer">The data gatherer.</param>
		/// <param name="queueFactory">The queue factory.</param>
		/// <param name="startup">The startup time.</param>
		public Replication(
			uint replicationLength, IDataGatherer dataGatherer, IEventQueueFactory queueFactory, uint startup )
		{
			_replicationLength = replicationLength;
			_startup = startup;
			CurrentState = ReplicationState.NotStarted;
			ReplicationData = dataGatherer;
			_queue = queueFactory.Create( dataGatherer );
		}

		/// <summary>
		/// Gets or sets the current state of the simulation.
		/// </summary>
		public ReplicationState CurrentState { get; private set; }

		/// <summary>
		/// Gets or sets the replication data.
		/// </summary>
		/// <value>The replication data.</value>
		public IDataGatherer ReplicationData { get; private set; }

		/// <summary>
		/// Perform the simulation.
		/// </summary>
		public void Run()
		{
			try
			{
				CurrentState = ReplicationState.WarmUp;

				// poke the queue to add first call event.
				_queue.Poke();

				uint currentTime = 0;

				// wait until warm up is done
				while( currentTime < _startup )
					currentTime = _queue.PerformNextEvent();

				// set data gatherer to start collecting data
				CurrentState = ReplicationState.Running;
				ReplicationData.Record();

				// perform simulation
				do
				{
					currentTime = _queue.PerformNextEvent();
				} while( currentTime < _replicationLength );

				CurrentState = ReplicationState.Finished;
			}
			catch( Exception e )
			{
				CurrentState = ReplicationState.Crashed;
				Console.WriteLine( e );
			}
		}
	}
}