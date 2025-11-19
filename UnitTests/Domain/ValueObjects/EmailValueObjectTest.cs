using tMoney.Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects;

public class EmailValueObjectTest
{
    [Fact]
    public void GivenValidEmail_WhenCreating_ThenShouldCreateSuccessfully()
    {
        // Arrange
        var rawEmail = "arthur@email.com.br";

        // Act
        var email = EmailValueObject.Factory(rawEmail);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(rawEmail, email.Email);
    }

    [Fact]
    public void GivenUpperCaseEmail_WhenCreating_ThenShouldNormalizeToLowerCase()
    {
        var rawEmail = "ARTHUR@EMAIL.COM.BR";

        var email = EmailValueObject.Factory(rawEmail);

        Assert.Equal(rawEmail.ToLower(), email.Email);
    }

    [Theory]
    [MemberData(nameof(GetInvalidEmails))]
    public void GivenInvalidEmail_WhenCreating_ThenShouldThrowArgumentException(string email)
    {
        Assert.ThrowsAny<ArgumentException>(() => EmailValueObject.Factory(email));
    }

    [Fact]
    public void GivenSameEmailValues_WhenComparing_ThenShouldBeEqual()
    {
        var email1 = EmailValueObject.Factory("arthur@email.com");
        var email2 = EmailValueObject.Factory("arthur@email.com");
        var email3 = EmailValueObject.Factory("arthur.email@email.com");

        Assert.Equal(email1, email2);
        Assert.True(email1.Equals(email2));
        Assert.True(email1 == email2);
        Assert.False(email1 == email3);
        Assert.False(email1 != email2);
        Assert.False(email1.Equals(email3));
    }

    [Fact]
    public void GivenSameEmailValues_WhenGettingHashCode_ThenShouldBeSame()
    {
        var email1 = EmailValueObject.Factory("teste@tmoney.com");
        var email2 = EmailValueObject.Factory("teste@tmoney.com");

        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
    }

    public static IEnumerable<object[]> GetInvalidEmails()
    {
        yield return new object[] { "" };
        yield return new object[] { " " };
        yield return new object[] { "invalid-email" };
        yield return new object[] { "invalid-email@email" };
        yield return new object[] { "invalid-email@email" };
        yield return new object[] { "invalid-email.com" };
        yield return new object[] { "invalid-email@.com" };
        yield return new object[] { "invalid-email@email." };
        yield return new object[] { "@sem-usuario.com" };
        yield return new object[] { "super.usuario.extremamente.grande.completo.nome.personalizado.unico.incrivelmente.extenso.para.testes.de.sistemas.que.precisam.validar.limites.de.caracteres.em.enderecos.de.email.muito.acima.do.normal@um.dominio.super.complexo.e.exageradamente.extenso.para.validacao.tecnica.de.aplicacoes.com.br" };
    }
}
