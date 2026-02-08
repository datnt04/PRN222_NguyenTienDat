using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelDataAccess.Models;

[Table("Booking")]
public partial class Booking
{
    [Key]
    [Column("BookingID")]
    public int BookingId { get; set; }

    [Column("TripID")]
    public int TripId { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    public DateOnly BookingDate { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("Bookings")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("TripId")]
    [InverseProperty("Bookings")]
    public virtual Trip Trip { get; set; } = null!;
}
