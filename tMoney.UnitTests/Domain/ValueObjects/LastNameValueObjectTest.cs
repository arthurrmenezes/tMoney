using tMoney.Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects;

public class LastNameValueObjectTest
{
    [Fact]
    public void GivenValidLastName_WhenFactoryCalled_ThenShouldCreateInstance()
    {
        var lastName = "Silva";

        var lastNameValueObject = LastNameValueObject.Factory(lastName);

        Assert.NotNull(lastNameValueObject);
        Assert.Equal(lastName, lastNameValueObject.LastName);
    }

    [Theory]
    [InlineData("santos", "Santos")]
    [InlineData("SANTOS", "Santos")]
    [InlineData("sAnToS", "Santos")]
    [InlineData("  santos  ", "Santos")]
    [InlineData("da silva", "Da Silva")]
    public void GivenUnformattedLastName_WhenFactoryCalled_ThenShouldNormalizeToTitleCaseAndTrim(string input, string expected)
    {
        var lastNameValueObject = LastNameValueObject.Factory(input);

        Assert.Equal(expected, lastNameValueObject.LastName);
    }

    [Fact]
    public void GivenLastNameWithExactMinLength_WhenFactoryCalled_ThenShouldCreate()
    {
        var lastName = "Luz";

        var lastNameValueObject = LastNameValueObject.Factory(lastName);

        Assert.Equal("Luz", lastNameValueObject.LastName);
    }

    [Fact]
    public void GivenLastNameWithExactMaxLength_WhenFactoryCalled_ThenShouldCreate()
    {
        var lastName = new string('a', 50);

        var lastNameValueObject = LastNameValueObject.Factory(lastName);

        var expected = "A" + new string('a', 49);
        Assert.Equal(expected, lastNameValueObject.LastName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenNullOrEmpty_WhenFactoryCalled_ThenShouldThrowArgumentException(string invalidLastName)
    {
        Assert.Throws<ArgumentException>(() => LastNameValueObject.Factory(invalidLastName));
    }

    [Theory]
    [InlineData("Sá")]
    [InlineData("Li")]
    [InlineData("  A  ")]
    public void GivenLastNameTooShort_WhenFactoryCalled_ThenShouldThrowArgumentException(string shortLastName)
    {
        Assert.Throws<ArgumentException>(() => LastNameValueObject.Factory(shortLastName));
    }

    [Fact]
    public void GivenLastNameTooLong_WhenFactoryCalled_ThenShouldThrowArgumentException()
    {
        var longLastName = new string('a', 51);

        Assert.Throws<ArgumentException>(() => LastNameValueObject.Factory(longLastName));
    }

    [Theory]
    [InlineData("Silva2")]
    [InlineData("123")]
    [InlineData("San7os")]
    public void GivenLastNameWithDigits_WhenFactoryCalled_ThenShouldThrowArgumentException(string lastNameWithDigits)
    {
        Assert.Throws<ArgumentException>(() => LastNameValueObject.Factory(lastNameWithDigits));
    }

    [Fact]
    public void GivenEqualLastNames_WhenComparing_ThenShouldBeEqual()
    {
        var lastName1 = "Oliveira";
        var lastName2 = "oliveira";

        var firstLastNameValueObject = LastNameValueObject.Factory(lastName1);
        var secondLastNameValueObject = LastNameValueObject.Factory(lastName2);

        Assert.Equal(firstLastNameValueObject, secondLastNameValueObject);
        Assert.True(firstLastNameValueObject.Equals(secondLastNameValueObject));
    }

    [Fact]
    public void GivenDifferentLastNames_WhenComparing_ThenShouldNotBeEqual()
    {
        var firstLastNameValueObject = LastNameValueObject.Factory("Silva");
        var secondLastNameValueObject = LastNameValueObject.Factory("Souza");

        Assert.NotEqual(firstLastNameValueObject, secondLastNameValueObject);
        Assert.False(firstLastNameValueObject.Equals(secondLastNameValueObject));
    }

    [Fact]
    public void GivenSameValues_WhenGettingHashCode_ThenShouldReturnSameHashCode()
    {
        var firstLastNameValueObject = LastNameValueObject.Factory("Pereira");
        var secondLastNameValueObject = LastNameValueObject.Factory("PEREIRA");

        Assert.Equal(firstLastNameValueObject.GetHashCode(), secondLastNameValueObject.GetHashCode());
    }

    [Fact]
    public void GivenInstance_WhenToStringCalled_ThenShouldReturnFormattedName()
    {
        var lastNameValueObject = LastNameValueObject.Factory("ferreira");

        Assert.Equal("Ferreira", lastNameValueObject.ToString());
    }
}
