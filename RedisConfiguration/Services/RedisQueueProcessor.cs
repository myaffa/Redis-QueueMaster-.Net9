using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;

namespace RedisConfiguration.Services {
	/// <summary>
	/// Implements the <see cref="IRedisQueueProcessor"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisQueueProcessor : IRedisQueueProcessor {
		/// <inheritdoc/>
		public Task ProcessMessageAsync(EnRedisQueueName queueName, string message) {
			Console.WriteLine($"🔄 Processing message from queue [{queueName}]: {message}");
			return Task.CompletedTask;
		}
	}
}
