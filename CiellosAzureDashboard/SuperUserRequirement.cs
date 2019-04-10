using Microsoft.AspNetCore.Authorization;

namespace CiellosAzureDashboard
{
    public class SuperUserRequirement : IAuthorizationRequirement
    {
        public bool isSuperUser { get; private set; }

        public SuperUserRequirement(bool _isSuperUser)
        {
            isSuperUser = _isSuperUser;
        }
    }
}
