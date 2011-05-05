using System.Collections.Generic;

namespace MvcFlash.Core.Providers
{
    public interface IFlashMessagePopper
    {
        /// <summary>
        /// Pops all the messages and returns them.
        /// </summary>
        /// <returns></returns>
        ICollection<dynamic> Pop();

        /// <summary>
        /// Clears all the messages.
        /// </summary>
        void Clear();
    }
}