using System.Threading.Tasks;

namespace Managers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Rimaethon.Scripts.Utility;
	using UnityEngine;
	using System.Threading;

	namespace _Scripts.Managers
	{
		public class TimeManager:PersistentSingleton<TimeManager>
		{
			private List<ITimeDependent> timeDependentObjects = new List<ITimeDependent>();
			private int numberOfTimeDependentObjects;
			private long currentTime;
			private CancellationTokenSource cancellationTokenSource;

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
						await Task.Delay(1000, cancellationToken: cancellationToken);
					}
				}
				catch (TaskCanceledException)
				{
				}
			}
		}
	}

}
