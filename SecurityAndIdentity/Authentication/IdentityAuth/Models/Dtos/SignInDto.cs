﻿namespace IdentityAuth.Models.Dtos;

public class SignInDto
{
    public string EmailAddress { get; set; } = default!;
    public string Password { get; set; } = default!;
}