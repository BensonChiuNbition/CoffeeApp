
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoffeeAppWebRole.Models
{
    public class CustomerEntity : TableServiceEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public CustomerEntity() { }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9_\\-]+)*\\.([a-z]{2,4})$",
            ErrorMessage = "Not a valid e-mail address.")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}