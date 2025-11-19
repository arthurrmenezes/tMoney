using tMoney.Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects;

public class FirstNameValueObjectTest
{
    [Fact]
    public void GivenValidName_WhenFactoryCalled_ThenShouldCreateInstance()
    {
        // Arrange
        var firstName = "Arthur";

        // Act
        var firstNameValueObject = FirstNameValueObject.Factory(firstName);

        // Assert
        Assert.NotNull(firstNameValueObject);
        Assert.Equal(firstName, firstNameValueObject.FirstName);
    }

    [Theory]
    [InlineData("arthur", "Arthur")]
    [InlineData("ARTHUR", "Arthur")]
    [InlineData("aRtHuR", "Arthur")]
    [InlineData("  arthur  ", "Arthur")]
    public void GivenUnformattedName_WhenFactoryCalled_ThenShouldNormalizeToTitleCaseAndTrim(string input, string expected)
    {
        var firstNameValueObject = FirstNameValueObject.Factory(input);

        Assert.Equal(expected, firstNameValueObject.FirstName);
    }

    [Fact]
    public void GivenNameWithExactMinLength_WhenFactoryCalled_ThenShouldCreate()
    {
        var firstName = "Ana";
        var firstNameValueObject = FirstNameValueObject.Factory(firstName);
        Assert.Equal("Ana", firstNameValueObject.FirstName);
    }

    [Fact]
    public void GivenNameWithExactMaxLength_WhenFactoryCalled_ThenShouldCreate()
    {
        var firstName = new string('a', 50);
        var firstNameValueObject = FirstNameValueObject.Factory(firstName);

        var expected = "A" + new string('a', 49);

        Assert.Equal(expected, firstNameValueObject.FirstName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenNullOrEmpty_WhenFactoryCalled_ThenShouldThrowArgumentException(string invalidName)
    {
        var exception = Assert.Throws<ArgumentException>(() => FirstNameValueObject.Factory(invalidName));
        Assert.Equal("O primeiro nome não pode ser nulo ou vazio.", exception.Message);
    }

    [Theory]
    [InlineData("Jo")]
    [InlineData("A")]
    [InlineData("  A  ")]
    public void GivenNameTooShort_WhenFactoryCalled_ThenShouldThrowArgumentException(string shortName)
    {
        var exception = Assert.Throws<ArgumentException>(() => FirstNameValueObject.Factory(shortName));
        Assert.Contains($"pelo menos {FirstNameValueObject.MinLength} caracteres", exception.Message);
    }

    [Fact]
    public void GivenNameTooLong_WhenFactoryCalled_ThenShouldThrowArgumentException()
    {
        var longName = new string('a', 51);

        var exception = Assert.Throws<ArgumentException>(() => FirstNameValueObject.Factory(longName));
        Assert.Contains($"não pode ultrapassar {FirstNameValueObject.MaxLength} caracteres", exception.Message);
    }

    [Theory]
    [InlineData("Arthur1")]
    [InlineData("123")]
    [InlineData("Jo4o")]
    public void GivenNameWithDigits_WhenFactoryCalled_ThenShouldThrowArgumentException(string nameWithDigits)
    {
        var exception = Assert.Throws<ArgumentException>(() => FirstNameValueObject.Factory(nameWithDigits));
        Assert.Equal("O primeiro nome não pode conter números.", exception.Message);
    }

    [Fact]
    public void GivenEqualNames_WhenComparing_ThenShouldBeEqual()
    {
        var name1 = FirstNameValueObject.Factory("arthur");
        var name2 = FirstNameValueObject.Factory("Arthur");

        Assert.Equal(name1, name2);
        Assert.True(name1.Equals(name2));
    }

    [Fact]
    public void GivenDifferentNames_WhenComparing_ThenShouldNotBeEqual()
    {
        var name1 = FirstNameValueObject.Factory("Arthur");
        var name2 = FirstNameValueObject.Factory("Maria");

        Assert.NotEqual(name1, name2);
        Assert.False(name1.Equals(name2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHash()
    {
        var name1 = FirstNameValueObject.Factory("Arthur");
        var name2 = FirstNameValueObject.Factory("ARTHUR");

        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }
}
