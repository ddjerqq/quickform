using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Forms;
using QuickForm.Attributes;

namespace QuickForm.Example;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum FavoriteAnimal
{
    [Display(Name = "😺 cat")]
    A,

    [Display(Name = "🐶 dog")]
    B,

    [Display(Name = "🦝 raccoon")]
    C,
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class RegisterCommand
{
    [Required]
    [Placeholder]
    [DisplayName("Email Address")]
    [EmailAddress]
    [ValidFeedback("Looks good!")]
    [Description("We will never share your email with anyone")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    [Placeholder("Enter password...")]
    [DataType(DataType.Password)]
    [Description("We will never share your password with anyone")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,32}$", ErrorMessage = "Password must have at least 8 characters, one uppercase letter, one lowercase letter...")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DisplayName("Birth Date")]
    [Description("Please enter your birth date")]
    [Range(typeof(DateTime), "1930-01-01", "2006-01-01", ErrorMessage = "Birth date must be between 1930-01-01 and 2006-01-01")]
    [DateType(InputDateType.Date)]
    public DateTime BirthDate { get; set; }

    [Required]
    [DisplayName("Favorite Animal")]
    [Description("Please select one")]
    public FavoriteAnimal FavAnimal { get; set; }
}