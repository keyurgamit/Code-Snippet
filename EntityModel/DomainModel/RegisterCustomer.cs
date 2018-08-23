using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModel.DomainModel
{
    public class RegisterCustomer : UserRole
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserRoleId { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public bool AgreeToReceiverEmailCommunication { get; set; }

        //public string Name { get; set; }
        public bool Published { get; set; }
        public bool IsDeleted { get; set; }

        virtual public int TotalPages { get; set; }
        virtual public int TotalRecords { get; set; }

        public List<UserRole> lstUserRole { get; set; }

        public RegisterCustomer()
        {
            lstUserRole = new List<UserRole>();
        }
    }

    public class UserRole
    {
        [Column("Id")]
        public int RoleId { get; set; }

        [Column("Name")]
        public string RoleName { get; set; }
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
    }

    public class ContactUs
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageContent { get; set; }
    }
}