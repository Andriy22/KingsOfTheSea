﻿namespace Application.Common.DTOs.Auth;

public class RefreshTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}