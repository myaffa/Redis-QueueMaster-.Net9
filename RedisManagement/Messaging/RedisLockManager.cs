using Microsoft.Extensions.Options;
using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using RedisConfiguration.Models;
using StackExchange.Redis;

namespace RedisManagement.Messaging {
	/// <summary>
	/// Implements the <see cref="IRedisLockManager"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisLockManager(IConnectionMultiplexer redis, IOptions<RedisSettings> settings) : IRedisLockManager {
		private readonly IConnectionMultiplexer _redis = redis;
		private readonly RedisSettings _settings = settings.Value;

		/// <summary>
		/// Generates a unique Redis key for a lock based on category and key.
		/// </summary>
		private string GetLockKey(EnRedisLockCategory category, string key) => $"{_settings.QueuePrefix}lock:{category}:{key}";

		/// <inheritdoc/>
		public async Task<bool> AcquireLockAsync(EnRedisLockCategory category, string key, TimeSpan expiration) {
			var db = _redis.GetDatabase();
			string lockKey = GetLockKey(category, key);

			// Attempt to set the lock if it does not already exist
			return await db.StringSetAsync(lockKey, "LOCKED", expiration, When.NotExists);
		}

		/// <inheritdoc/>
		public async Task<bool> ReleaseLockAsync(EnRedisLockCategory category, string key) {
			var db = _redis.GetDatabase();
			string lockKey = GetLockKey(category, key);

			// Remove the lock from Redis
			return await db.KeyDeleteAsync(lockKey);
		}

		/// <inheritdoc/>
		public async Task<bool> ExtendLockAsync(EnRedisLockCategory category, string key, TimeSpan additionalTime) {
			var db = _redis.GetDatabase();
			string lockKey = GetLockKey(category, key);

			// Retrieve the remaining TTL of the existing lock
			var currentTTL = await db.KeyTimeToLiveAsync(lockKey);
			if (currentTTL.HasValue) {
				// Extend the lock's expiration time
				return await db.KeyExpireAsync(lockKey, currentTTL.Value + additionalTime);
			}

			return false;
		}

		/// <inheritdoc/>
		public async Task<bool> IsLockedAsync(EnRedisLockCategory category, string key) {
			var db = _redis.GetDatabase();
			string lockKey = GetLockKey(category, key);

			// Check if the lock exists in Redis
			return await db.KeyExistsAsync(lockKey);
		}
	}
}
