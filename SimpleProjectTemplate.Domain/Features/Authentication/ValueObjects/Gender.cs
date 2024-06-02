using SimpleProjectTemplate.Domain.Abstract;
using SimpleProjectTemplate.Domain.Features.Authentication.Exceptions;

namespace SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;

public class Gender : ValueObject
{
    public Gender(string value)
    {
        if (!value.Equals("male") && !value.Equals("female"))
        {
            throw new GenderValidationException();
        }
        
        Value = value;
    }
    
    public string Value { get; set; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static Gender Male => new Gender("Male");
    public static Gender Female => new Gender("Female");
    public static Gender Nonbinary => new Gender("Nonbinary");

    public static IEnumerable<Gender> AllGenders =>
        new List<Gender>()
        {
            Male, Female, Nonbinary
        };
}