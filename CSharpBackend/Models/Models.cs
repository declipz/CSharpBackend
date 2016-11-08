using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CSharpBackend.Models
{
    public class Constants
    {
        public const string AUTH_KEY = "wTj64Ew98J0awR";

        
    }
    public class Error
    {
        public Error(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public User(int id, string email, string password)
        {
            this.id = id;
            this.email = email;
            this.password = password;
        }
    }
    public class Platform
    {
        public int id { get; set; }
        public string platform_name { get; set; }

        public Platform(int id, string platform_name)
        {
            this.id = id;
            this.platform_name = platform_name;
        }
    }

    public class Brand
    {
        public int id { get; set; }
        public int platform_id { get; set; }
        public string brand_name { get; set; }

        public Brand(int id, int platform_id, string brand_name)
        {
            this.id = id;
            this.platform_id = platform_id;
            this.brand_name = brand_name;
        }
    }

    public class Device
    {
        public int id { get; set; }
        public int brand_id { get; set; }
        public int platform_id { get; set; }
        public string name { get; set; }
        public string cpu { get; set; }
        public string ram { get; set; }
        public int price { get; set; }
        public string description { get; set; }

        public Device(int id, int brand_id, int platform_id, string name, string cpu, string ram, int price, string description)
        {
            this.id = id;
            this.brand_id = brand_id;
            this.platform_id = platform_id;
            this.name = name;
            this.cpu = cpu;
            this.ram = ram;
            this.price = price;
            this.description = description;
        }
    }

    public class UserBlank
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
    
    public class CartItem
    {
        public int id { get; set; }
        public int device_id { get; set; }
        public int user_id { get; set; }
        public CartItem(int id, int device_id, int user_id)
        {
            this.id = id;
            this.device_id = device_id;
            this.user_id = user_id;
        }
    }

    public class OrderItem
    {
        public int id { get; set; }
        public int device_id { get; set; }
        public int user_id { get; set; }
        public DateTime date { get; set; }
        public OrderItem(int id, int device_id, int user_id, DateTime date)
        {
            this.id = id;
            this.device_id = device_id;
            this.user_id = user_id;
            this.date = date;
        }
    }

    public class Picture
    {
        public int id { get; set; }
        public int device_id { get; set; }
        public string url { get; set; }
        public Picture(int id, int device_id, string url)
        {
            this.id = id;
            this.device_id = device_id;
            this.url = url;
        }
    }
}