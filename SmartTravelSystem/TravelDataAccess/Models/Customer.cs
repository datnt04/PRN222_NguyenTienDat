using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelDataAccess.Models;

[Table("Customer")]
[Index("Code", Name = "UQ__Customer__A25C5AA7E11341BB", IsUnique = true)]
[Index("Email", Name = "UQ__Customer__A9D1053486D41AD1", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(150)]
    public string FullName { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string? Email { get; set; }

    public int? Age { get; set; }

    [StringLength(100)]
    public string Password { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
