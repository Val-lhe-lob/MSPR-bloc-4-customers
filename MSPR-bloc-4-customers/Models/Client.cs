using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSPR_bloc_4_customers.Models;

public partial class Client
{

    public int IdClient { get; set; }

    [Required]
    [StringLength(100)]
    public string Nom { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Prenom { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Ville { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string CodePostal { get; set; } = null!;

    [StringLength(100)]
    public string? Entreprise { get; set; }



}
