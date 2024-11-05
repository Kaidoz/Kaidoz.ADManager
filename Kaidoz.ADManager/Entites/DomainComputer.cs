using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaidoz.ADManager.Abstractions;

namespace Kaidoz.ADManager.Entities
{
    public class DomainComputer : BaseDomainEntity
    {
        public string Name { get; set; }

        internal DomainComputer(ComputerPrincipal principal)
        {
            Name = principal.Name;
            Description = principal.Description;

            LastLogon = principal.LastLogon
                ?? DateTime.MinValue;

            
        }

        private void ParseProperties(ComputerPrincipal principal)
        {
            DirectoryEntry directoryEntry = (DirectoryEntry)principal.GetUnderlyingObject();

            PropertyCollection properties = directoryEntry.Properties;


        }
    }
}
