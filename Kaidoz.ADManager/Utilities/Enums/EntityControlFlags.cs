using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaidoz.ADManager.Utilities.Enums
{
    [Flags]
    public enum EntityControlFlags
    {
        SCRIPT = 0x0001,
        ACCOUNTDISABLE = 0x0002,
        HOMEDIR_REQUIRED = 0x0008,
        LOCKOUT = 0x0010,
        PASSWD_NOTREQD = 0x0020,
        PASSWD_CANT_CHANGE = 0x0040,
        ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x0080,
        TEMP_DUPLICATE_ACCOUNT = 0x0100,
        NORMAL_ACCOUNT = 0x0200,
        INTERDOMAIN_TRUST_ACCOUNT = 0x0800,
        WORKSTATION_TRUST_ACCOUNT = 0x1000,
        SERVER_TRUST_ACCOUNT = 0x2000,
        DONT_EXPIRE_PASSWORD = 0x10000,
        PASSWORD_EXPIRED = 0x80000,
        TRUSTED_FOR_DELEGATION = 0x200000,
        NOT_DELEGATED = 0x400000,
        USE_DES_KEY_ONLY = 0x800000,
        DONT_REQUIRE_PREAUTH = 0x1000000,
        TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x2000000
    }
}
