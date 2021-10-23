using Gerk.AsyncThen;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gerk.LightweightCancellationToken
{
	public class LightweightCancellationToken
	{
		internal volatile bool cancelled;

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
}
