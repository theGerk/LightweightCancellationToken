using Gerk.AsyncThen;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gerk.LightweightCancellationToken
{
	public class LightweightCancellationToken
	{
		private volatile bool cancelled;
		internal bool Cancelled => cancelled;

		internal void Cancel()
		{
			cancelled = true;
		}

		public LightweightCancellationToken(bool cancelled = false)
		{
			this.cancelled = cancelled;
		}

		public LightweightCancellationToken(TimeSpan timeSpan)
		{
			Task.Delay(timeSpan).Then(Cancel);
		}
	}

	public static class LightweightCancellationTokenExtensions
	{
		public static bool IsCancelled(this LightweightCancellationToken token) => token?.Cancelled ?? false;
		public static void Cancel(this LightweightCancellationToken token) => token?.Cancel();
	}
}
