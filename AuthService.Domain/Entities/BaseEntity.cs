using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entities;

public abstract class BaseEntity<TIdentity>
{
    [Key]
    public TIdentity Id { get; set; }
}