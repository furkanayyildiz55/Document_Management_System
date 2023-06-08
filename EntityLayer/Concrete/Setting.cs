using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class Setting
    {
        [Key]
        public int LdapID { get; set; }

        public bool LdapActive { get; set; }

        public string LdapUrl { get; set; }

        public string LdapUserName { get; set; }
        
        public string LdapPassword { get; set; }

        public int  LdapPort { get; set; }
    }
}
