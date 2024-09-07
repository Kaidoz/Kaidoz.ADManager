using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDZ.ActiveDirectory.Manager
{
    internal class PrinterContext : IDisposable
    {
        private PrincipalContext? context;

        public PrinterContext(string domain, string login, string password)
        {
            context = new PrincipalContext(ContextType.Domain, domain, login, password);
        }

        public void SetDescription()
        {
            /*
                using (UserPrincipal user = new UserPrincipal(context))
                {
                    using (PrincipalSearcher searcher = new PrincipalSearcher(user))
                    {
                        foreach (var result in searcher.FindAll())
                        {
                            if (result is PrintQueue printQueue)
                            {
                                Console.WriteLine(printer.Name);
                            }
                        }
                    }
                }
            */
        }

        public void Dispose()
        {
            context?.Dispose();
            context = null;
        }
    }
}
