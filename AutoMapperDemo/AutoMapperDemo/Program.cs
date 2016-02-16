using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Web; 

namespace AutoMapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            Mapper.CreateMap<Customer, CustomerViewItem>()
               .ForMember(cv => cv.FullName, m => m.MapFrom(s => s.FirstName + " " + s.LastName))
              .ForMember(cv => cv.VIP, m => m.ResolveUsing<VIPResolver>().FromMember(x => x.VIP))
              .ForMember(cv => cv.DateOfBirth, m => m.AddFormatter<DateFormatter>());           
            Mapper.AssertConfigurationIsValid();        

            program.Run();
        }

        private void Run()
        {
            Customer customer = GetCustomerFromDB();

            CustomerViewItem customerViewItem = Mapper.Map<Customer, CustomerViewItem>(customer);            

            //CustomerViewItem customerViewItem = new CustomerViewItem()
            //{
            //    FirstName = customer.FirstName,
            //    LastName = customer.LastName,
            //    DateOfBirth = customer.DateOfBirth,
            //    NumberOfOrders = customer.NumberOfOrders
            //};

            //ShowCustomerInDataGrid(customerViewItem);

            ShowCustomerInDataGrid(customerViewItem);
        }

        private void ShowCustomerInDataGrid(CustomerViewItem customerViewItem) { }

        private Customer GetCustomerFromDB()
        {
            return new Customer()
            {
                DateOfBirth = new DateTime(1995, 08, 17),
                FirstName = "Loki",
                LastName = "Alice",
                NumberOfOrders = 7,
                Company = new Company() { Name = "Terralogic",Address = "HCM City" },
                VIP = false
            };
        }
    }

    public class Customer
    {
        public Company Company { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int NumberOfOrders { get; set; }

        public bool VIP { get; set; }

    }

    public class Company
    {
        public string Name { get; set; }
        public String Address { get; set; }
    }

    public class CustomerViewItem
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        //public string DateOfBirth { get; set; }

        public int NumberOfOrders { get; set; }

        public string VIP { get; set; }
    }

    public class VIPResolver : ValueResolver<bool, string>
    {
        protected override string ResolveCore(bool source)
        {
            return source ? "Y" : "N";
        }
    }

    public class DateFormatter : IValueFormatter
    {
        public string FormatValue(ResolutionContext context)
        {
            return ((DateTime)context.SourceValue).ToLongDateString();
        }
    }
}