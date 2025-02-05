using RedisConfiguration.Enums;

namespace RedisConfiguration.Interfaces {
	/// <summary>
	/// Interface for managing distributed locks in Redis.
	/// </summary>
	public interface IRedisLockManager {
		/// <summary>
		/// Attempts to acquire a lock for a given key and category.
		/// The lock is set with an expiration time to prevent indefinite blocking.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="key">The unique key identifying the lock.</param>
		/// <param name="expiration">The duration for which the lock is held.</param>
		/// <returns>True if the lock was acquired successfully; otherwise, false.</returns>
		Task<bool> AcquireLockAsync(EnRedisLockCategory category, string key, TimeSpan expiration);

		/// <summary>
		/// Releases an existing lock, allowing others to acquire it.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="key">The unique key identifying the lock.</param>
		/// <returns>True if the lock was successfully released; otherwise, false.</returns>
		Task<bool> ReleaseLockAsync(EnRedisLockCategory category, string key);

		/// <summary>
		/// Extends the expiration time of an existing lock.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="key">The unique key identifying the lock.</param>
		/// <param name="additionalTime">The additional time to extend the lock.</param>
		/// <returns>True if the lock was successfully extended; otherwise, false.</returns>
		Task<bool> ExtendLockAsync(EnRedisLockCategory category, string key, TimeSpan additionalTime);

		/// <summary>
		/// Checks if a lock is currently active.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="key">The unique key identifying the lock.</param>
		/// <returns>True if the lock exists; otherwise, false.</returns>
		Task<bool> IsLockedAsync(EnRedisLockCategory category, string key);
	}
}
