using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Utility;

namespace Managers
{
	public class TimeManager : PersistentSingleton<TimeManager>
	{
		private CancellationTokenSource cancellationTokenSource;
		private long currentTime;
		private int numberOfTimeDependentObjects;
		private List<ITimeDependent> timeDependentObjects = new List<ITimeDependent>();

		private void Start()
		{
			Application.targetFrameRate = 60;
			timeDependentObjects = FindObjectsOfType<MonoBehaviour>().OfType<ITimeDependent>().ToList();
			currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			numberOfTimeDependentObjects = timeDependentObjects.Count;
			cancellationTokenSource = new CancellationTokenSource();
			TikTak(cancellationTokenSource.Token);
		}

		private void OnDisable()
		{
			cancellationTokenSource?.Cancel();
		}

		private async void TikTak(CancellationToken cancellationToken)
		{
			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					timeDependentObjects = FindObjectsOfType<MonoBehaviour>().OfType<ITimeDependent>().ToList();
					numberOfTimeDependentObjects = timeDependentObjects.Count;

					currentTime++;

					for (int i = 0; i < numberOfTimeDependentObjects; i++)
					{
						timeDependentObjects[i].OnTimeUpdate(currentTime);
					}

					await Task.Delay(1000, cancellationToken);
				}
			}
			catch (TaskCanceledException)
			{
			}
		}
	}
}
