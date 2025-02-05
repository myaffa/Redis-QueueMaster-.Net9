using Microsoft.AspNetCore.Mvc;
using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;

namespace RedisManagement.Controllers {
	/// <summary>
	/// Controller for managing Redis queue operations and distributed locks.
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class HomeController(IRedisQueueFactory redisQueueFactory, IRedisLockManager redisLockManager) : Controller {
		private readonly IRedisQueueFactory _redisQueueFactory = redisQueueFactory;
		private readonly IRedisLockManager _redisLockManager = redisLockManager;

		/// <summary>
		/// Sends a message to a specified Redis queue.
		/// </summary>
		/// <param name="queue">The type of Redis queue.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="text">The message content.</param>
		/// <returns>HTTP response indicating success or failure.</returns>
		[HttpPost("send-message/{queue}/{queueName}")]
		public async Task<IActionResult> SendMessage([FromRoute] EnRedisQueueType queue, [FromRoute] EnRedisQueueName queueName, [FromBody] string text) {
			if (string.IsNullOrWhiteSpace(text)) {
				return BadRequest("Message content cannot be empty.");
			}

			try {
				var redisQueue = _redisQueueFactory.GetQueue(queue);
				await redisQueue.SendMessageAsync(queueName, text);
				return Ok($"✅ Message sent successfully to queue: {queue}");
			} catch (Exception ex) {
				return StatusCode(500, $"❌ Failed to send message: {ex.Message}");
			}
		}

		/// <summary>
		/// Acquires a distributed lock in Redis.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="lockID">The unique lock identifier.</param>
		/// <returns>HTTP response indicating success or failure.</returns>
		[HttpPost("acquire-lock/{category}/{lockID}")]
		public async Task<IActionResult> AcquireLockAsync([FromRoute] EnRedisLockCategory category, [FromRoute] string lockID) {
			var res = await _redisLockManager.AcquireLockAsync(category, lockID, TimeSpan.FromMinutes(5));
			if (!res)
				return BadRequest($"❌ Failed to acquire lock for category: {category}");
			return Ok($"✅ Lock acquired successfully for category: {category}");
		}

		/// <summary>
		/// Releases a distributed lock in Redis.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="lockID">The unique lock identifier.</param>
		/// <returns>HTTP response indicating success or failure.</returns>
		[HttpPost("release-lock/{category}/{lockID}")]
		public async Task<IActionResult> ReleaseLockAsync([FromRoute] EnRedisLockCategory category, [FromRoute] string lockID) {
			var res = await _redisLockManager.ReleaseLockAsync(category, lockID);
			if (!res)
				return BadRequest($"❌ Failed to release lock for category: {category}");
			return Ok($"✅ Lock released successfully for category: {category}");
		}

		/// <summary>
		/// Extends an existing distributed lock in Redis.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="lockID">The unique lock identifier.</param>
		/// <returns>HTTP response indicating success or failure.</returns>
		[HttpPost("extend-lock/{category}/{lockID}")]
		public async Task<IActionResult> ExtendLockAsync([FromRoute] EnRedisLockCategory category, [FromRoute] string lockID) {
			var res = await _redisLockManager.ExtendLockAsync(category, lockID, TimeSpan.FromMinutes(5));
			if (!res)
				return BadRequest($"❌ Failed to extend lock for category: {category}");
			return Ok($"✅ Lock extended successfully for category: {category}");
		}

		/// <summary>
		/// Checks if a distributed lock is currently active in Redis.
		/// </summary>
		/// <param name="category">The lock category.</param>
		/// <param name="lockID">The unique lock identifier.</param>
		/// <returns>HTTP response with lock status.</returns>
		[HttpGet("is-locked/{category}/{lockID}")]
		public async Task<IActionResult> IsLockedAsync([FromRoute] EnRedisLockCategory category, [FromRoute] string lockID) {
			bool isLocked = await _redisLockManager.IsLockedAsync(category, lockID);
			return Ok(new { category, lockID, isLocked });
		}
	}
}
