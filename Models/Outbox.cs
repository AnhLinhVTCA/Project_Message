using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Email_MVC.Models
{
    public class Outbox
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int? SenderId { get; set; }
        public int IsDeleted { get; set; }
        // public bool IsDeleted
        // {
        //     get { return Deleted == 1; }
        //     set { Deleted = value ? 1 : 0; }
        // }
        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; }

        public Outbox() { }
    }
}