using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoRenderHermes.ADOApp;

namespace VideoRenderHermes.Classes
{
    public class ClientInformation
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public int GenderID { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public byte[] Photo { get; set; }
        public string Gender { get; set; }
        public DateTime LastDate { get; set; }
        public int VisitCount { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public ClientInformation(Client client)
        {
            Id = client.Id;
            Firstname = client.Firstname;
            Lastname = client.Lastname;
            Patronymic = client.Patronymic;
            GenderID = client.GenderID;
            BirthDate = client.BirthDate;
            PhoneNumber = client.PhoneNumber;
            Email = client.Email;
            AddedDate = client.AddedDate;
            Photo = client.Photo;
            Gender = App.Connection.Gender.FirstOrDefault(x => x.GenderID == GenderID).Name;
            if(client.Visit.LastOrDefault(x => x.Id != null) != null)
            {
                LastDate = client.Visit.LastOrDefault(x => x.Id != null).Date;
            }
            
            VisitCount = client.Visit.Count();
            
            foreach(var ct in client.ClientTag)
            {
                Tags.Add(ct.Tag);
            }
        }
    }
}
