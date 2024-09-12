using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Kaidoz.ADManager.Interfaces;
using Kaidoz.ADManager.Utilities;
using Kaidoz.ADManager.Utilities.Enums;

namespace Kaidoz.ADManager.Abstractions
{
    public abstract class BaseDomainEntity : IDomainEntity
    {
        /// <summary>
        /// Property 'ObjectSid'
        /// </summary>
        public byte[] ObjectSid { get; set; } = [];

        /// <summary>
        /// Property 'ObjectGUID'
        /// </summary>
        public byte[] ObjectGUID { get; set; } = [];

        public bool IsEnabled { get; set; }

        public EntityControlFlags ControlFlags { get; set; }

        /// <summary>
        /// Property 'displayName'
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Property 'description'
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Datetime UTC. Property 'lastLogon'
        /// </summary>
        public DateTime LastLogon { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Datetime UTC. Property 'whenCreated'
        /// </summary>
        public DateTime WhenCreated { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Datetime UTC. Property  'whenChanged'
        /// </summary>
        public DateTime WhenChanged { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Base dn to domain entity. Property 'distinguishedName'
        /// </summary>
        public string DistinguishedName { get; set; } = string.Empty;

        public void ParseProperties(Principal principal)
        {
            DirectoryEntry directoryEntry = (DirectoryEntry)principal.GetUnderlyingObject();

            PropertyCollection properties = directoryEntry.Properties;

            ParseBaseProperties(properties);
        }

        public void SaveProperties(Principal principal)
        {
            DirectoryEntry directoryEntry = (DirectoryEntry)principal.GetUnderlyingObject();

            PropertyCollection properties = directoryEntry.Properties;

            properties[ADProperties.Created].Value = WhenCreated;

            properties[ADProperties.Modified].Value = WhenChanged;

            properties[ADProperties.DisplayName].Value = DisplayName;

            SetControl(properties);
        }

        public void ParseBaseProperties(PropertyCollection propertyCollection)
        {
            ParseControlFlags(propertyCollection);

            if (propertyCollection.Contains(ADProperties.DisplayName))
            {
                DisplayName = propertyCollection[ADProperties.DisplayName].Value?.ToString()
                    ?? string.Empty;
            }

            if (propertyCollection.Contains(ADProperties.ObjectSid))
            {
                ObjectSid = (byte[])(propertyCollection[ADProperties.ObjectSid]?.Value
                    ?? throw new Exception(ADProperties.ObjectSid + " is null"));
            }

            if (propertyCollection.Contains(ADProperties.ObjectGuid))
            {
                ObjectGUID = (byte[])(propertyCollection[ADProperties.ObjectGuid]?.Value
                    ?? throw new Exception(ADProperties.ObjectGuid + " is null"));
            }

            if (propertyCollection.Contains(ADProperties.Created))
            {
                WhenCreated = (DateTime)(propertyCollection[ADProperties.Created].Value
                    ?? DateTime.MinValue);
            }

            if (propertyCollection.Contains(ADProperties.Modified))
            {
                WhenChanged = (DateTime)(propertyCollection[ADProperties.Modified].Value
                    ?? DateTime.MinValue);
            }

            if (propertyCollection.Contains(ADProperties.LastLogon))
            {
                LastLogon = (DateTime)(propertyCollection[ADProperties.LastLogon].Value
                    ?? DateTime.MinValue);
            }

            if (propertyCollection.Contains(ADProperties.Description))
            {
                Description = propertyCollection[ADProperties.Description].Value?.ToString()
                    ?? string.Empty;
            }

            if (propertyCollection.Contains(ADProperties.DistinguishedName))
            {
                DistinguishedName = propertyCollection[ADProperties.DistinguishedName].Value?.ToString()
                    ?? string.Empty;
            }
        }

        private void SetControl(PropertyCollection propertyCollection)
        {
            EntityControlFlags controlFlags = (EntityControlFlags)
                (propertyCollection[ADProperties.UserAccountControl].Value ??
                throw new Exception(ADProperties.UserAccountControl + " is null"));

            if (IsEnabled)
            {
                controlFlags |= EntityControlFlags.ACCOUNTDISABLE;
            }
            else
            {
                controlFlags &= ~EntityControlFlags.ACCOUNTDISABLE;
            }

            propertyCollection["userAccountControl"].Value = controlFlags;

        }

        private void ParseControlFlags(PropertyCollection propertyCollection)
        {
            var value = propertyCollection[ADProperties.UserAccountControl]?.Value;

            ArgumentNullException.ThrowIfNull(value, nameof(value));

            Debug.Assert(false);

            ControlFlags = (EntityControlFlags)value;

            if (ControlFlags.HasFlag(EntityControlFlags.ACCOUNTDISABLE))
                IsEnabled = false;
            else
                IsEnabled = true;
        }
    }
}
