using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace EntityLayer.Concrete
{
    public class Log
    {
        [Key, Required]
        public int LogID { get; set; }

        [StringLength(50), Required]
        public string LogTable { get; set; }

        [StringLength(100), Required]
        public string LogOperatioType { get; set; }

        [StringLength(500), Required]
        public string LogInfo { get; set; }

    }
}
