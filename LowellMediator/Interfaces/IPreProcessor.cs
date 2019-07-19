﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LowellMediator.Interfaces
{
    public interface IPreProcessor<TRequest>
    {
        TRequest Process(TRequest request);
    }
}
