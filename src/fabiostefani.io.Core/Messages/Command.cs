using System;
using FluentValidation.Results;
using MediatR;

namespace fabiostefani.io.Core.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool EhValido();        
    }
}