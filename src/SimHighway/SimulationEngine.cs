using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using HighwaySimulation;
using RandomContainer;

namespace SimHighway
{
	/// <summary>
	/// The Engine that actually runs the simulation and returns the result.
	/// </summary>
	internal class SimulationEngine
	{
		#region Private fields
		readonly uint _channels;
		readonly uint _highwayLength;
		readonly IRandomFactory _randomFactory;
		readonly IReplicationFactory _replicationFactory;
		readonly uint _threadCount;
		readonly uint _replications;
		readonly Action<object> _report;
		readonly uint _reserved;
		readonly uint _stationCount;
		readonly IDictionary<uint, Action<uint>> _bagOfTasks;
		DataGatherer _totalCollectedData;
		Semaphore _workersDone;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="SimulationEngine"/> class.
		/// </summary>
		/// <param name="stationCount">The station count.</param>
		/// <param name="highwayLength">Length of the highway.</param>
		/// <param name="replications">The replications.</param>
		/// <param name="channels">The channels.</param>
		/// <param name="reserved">The reserved.</param>
		/// <param name="report">The report.</param>
		/// <param name="randomFactory">The random factory.</param>
		/// <param name="replicationFactory">The replication factory.</param>
		/// <param name="threadCount">The thread count. Create this many parallel threads to run concurrent replications in.</param>
		public SimulationEngine(
			uint stationCount,
			uint highwayLength,
			uint replications,
			uint channels,
			uint reserved,
			Action<object> report,
			IRandomFactory randomFactory,
			IReplicationFactory replicationFactory,
			uint threadCount
			)
		{
			if( threadCount == 0 )
				throw new ArgumentException( Messages.CantRunWithNoWorkerThreads, "threadCount" );

			_randomFactory = randomFactory;
			_replicationFactory = replicationFactory;
			_threadCount = threadCount;
			_stationCount = stationCount;
			_highwayLength = highwayLength;
			_replications = replications;
			_channels = channels;
			_reserved = reserved;
			_report = report;
			_bagOfTasks = new Dictionary<uint, Action<uint>>( (int) replications );
			_totalCollectedData = new DataGatherer( 0 );
		}

		public void Start()
		{
			// Create bag of tasks
			for( uint i = 1; i <= _replications; i++ )
				_bagOfTasks.Add( i, CreateAndRunReplication );

			_workersDone = new Semaphore( 0, (int) _threadCount );

			//spawn and start worker threads.
			IList<Thread> workers = new List<Thread>();
			for( int i = 0; i < _threadCount; i++ )
			{
				var t = new Thread( WorkerThread );
				t.Start();
				workers.Add( t );
			}

			// wait for worker threads to end and report
			foreach( Thread w in workers )
				_workersDone.WaitOne();
			_report( _totalCollectedData );
		}

		void WorkerThread()
		{
			while( true )
			{
				KeyValuePair<uint, Action<uint>> currentTask;

				lock( _bagOfTasks ) // ensure atomic operation
				{
					if( _bagOfTasks.Count == 0 )
						break;
					currentTask = _bagOfTasks.First();
					_bagOfTasks.Remove( currentTask );
				}

				currentTask.Value( currentTask.Key );
			}

			// Signal to main thread that this worker has finished.
			_workersDone.Release();

		}

		void CreateAndRunReplication( uint replicationId )
		{
			// setup simulation run
			IReplication r = _replicationFactory.Create(
				new DataGatherer( replicationId ),
				new EventQueueFactory(
					_randomFactory.Create(),
					_stationCount,
					_highwayLength,
					_channels,
					_reserved )
				);

			// run simulation and measure how long it takes
			long before = DateTime.Now.Ticks;
			r.Run();
			long after = DateTime.Now.Ticks;

			// Collect and print required data
			_totalCollectedData += r.ReplicationData;
			_report( r.ReplicationData );
			Debug.WriteLine(
				string.Format( "took {0}", new TimeSpan( after - before ) ) );
		}
	}
}