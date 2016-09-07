using System;
using System.Linq;
using log4net;
using log4net.Config;

namespace u2fhost.console
{
	public class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        // https://shop.nitrokey.com/shop/product/nitrokey-u2f-5

		private const int VendorId = 0x2581;
		private const int ProductId = 0xF1D0;

		public static void Main(string[] args)
		{
			XmlConfigurator.Configure();

			Sample.Run(VendorId, ProductId).Wait();
		}

		private static void Ping(U2FHidDevice u2F)
		{
			var r = new Random();
			var pingData = new byte[1024];
			r.NextBytes(pingData);

			var pingResponse = u2F.PingAsync(pingData).Result;

			Log.Debug($"Ping response data matches request: {pingData.SequenceEqual(pingResponse)}");
		}

		private static void PrintVersions(ApduDevice apdu)
		{
			Log.Debug("Supported versions:");
			foreach (var version in apdu.GetSupportedVersionsAsync().Result)
			{
				Log.Debug(version);
			}
		}
	}
}
