using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Email_MVC.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; }
        [Column(TypeName = "TEXT")]
        public string Content { get; set; }
        public int? SenderId { get; set; }
        public DateTime SendTime { get; set; }
        [ForeignKey("SenderId")]
        public virtual Users User { get; set; }
        public List<Inbox> Inbox { get; set; }
        public List<Outbox> Outbox { get; set; }
        public Message() { }
    }
}