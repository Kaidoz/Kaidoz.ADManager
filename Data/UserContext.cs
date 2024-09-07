using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KDZ.ActiveDirectory.Models;

namespace KDZ.ActiveDirectory.Manager
{
    public class UserContext : IDisposable
    {
        private readonly PrincipalContext Context;

        private readonly UserPrincipal Principal;

        public UserContext(string domain, string login, string password)
        {
            if (domain == null)
            {
                throw new Exception("");
            }

            Context = new PrincipalContext(ContextType.Domain, domain, login, password);

            Principal = new UserPrincipal(Context);
        }

        public void Dispose()
        {
            if (Principal != null)
                Principal.Dispose();

            if (Context != null)
                Context.Dispose();
        }

        public UserPrincipal GetUser(string accountName)
        {
            try
            {

                var user = UserPrincipal.FindByIdentity(Context, accountName);

                return user;
            }
            catch
            {
                return null;
            }
        }

        public List<DomainUser> GetUsers(string groupName)
        {
            var list = new List<DomainUser>();

            GroupPrincipal group = GroupPrincipal.FindByIdentity(Context, groupName);

            if (group != null)
            {
                list.AddRange(group.GetMembers()
                    .Where(userPrincipal => userPrincipal != null)
                    .Select(userPrincipal => new DomainUser((UserPrincipal)userPrincipal)));
            }

            return list;
        }

        public void SetDescription(string accountName, string text)
        {
            try
            {
                Principal.SamAccountName = accountName;

                using PrincipalSearcher search = new PrincipalSearcher(Principal);

                if (search == null)
                    return;

                var result = search.FindOne();

                if (result == null)
                {
                    return;
                }

                UserPrincipal userResult = (UserPrincipal)result;

                var obj = userResult.GetUnderlyingObject();

                if (obj == null)
                    return;

                DirectoryEntry directoryEntry = (DirectoryEntry)obj;

                directoryEntry.Properties["description"].Value = text;

                directoryEntry.CommitChanges();
            }
            catch
            {

            }
        }
    }
}
