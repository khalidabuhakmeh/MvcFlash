namespace MvcFlash.Core.Providers
{
    public interface IFlashMessagePusher
    {
        /// <summary>
        /// Pushes the specified message into the message stack.
        /// </summary>
        /// <param name="message">The message.</param>
        void Push(object message);
    }
}