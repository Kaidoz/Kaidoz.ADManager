using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaidoz.ADManager.Abstractions;
using Kaidoz.ADManager.Utilities;

namespace KDZ.ActiveDirectory.Models
{
    public class DomainUser : BaseDomainEntity
    {
        internal DomainUser(UserPrincipal principal)
        {
            ParseUser(principal);
        }

        public new void ParseProperties(Principal principal)
        {
            ParseUser((UserPrincipal)principal);

            base.ParseProperties(principal);
        }

        private void ParseUser(UserPrincipal principal)
        {
            Username = principal.SamAccountName;

            FirstName = principal.Name;

            LastName = principal.Surname;

            MiddleName = principal.MiddleName;

            ParseAdditionalyProperties(principal);
        }

        private void ParseAdditionalyProperties(UserPrincipal principal)
        {
            DirectoryEntry directoryEntry = (DirectoryEntry)principal.GetUnderlyingObject();

            System.DirectoryServices.PropertyCollection properties = directoryEntry.Properties;


            if (properties.Contains(ADProperties.Title))
                Title = properties[ADProperties.Title].Value?.ToString() 
                    ?? string.Empty;

            if (properties.Contains("department"))
                Department = properties["department"].Value?.ToString()
                    ?? string.Empty;

            if (properties.Contains("mobile"))
                Mobile = properties["mobile"].Value?.ToString()
                    ?? string.Empty;

            if (properties.Contains("ipPhone"))
                IpTelephone = properties["ipPhone"].Value?.ToString()
                    ?? string.Empty;

            if (properties.Contains("homePhone"))
                HomePhone = properties["homePhone"].Value?.ToString()
                    ?? string.Empty;

            if (properties.Contains("manager"))
                Manager = properties["manager"].Value?.ToString()
                    ?? string.Empty;


        }

        public void CompleteChanges(UserPrincipal userPrincipal)
        {
            userPrincipal.Name = FirstName;
            userPrincipal.Surname = LastName;
            userPrincipal.Enabled = IsEnabled;
            userPrincipal.SamAccountName = Username;
            userPrincipal.DisplayName = DisplayName;
            userPrincipal.MiddleName = MiddleName;

            SetProperties(userPrincipal);
        }

        private void SetProperties(UserPrincipal userPrincipal)
        {
            DirectoryEntry directoryEntry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();

            System.DirectoryServices.PropertyCollection properties = directoryEntry.Properties;

            userPrincipal.Name = FirstName;

            userPrincipal.SamAccountName = Username;

            properties["title"].Value = Title;
            properties["department"].Value = Department;
            properties["mobile"].Value = Mobile;
            properties["ipPhone"].Value = IpTelephone;
            properties["homePhone"].Value = HomePhone;
            properties["manager"].Value = Manager;
        }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; } = null!;

        /// <summary>
        /// Mobile
        /// </summary>
        public string Mobile { get; set; } = null!;

        /// <summary>
        /// IpPhone
        /// </summary>
        public string IpTelephone { get; set; } = null!;

        /// <summary>
        /// HomePhone
        /// </summary>
        public string HomePhone { get; set; } = null!;

        /// <summary>
        /// Manager. Readonly, for change use UserContext
        /// </summary>
        public string Manager { get; protected set; } = null!;
    }
}
