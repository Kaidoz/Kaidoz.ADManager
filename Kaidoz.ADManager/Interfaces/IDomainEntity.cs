using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaidoz.ADManager.Interfaces
{
    public interface IDomainEntity 
    {
        public void ParseProperties(Principal principal);

        public void SaveProperties(Principal principal);
    }
}
