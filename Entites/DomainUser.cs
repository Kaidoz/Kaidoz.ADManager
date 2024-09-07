using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDZ.ActiveDirectory.Models
{
    public class DomainUser
    {
        internal DomainUser(UserPrincipal principal)
        {
            ParseUser(principal);
        }

        private void ParseUser(UserPrincipal principal)
        {
            Username = principal.SamAccountName;

            FirstName = principal.Name;

            LastName = principal.Surname;

            MiddleName = principal.MiddleName;

            if (principal.Enabled != null)
                Enabled = principal.Enabled.Value;

            if (principal.LastLogon != null)
                LastLoginDate = principal.LastLogon.Value;

            ParseProperties(principal);
        }

        private void ParseProperties(UserPrincipal principal)
        {
            DirectoryEntry directoryEntry = (DirectoryEntry)principal.GetUnderlyingObject();

            System.DirectoryServices.PropertyCollection properties = directoryEntry.Properties;


            if (properties.Contains("title"))
                Title = properties["title"].Value.ToString();

            if (properties.Contains("department"))
                Department = properties["department"].Value.ToString();

            if (properties.Contains("mobile"))
                Mobile = properties["mobile"].Value.ToString();

            if (properties.Contains("ipPhone"))
                IpTelephone = properties["ipPhone"].Value.ToString();

            if (properties.Contains("homePhone"))
                HomePhone = properties["homePhone"].Value.ToString();

            if (properties.Contains("manager"))
                Manager = properties["manager"].Value.ToString();
        }

        public void CompleteChanges(UserPrincipal userPrincipal)
        {
            userPrincipal.Name = FirstName;
            userPrincipal.Surname = LastName;
            userPrincipal.Enabled = Enabled;
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

        public bool Enabled { get; set; }

        public DateTime LastLoginDate { get; set; }


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
        /// Manager
        /// </summary>
        public string Manager { get; set; } = null!;
    }
}
