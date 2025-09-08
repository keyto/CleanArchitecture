using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
 

namespace CleanArchitecture.Infraestructure.Authentication
{
    public class PermissionAuthorizationPolicyProvider :DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options) : base(options)
        {
            
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // obtener la politica por defecto
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

            if (policy is not null)
            {
                return policy;
            } 
            else                
            {
                // si la politica es nula, crear una nueva politica            
                //var policyBuilder = new AuthorizationPolicyBuilder();
                //policyBuilder.AddRequirements(new PermissionRequirement(policyName));
                //policy = policyBuilder.Build();

                return new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();
            }
           
        }
    }
}
