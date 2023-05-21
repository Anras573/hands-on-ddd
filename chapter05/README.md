# Chapter 05: Implementing the Model

This is the first chapter in which we actually write code, so this is where my reimplementation of the code starts to differ. I have tried to document all the differences between my code and the one [Alexey](https://github.com/alexeyzimarev) provides in his book.

## Changes

* Treat Warnings as Errors
* Records instead of classes
* Nullables
* More Unit Tests
* Exceptions
* EnsureValidState

### Treat Warnings as Errors

One of the first thing I did, was to turn on the `Treat Warnings as Error` setting in the `.csproj` file. By doing so, I'm forced to handle all the compiler warnings as errors. I strongly believe the compiler warnings are valuable and should not be ignored, as they often indicate potential issues or code smells. Treating warnings as error ensures that I address them promptly, leading to cleaner and more robust code.

```xml
<!-- .csproj file -->
<Project Sdk="Microsoft.NET.Sdk">

  <!-- Other PropertyGroups -->

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <TreatWarningsAsErrors>True</TreatWarningsAsErrors>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <TreatWarningsAsErrors>True</TreatWarningsAsErrors>
  </PropertyGroup>

  <!-- ItemGroups here -->

</Project>
```

### Records instead of classes

In the book, [Alexey](https://github.com/alexeyzimarev) mentioned that in newer versions of C# he would be using `record types` to eliminate much of the boilerplate code associated with value objects. Following his suggestion, I opted to use record types in my reimplementation. Records in C# provide value-based equality and immutability, which removes the need for the boilerplate code associated with value objects.

```csharp
public record ClassifiedAdText(string Value)
{
    public static ClassifiedAdText FromString(string value) => new (value);

    public static implicit operator string(ClassifiedAdText self) => self.Value;
}
```

### Nullables

To handle the possibility of null values for properties such as `Title`, `Text`, `Price`, and `ApprovedBy` I marked them as nullable. This allows for more precise modeling of the domain by indicating that these properties can have a value or be null. Although I may consider introducing the Maybe Monad in future iterations, nullable types currently serve the purpose effectively.

```csharp
public class ClassifiedAd : Entity
{
    // Other properties omitted for brevity

    public ClassifiedAdTitle? Title { get; private set; }
    public ClassifiedAdText? Text { get; private set; }
    public Price? Price { get; private set; }
    public UserId? ApprovedBy { get; private set; }

    // Rest of the code skipped
}
```

### More Unit Tests

In my reimplementation, I emphasized the importance of having comprehensive unit tests to ensure the validity of domain objects. I added more unit tests to cover various scenarios, such as invalid instantiations of domain objects. By having explicit unit tests for each rule and constraint, I can validate that the domain objects behave as expected and adhere to the specified business rules.

```csharp
namespace Marketplace.Domain.Tests;

public class PriceTest
{
    private static readonly ICurrencyLookup CurrencyLookup = new FakeCurrencyLookup();

    [Fact]
    public void Price_cannot_be_instantiated_with_a_negative_amount()
    {
        Assert.Throws<ArgumentException>(() => Price.FromDecimal(-1, "EUR", CurrencyLookup));
    }
}
```

While I understand that the book may not focus extensively on unit testing, I believe that comprehensive unit tests play a crucial role in ensuring the correctness and robustness of the codebase.

### Exceptions

During my implementation, I noticed that [Alexey](https://github.com/alexeyzimarev) had switched the order of parameters in some exception constructors. For example, the ArgumentOutOfRangeException constructor takes the paramName first and the message as the second parameter. To maintain consistency and follow established conventions, I reverted the order of parameters in the exception constructors.

```csharp
// From ClassifiedAdTitle.cs
private static void CheckValidity(string value)
{
    if (value.Length > 100)
        throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 100 characters");
}
```

### EnsureValidState

In the original book, [Alexey](https://github.com/alexeyzimarev) refactored the validation logic to a single method named `EnsureValidState()`. However, I noticed that this refactoring resulted in losing some valuable information. When `EnsureValidState()` is invoked, it only informs the user that the entity is in an invalid state without specifying what exactly is wrong with the state. Although it might make sense that the recent user action caused the state to become invalid, it may not always be the case. To provide more useful feedback, I modified the `EnsureValidState()` method as follows:

```csharp
protected override void EnsureValidState()
{
    if (State == ClassifiedAdState.Inactive)
        return;

    var errors = new List<string>();

    if (Title is null)
        errors.Add($"{nameof(Title)} cannot be empty");

    if (Text is null)
        errors.Add($"{nameof(Text)} cannot be empty");

    if (Price is null || Price?.Amount == 0)
        errors.Add($"{nameof(Price)} cannot be zero");

    if (State == ClassifiedAdState.Active && ApprovedBy is null)
        errors.Add("Approver must be set");

    if (errors.Any())
        throw new InvalidEntityStateException(this, string.Join(", ", errors));
}
```

## Conclusion

In this chapter, I detailed the changes I made to the code examples presented in the book. My goal was to reimplement the code in a manner that aligns with my preferred coding style and principles, while staying true to the concepts of Domain-Driven Design (DDD) and utilizing newer versions of .NET. By treating warnings as errors, utilizing records, introducing nullables, writing comprehensive unit tests, aligning exception parameter order, and enhancing the `EnsureValidState()` method, I aimed to create a codebase that is more robust, readable, and informative. These changes reflect my personal coding style and provide a solid foundation for further development and exploration of DDD concepts in a .NET environment.
