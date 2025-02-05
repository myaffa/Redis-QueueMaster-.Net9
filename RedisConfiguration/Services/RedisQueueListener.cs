using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using RedisConfiguration.Models;
using StackExchange.Redis;

namespace RedisConfiguration.Services {
	/// <summary>
	/// Background service that listens to Redis queues and processes incoming messages.
	/// </summary>
	public class RedisQueueListener(IConnectionMultiplexer redis, IOptions<RedisSettings> settings, IServiceProvider serviceProvider) : BackgroundService {
		private readonly IConnectionMultiplexer _redis = redis ?? throw new ArgumentNullException(nameof(redis));
		private readonly RedisSettings _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
		private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

		/// <inheritdoc/>
		/// <summary>
		/// Continuously monitors Redis queues for new messages and processes them asynchronously.
		/// </summary>
		/// <param name="stoppingToken">A cancellation token to stop the service.</param>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			while (!stoppingToken.IsCancellationRequested) {
				var tasks = Enum.GetValues<EnRedisQueueName>()
					.Cast<EnRedisQueueName>()
					.Select(queueName => Task.Run(() => ProcessQueueAsync(queueName), stoppingToken));

				await Task.WhenAll(tasks);
				await Task.Delay(500, stoppingToken); // Adds a short delay to avoid excessive CPU usage.
			}
		}

		/// <summary>
		/// Processes messages from a specific Redis queue.
		/// </summary>
		/// <param name="queueName">The queue name to process messages from.</param>
		private async Task ProcessQueueAsync(EnRedisQueueName queueName) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";

			var message = await db.ListLeftPopAsync(fullQueueName);
			if (!message.IsNullOrEmpty) {
				using var scope = _serviceProvider.CreateScope();
				var processor = scope.ServiceProvider.GetRequiredService<IRedisQueueProcessor>();
				await processor.ProcessMessageAsync(queueName, message.ToString());
			}
		}

	}
}
