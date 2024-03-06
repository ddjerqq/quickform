# Validation

QuickForm has two types of validation

- `DataAnnotationsValidator`
- [`Blazored.FluentValidationValidator`](https://github.com/Blazored/FluentValidation)

## DataAnnotationAttributes

```csharp
public class User
{
    [Required]
    public string Name { get; set; }
    
    [Range(18, 100. ErrorMessage = "You must be 18 years or older to register")]
    public int Age { get; set; }
    
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,32}$", ErrorMessage = "Password must have at least 8 characters, one uppercase letter, one lowercase letter...")]
    public string Password { get; set; }
}
```

## FluentValidation

```csharp
public class ProductCreateCommand
{
    public string Name { get; set; }
    
    public decimal Price { get; set; }
}

public class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MustAsync(async (name, product, cancellationToken) => {
                var product = await productRepository.GetByNameAsync(name, cancellationToken);
                return product is null;
            })
            .WithMessage("Product with this name already exists");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}
```


!!! info

    These validation rules will be automatically applied to your form, 
    all fields will get validated when they are changed.