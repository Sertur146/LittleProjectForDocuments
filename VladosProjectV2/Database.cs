using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VladosProjectV2
{
    class ProjectDB : DbContext
    {

        public ProjectDB() : base("DBConnection")
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ManagerNote> ManagerNotes { get; set; }
        public DbSet<ClientsArchiv> ClientsArchivs { get; set; }
        public DbSet<DocementsArchiv> DocementsArchivs { get; set; }

        public class Client
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Client_ID { get; set; }
            public string First_name { get; set; }
            public string Middle_name { get; set; }
            public string Surname { get; set; }
            public string Phone_number { get; set; }
            public string Comment { get; set; }
            public string Manager { get; set; }
        }

        public class ClientsArchiv
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Client_ID { get; set; }
            public string First_name { get; set; }
            public string Middle_name { get; set; }
            public string Surname { get; set; }
            public string Phone_number { get; set; }
            public string Comment { get; set; }
            public string Manager { get; set; }
        }

        public class ManagerNote
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Note_ID { get; set; }
            public int Client_ID { get; set; }
            public string NoteText { get; set; }
            public DateTime CreationDate { get; set; }
        }

        public class Document
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Document_ID { get; set; }
            public int Client_ID { get; set; }
            public string Document_name { get; set; }
            public string Document_path { get; set; }
            public DateTime Date_of_issue { get; set; }
            public DateTime Expiration_date { get; set; }
            public bool Status { get; set; }
        }

        public class DocementsArchiv
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Document_ID { get; set; }
            public int Client_ID { get; set; }
            public string Document_name { get; set; }
            public string Document_path { get; set; }
            public DateTime Date_of_issue { get; set; }
            public DateTime Expiration_date { get; set; }
            public bool Status { get; set; }

        }

        public class User
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UserID { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
