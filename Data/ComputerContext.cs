using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDZ.ActiveDirectory.Manager
{
    public class ComputerContext : IDisposable
    {
        private readonly PrincipalContext Context;

        private readonly ComputerPrincipal Principal;

        public ComputerContext(string domain, string login, string password)
        {
            if (domain == null)
            {
                throw new Exception("");
            }

            Context = new PrincipalContext(ContextType.Domain, domain, login, password);

            Principal = new ComputerPrincipal(Context);
        }

        public void SetDescription(string machineName, string text)
        {
            Principal.Name = machineName;

            using PrincipalSearcher search = new PrincipalSearcher(Principal);

            if (search == null)
                return;

            ComputerPrincipal result = (ComputerPrincipal)search.FindOne();

            var obj = result.GetUnderlyingObject();

            if (obj == null)
                return;

            DirectoryEntry directoryEntry = (DirectoryEntry)obj;

            directoryEntry.Properties["description"].Value = text;

            directoryEntry.CommitChanges();
        }

        public void Dispose()
        {
            Principal.Dispose();
            Context.Dispose();
        }
    }
}
