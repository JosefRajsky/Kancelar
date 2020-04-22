using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailSender_Api
{
    public class Mail
    {
        [Key]
        public Guid Id { get; set; }        
        public Guid? EventGuid { get; set; }
        public int Generation { get; set; }
        public Guid MailId { get; set; }
        public string Value1 { get; set; }
        public int Value2 { get; set; }

    }
}
