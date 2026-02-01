using System;
using System.Collections.Generic;

namespace Platform.Api.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public LoginUser User { get; set; } = null!;
        public object Modules { get; set; } = null!;
    }

    public class LoginUser
    {
        public Guid Id { get; set; }
        public string Usuario { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }
}