using System;
using System.Configuration;
using System.Globalization;
using RandomContainer;

namespace SimHighway
{
	internal class Program
	{
		static void Main( string[] args )
		{
			try
			{
				//
				// Get data from settings file.
				//
				uint stationCount = UInt32.Parse( ConfigurationManager.AppSettings["baseStationCount"], CultureInfo.InvariantCulture );

				// distance is read as meter, but stored as mm. With unsigned 32 bit integer datatype, this can handle distances up to some 4300 km.
				uint highwayLength = UInt32.Parse( ConfigurationManager.AppSettings["highwayLength"], CultureInfo.InvariantCulture ) * 1000;
				double callPosStart = Double.Parse( ConfigurationManager.AppSettings["callPositionStart"], CultureInfo.InvariantCulture ) * 1000;
				double callPosEnd = Double.Parse( ConfigurationManager.AppSettings["callPositionEnd"], CultureInfo.InvariantCulture ) * 1000;
				double callPosPeak = Double.Parse( ConfigurationManager.AppSettings["callPositionPeak"], CultureInfo.InvariantCulture ) * 1000;

				// speed is read as meter/second (which is equivalent to mm/ms)
				double speedMean = Double.Parse( ConfigurationManager.AppSettings["speedMean"], CultureInfo.InvariantCulture );
				double speedDeviation = Double.Parse( ConfigurationManager.AppSettings["speedDeviation"], CultureInfo.InvariantCulture );

				// time is read as seconds but stored as milliseconds. This gives a maximum measurment value of just under 50 days. I consider increasing this.
				double interArrivalMean = Double.Parse( ConfigurationManager.AppSettings["interArrivalMean"], CultureInfo.InvariantCulture ) * 1000;
				double durationMean = Double.Parse( ConfigurationManager.AppSettings["durationMean"], CultureInfo.InvariantCulture ) * 1000;

				//
				// Get data from command-line
				//
				int seed = Int32.Parse( args[0], CultureInfo.InvariantCulture );
				uint length = UInt32.Parse( args[1], CultureInfo.InvariantCulture ) * 1000;
				uint replication = UInt32.Parse( args[2], CultureInfo.InvariantCulture );
				uint warmup = UInt32.Parse( args[3], CultureInfo.InvariantCulture ) * 1000;
				uint channels = UInt32.Parse( args[4], CultureInfo.InvariantCulture );
				uint reserved = UInt32.Parse( args[5], CultureInfo.InvariantCulture );
				uint threadcount = 1;
				if( args.Length > 6 )
					threadcount = UInt32.Parse( args[6], CultureInfo.InvariantCulture );

				Action<object> reporter = o => Console.WriteLine( o );

				new SimulationEngine(
					stationCount,
					highwayLength,
					replication,
					channels,
					reserved,
					reporter,
					new RandomFactory(
						seed,
						callPosStart,
						callPosEnd,
						callPosPeak,
						speedMean,
						speedDeviation,
						interArrivalMean,
						durationMean ),
					new ReplicationFactory( length, warmup ),
					threadcount
					).Start();
			}
			catch( ArgumentException ae )
			{
				Console.WriteLine( Messages.ErrorParsingInput, ae.ParamName );
			}
			catch( FormatException fe )
			{
				Console.WriteLine( Messages.ErrorParsingInput, fe.Message );
			}
			catch( OverflowException oe )
			{
				Console.WriteLine( Messages.ErrorParsingInput, oe.Message );
			}
			catch( IndexOutOfRangeException )
			{
				Console.WriteLine( Messages.ErrorParsingInput, "bad parameter count" );
			}
		}
	}
}