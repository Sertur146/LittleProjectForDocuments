using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VladosProjectV2
{
    class DBChanges
    {
        private enum Tables
        {
            Clients = 0,
            Documents
        }

        public enum Operations
        {
            Add = 0,
            Edit,
            Delete
        }

        private static ProjectDB dbContext = new ProjectDB();

        public static void Client_Change(Operations operation, string first_name, string middle_name,
            string surname, string phone_number, string comment, string manager)
        {
            var client_id = int.Parse(dbContext.Clients.Where
                (p => p.Phone_number.Contains(phone_number))
                   .Select(p => p.Client_ID).FirstOrDefault().ToString());
            var Client_First_Name = first_name;
            var Client_Middle_Name = middle_name;
            var Client_Surname = surname;
            var Client_Phone_Number = phone_number;
            var Client_Comment = comment;
            var Client_Manager = manager;
            ProjectDB.Client client;
            switch (operation)
            {
                case Operations.Add:
                    client = new ProjectDB.Client()
                    {
                        First_name = Client_First_Name,
                        Middle_name = Client_Middle_Name,
                        Surname = Client_Surname,
                        Phone_number = Client_Phone_Number,
                        Comment = Client_Comment,
                        Manager = Client_Manager
                    };
                    dbContext.Clients.Add(client);
                    break;
                case Operations.Edit:
                    client = dbContext.Clients.Find(client_id);
                    {
                        client.First_name = Client_First_Name;
                        client.Middle_name = Client_Middle_Name;
                        client.Surname = Client_Surname;
                        client.Phone_number = Client_Phone_Number;
                        client.Comment = Client_Comment;
                        client.Manager = Client_Manager;
                    };
                    break;
                case Operations.Delete:
                    client = dbContext.Clients.Find(client_id);
                    dbContext.Clients.Remove(client);
                    break;
            }
            dbContext.SaveChanges();
        }

        public static void ClientArchiv_Change(Operations operation, string first_name, string middle_name,
            string surname, string phone_number, string comment, string manager)
        {
            var client_id = int.Parse(dbContext.ClientsArchivs.Where
                (p => p.Phone_number.Contains(phone_number))
                   .Select(p => p.Client_ID).FirstOrDefault().ToString());
            var Client_First_Name = first_name;
            var Client_Middle_Name = middle_name;
            var Client_Surname = surname;
            var Client_Phone_Number = phone_number;
            var Client_Comment = comment;
            var Client_Manager = manager;
            ProjectDB.ClientsArchiv client;
            switch (operation)
            {
                case Operations.Add:
                    client = new ProjectDB.ClientsArchiv()
                    {
                        First_name = Client_First_Name,
                        Middle_name = Client_Middle_Name,
                        Surname = Client_Surname,
                        Phone_number = Client_Phone_Number,
                        Comment = Client_Comment,
                        Manager = Client_Manager
                    };
                    dbContext.ClientsArchivs.Add(client);
                    break;
                case Operations.Edit:
                    client = dbContext.ClientsArchivs.Find(client_id);
                    {
                        client.First_name = Client_First_Name;
                        client.Middle_name = Client_Middle_Name;
                        client.Surname = Client_Surname;
                        client.Phone_number = Client_Phone_Number;
                        client.Comment = Client_Comment;
                        client.Manager = Client_Manager;
                    };
                    break;
                case Operations.Delete:
                    client = dbContext.ClientsArchivs.Find(client_id);
                    dbContext.ClientsArchivs.Remove(client);
                    break;
            }
            dbContext.SaveChanges();
        }

        public static void ManagerNote_Change(Operations operation, int ClientID, string Note_text, DateTime Date)
        {
            var note_id = int.Parse(dbContext.ManagerNotes.AsEnumerable().Where
                (p => p.Client_ID == ClientID && p.CreationDate == Date)
                   .Select(p => p.Note_ID).FirstOrDefault().ToString());
            var client_id = ClientID;
            var note_text = Note_text;
            var date = Date;

            ProjectDB.ManagerNote note;
            switch (operation)
            {
                case Operations.Add:
                    note = new ProjectDB.ManagerNote()
                    {
                        Client_ID = client_id,
                        NoteText = note_text,
                        CreationDate = date
                    };
                    dbContext.ManagerNotes.Add(note);
                    break;
                case Operations.Edit:
                    note = dbContext.ManagerNotes.Find(note_id);
                    {
                        note.NoteText = note_text;
                        note.CreationDate = date;
                    };
                    break;
                case Operations.Delete:
                    note = dbContext.ManagerNotes.Find(note_id);
                    dbContext.ManagerNotes.Remove(note);
                    break;
            }
            dbContext.SaveChanges();
        }

        public static void Document_Change(Operations operation, int client_id, string doc_name, string doc_path,
            DateTime datestart, DateTime dateend, bool status)
        {
            var Document_id = int.Parse(dbContext.Documents.AsEnumerable().Where
                (p => p.Client_ID == client_id && p.Document_name == doc_name)
                   .Select(p => p.Document_ID).FirstOrDefault().ToString());
            var Client_id = client_id;
            var Document_Name = doc_name;
            var Document_Path = doc_path;
            var Date_Of_Issue = datestart;
            var Expiration_Date = dateend;
            var Document_status = status;
            ProjectDB.Document document;
            switch (operation)
            {
                case Operations.Add:
                    document = new ProjectDB.Document()
                    {
                        Client_ID = Client_id,
                        Document_name = Document_Name,
                        Document_path = Document_Path,
                        Date_of_issue = Date_Of_Issue,
                        Expiration_date = Expiration_Date,
                        Status = status
                    };
                    dbContext.Documents.Add(document);
                    break;
                case Operations.Edit:
                    document = dbContext.Documents.Find(Document_id);
                    {
                        document.Date_of_issue = Date_Of_Issue;
                        document.Expiration_date = Expiration_Date;
                        document.Status = status;
                    };
                    dbContext.Documents.Add(document);
                    break;
                case Operations.Delete:
                    document = dbContext.Documents.Find(Document_id);
                    dbContext.Documents.Remove(document);
                    break;
            }
            dbContext.SaveChanges();
        }

        public static void DocumentArchiv_Change(Operations operation, int client_id, string doc_name, string doc_path,
            DateTime datestart, DateTime dateend, bool status)
        {
            var Document_id = int.Parse(dbContext.DocementsArchivs.AsEnumerable().Where
                (p => p.Client_ID == client_id && p.Document_name == doc_name)
                   .Select(p => p.Document_ID).FirstOrDefault().ToString());
            var Client_id = client_id;
            var Document_Name = doc_name;
            var Document_Path = doc_path;
            var Date_Of_Issue = datestart;
            var Expiration_Date = dateend;
            var Document_status = status;
            ProjectDB.DocementsArchiv document;
            switch (operation)
            {
                case Operations.Add:
                    document = new ProjectDB.DocementsArchiv()
                    {
                        Client_ID = Client_id,
                        Document_name = Document_Name,
                        Document_path = Document_Path,
                        Date_of_issue = Date_Of_Issue,
                        Expiration_date = Expiration_Date,
                        Status = status
                    };
                    dbContext.DocementsArchivs.Add(document);
                    break;
                case Operations.Edit:
                    document = dbContext.DocementsArchivs.Find(Document_id);
                    {
                        document.Date_of_issue = Date_Of_Issue;
                        document.Expiration_date = Expiration_Date;
                        document.Status = status;
                    };
                    dbContext.DocementsArchivs.Add(document);
                    break;
                case Operations.Delete:
                    document = dbContext.DocementsArchivs.Find(Document_id);
                    dbContext.DocementsArchivs.Remove(document);
                    break;
            }
            dbContext.SaveChanges();
        }

        public static void User_Change(Operations operation, int user_id, string login, string password)
        {
            var User_id = int.Parse(dbContext.Users.AsEnumerable().Where
                (p => p.Login == login).Select(p => p.UserID).FirstOrDefault().ToString());
            var User_login = login;
            var User_password = password;
            ProjectDB.User user;
            switch (operation)
            {
                case Operations.Add:
                    user = new ProjectDB.User()
                    {
                        Login = User_login,
                        Password = User_password
                    };
                    dbContext.Users.Add(user);
                    break;
                case Operations.Edit:
                    user = dbContext.Users.Find(User_id);
                    {
                        user.Login = User_login;
                        user.Password = User_password;
                    };
                    dbContext.Users.Add(user);
                    break;
                case Operations.Delete:
                    user = dbContext.Users.Find(User_id);
                    dbContext.Users.Remove(user);
                    break;
            }
            dbContext.SaveChanges();
        }
    }
}
