﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Seedwork
{
    public interface IRepository<T> where T: IAggregateRoot
    {
    }
}
