using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Dapper;
using System.Security.Claims;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using smartcube.model;

namespace smartcube.Model
{
    public class Users
    {
        public int user_id { get; set; }
        public string? first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public byte[] hash_password { get; set; }
        public byte[] salt_password { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public Users()
        {
            if (first_name == null) first_name = "";
            if (last_name == null) last_name = "";
            if (email == null) email = "";
            if (hash_password == null) hash_password = new byte[0];
            if (salt_password == null) salt_password = new byte[0];
        }
    }
}