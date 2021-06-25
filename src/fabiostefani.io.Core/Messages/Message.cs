using System;

namespace fabiostefani.io.Core.Messages
{
    public abstract class Message
    {
        public string MessageTyoe { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            MessageTyoe = GetType().Name;
        }
    }
}