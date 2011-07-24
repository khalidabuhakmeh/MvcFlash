namespace MvcFlash.Core.Providers
{
    public interface IFlashMessagePusher
    {
        /// <summary>
        /// Pushes the specified message into the message stack.
        /// </summary>
        /// <param name="message">The message.</param>
        void Push(object message);

		/// <summary>
		/// Pushes the message to the message stack with a unique key. If the key already exists, it will overwrite it.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="message">The message.</param>
    	void Push(string key, object message);
    }
}