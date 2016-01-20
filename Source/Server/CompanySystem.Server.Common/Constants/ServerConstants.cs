namespace CompanySystem.Server.Common.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ServerConstants
    {
        // Common
        public const string RequestCannotBeEmpty = "Request cannot be empty or have NULL properties.";

        // Votes Controller

        public const string VoteSuccessfulMessage = "Vote successful";
        public const string VoteFailedMessage = "Vote failed";
        public const string NoVotesMessage = "There are no votes currently for this event.";
    }
}