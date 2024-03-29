﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class RefreshToken
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(128)] public Guid Token { get; set; }

    public Player User { get; set; }

    [ForeignKey("User")] public string UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ToLife { get; set; }
    public bool IsExpired { get; set; }
    public string IpAddress { get; set; }
}