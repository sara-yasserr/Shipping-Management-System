﻿namespace Shipping.BusinessLogicLayer.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? cityId { get; set; }
        public string? StoreName { get; set; }

    }
}
