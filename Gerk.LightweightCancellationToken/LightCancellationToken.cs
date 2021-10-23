using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gerk.LightweightCancellationToken
{
	internal class CancellationCallbackDisposer : IDisposable
	{
		private bool isDisposed;
		public bool IsDisposed => Volatile.Read(ref isDisposed);

		public CancellationCallbackDisposer(bool disposed)
		{
			isDisposed = disposed;
		}

		public void Dispose()
		{
			Volatile.Write(ref isDisposed, true);
		}
	}

	public class LightweightCancellationToken
	{
		internal bool cancelled;
		private readonly object onCancelAppendLock = new object();
		private List<Action> onCancel;

		internal CancellationCallbackDisposer OnCancellationEvent(Action callback)
		{
			var isCanclled = Volatile.Read(ref cancelled);
			var disposer = new CancellationCallbackDisposer(isCanclled);
			if (isCanclled)
				callback();
			else
			{
				void wrapperCallback()
				{
					if (!disposer.IsDisposed)
						callback();
				}

				lock (onCancelAppendLock)
				{
					onCancel.Add(wrapperCallback);
				}
			}
			return disposer;
		}

		internal void Cancel()
		{
			Volatile.Write(ref cancelled, true);
			foreach (var callback in onCancel)
			{
				callback();
			}
		}

		public LightweightCancellationToken()
		{
			cancelled = false;
		}

		public LightweightCancellationToken(bool cancelled)
		{
			this.cancelled = cancelled;
		}

		public LightweightCancellationToken(TimeSpan timeSpan)
		{
			Task.Delay(timeSpan).Then
		}
	}
}
