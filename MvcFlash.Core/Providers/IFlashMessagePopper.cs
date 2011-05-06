using System;
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
        /// Pops all the messages based on the specified where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns></returns>
        ICollection<dynamic> Select(Func<dynamic, bool> where);

        /// <summary>
        /// Clears all the messages.
        /// </summary>
        void Clear();
    }
}