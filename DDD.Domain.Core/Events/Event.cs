﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Core.Events
{
    public abstract class Event : Message<bool>, INotification
    {
        public DateTime Timestamp
        {
            get;
            private set;
        }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
