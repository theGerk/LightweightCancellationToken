using Gerk.AsyncThen;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gerk.LightweightCancellationToken
{
	/// <summary>
	/// Very light-weight cancellation token. Not gaurenteed to be thread safe, but rather eventually consistant. Can be cancelled, and never uncancelled. A default value of <see langword="null"/> is seen as a token that is never cancelled.
	/// </summary>
	public class LightweightCancellationToken
	{
		private volatile bool cancelled;
		internal bool Cancelled => cancelled;

		internal void Cancel()
		{
			cancelled = true;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cancelled">Weather or not the token is cancled already.</param>
		public LightweightCancellationToken(bool cancelled = false)
		{
			this.cancelled = cancelled;
		}

		/// <summary>
		/// Cancels after an ammount of time.
		/// </summary>
		/// <param name="timeSpan">Time until the </param>
		public LightweightCancellationToken(TimeSpan timeSpan)
		{
			cancelled = false;
			Task.Delay(timeSpan).Then(Cancel);
		}
	}

	/// <summary>
	/// Extension methods for <see cref="LightweightCancellationToken"/>.
	/// </summary>
	public static class LightweightCancellationTokenExtensions
	{
		/// <summary>
		/// Is the <paramref name="token"/> cancelled?
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns><see langword="true"/> if it is cancelled, <see langword="false"/> if it has not been cancelled.</returns>
		public static bool IsCancelled(this LightweightCancellationToken token) => token?.Cancelled ?? false;
		/// <summary>
		/// Cancels the token.
		/// </summary>
		/// <param name="token">The token.</param>
        public static void Cancel(this LightweightCancellationToken token) => token?.Cancel();
	}
}
