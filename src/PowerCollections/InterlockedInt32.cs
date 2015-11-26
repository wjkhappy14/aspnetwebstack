
using System;
using System.Threading;

namespace Wintellect.PowerCollections
{
    /// <summary>
    /// Thread-safe helper to manage a value.
    /// </summary>
    public class InterlockedInt32
    {
        // fields
        private int _value;

        // constructors
        public InterlockedInt32(int initialValue)
        {
            _value = initialValue;
        }

        // properties
        public int Value
        {
            get { return Interlocked.CompareExchange(ref _value, 0, 0); }
        }

        // methods
        public bool TryChange(int toValue)
        {
            return Interlocked.Exchange(ref _value, toValue) != toValue;
        }

        public bool TryChange(int fromValue, int toValue)
        {
            if (fromValue == toValue)
            {
                throw new ArgumentException("fromValue and toValue must be different.");
            }
            return Interlocked.CompareExchange(ref _value, toValue, fromValue) == fromValue;
        }
    }
}