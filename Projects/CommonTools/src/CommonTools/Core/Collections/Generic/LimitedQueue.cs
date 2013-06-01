using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Core.Collections.Generic
{
    /// <summary>
    /// This class contains all limited queue related data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedQueue<T> : Queue<T>
    {
        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>The limit.</value>
        public int Limit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public LimitedQueue(int limit)
            : base(limit)
        {
            this.Limit = limit;
        }
        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Generic.Queue`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.Queue`1"/>. The value can be null for reference types.</param>
        public new void Enqueue(T item)
        {
            if (this.Count == Limit)
                base.Dequeue();

            base.Enqueue(item);
        }
    }
}
