using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace EntityLayer.Concrete
{
    public class Log
    {
        [Key]
        public int LogID { get; set; }

        public string LogTable { get; set; }

        public string LogOperatioType { get; set; }

        public string LogInfo { get; set; }

    }
}
