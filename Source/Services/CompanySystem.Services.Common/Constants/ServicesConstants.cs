﻿namespace CompanySystem.Services.Common.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ServicesConstants
    {
        // Common
        public const int DbModelInsertionFailed = -1;
        public const int DbModelCreationFailed = -2;

        // Events service
        public const bool EventCancellationSuccessful = true;
        public const bool EventCancellationFailed = false;
        public const int VoteCreationSuccessful = 1;
    }
}
