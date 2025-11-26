using tMoney.Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects;

public class IdValueObjectTest
{
    [Fact]
    public void GivenValidGuid_WhenFactoryCalled_ThenShouldCreateInstance()
    {
        var id = Guid.NewGuid();

        var idValueObject = IdValueObject.Factory(id);

        Assert.NotNull(idValueObject);
        Assert.Equal(id, idValueObject.Id);
    }

    [Fact]
    public void GivenEmptyGuid_WhenFactoryCalled_ThenShouldThrowArgumentException()
    {
        var emptyId = Guid.Empty;

        Assert.Throws<ArgumentException>(() => IdValueObject.Factory(emptyId));
    }

    [Fact]
    public void GivenNewCall_WhenCalled_ThenShouldCreateInstanceWithNonEmptyGuid()
    {
        var idValueObject = IdValueObject.New();

        Assert.NotNull(idValueObject);
        Assert.NotEqual(Guid.Empty, idValueObject.Id);
    }

    [Fact]
    public void GivenSameGuid_WhenComparing_ThenShouldBeEqual()
    {
        var id = Guid.NewGuid();
        var firstIdValueObject = IdValueObject.Factory(id);
        var secondIdValueObject = IdValueObject.Factory(id);

        Assert.Equal(firstIdValueObject, secondIdValueObject);
        Assert.True(firstIdValueObject.Equals(secondIdValueObject));
    }

    [Fact]
    public void GivenDifferentGuids_WhenComparing_ThenShouldNotBeEqual()
    {
        var firstIdValueObject = IdValueObject.New();
        var secondIdValueObject = IdValueObject.New();

        Assert.NotEqual(firstIdValueObject, secondIdValueObject);
        Assert.False(firstIdValueObject.Equals(secondIdValueObject));
    }

    [Fact]
    public void GivenSameGuid_WhenGettingHashCode_ThenShouldReturnSameHashCode()
    {
        var id = Guid.NewGuid();
        var firstIdValueObject = IdValueObject.Factory(id);
        var secondIdValueObject = IdValueObject.Factory(id);

        Assert.Equal(firstIdValueObject.GetHashCode(), secondIdValueObject.GetHashCode());
    }

    [Fact]
    public void GivenInstance_WhenToStringCalled_ThenShouldReturnGuidString()
    {
        var id = Guid.NewGuid();
        var idValueObject = IdValueObject.Factory(id);

        var result = idValueObject.ToString();

        Assert.Equal(id.ToString(), result);
    }
}
