using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventMicroservice.API.Filters
{
    //public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    //{
    //    public string Realm { get; set; }
    //    public bool AllowMultiple => false;

    //    public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
    //    {
    //        Challenge(context);
    //        return Task.FromResult(0);
    //    }

    //    private void Challenge(HttpAuthenticationChallengeContext context)
    //    {
    //        string parameter = null;

    //        if (!string.IsNullOrEmpty(Realm))
    //            parameter = "realm=\"" + Realm + "\"";

    //        //context.ChallengeWith("Bearer", parameter);
    //    }
    //}
}
