using System.Text.RegularExpressions;
using qoqo.Services;
using Xunit;

namespace qoqo_test.unit_test;

public class RegexServiceTest
{
    
    [Theory]
    [InlineData("jean@gmail.com")]
    [InlineData("di.di.eu@ppppp.ccc")]
    [InlineData("a@a.a")]
    [InlineData("6666@abc.com")]
    public void ValidEmail(string value)
    {
        Assert.True(RegexService.CheckEmail(value));
    }
      
    [Theory]
    [InlineData("didier")]
    [InlineData("jeanA.gmail.com")]
    [InlineData("didier@didiercom")]
    [InlineData("66 66@6666.666")]
    public void NotValidEmail(string value)
    {
        Assert.False(RegexService.CheckEmail(value));
    }
    
    [Theory]
    [InlineData("Didier")]
    [InlineData("DansLeSouth")]
    [InlineData("jean______p")]
    [InlineData("101010100112222")]
    [InlineData("didier_didier_didier666")]
    [InlineData("____SansReroDelaMuerte____")]
    public void ValidUserName(string value)
    {
        Assert.True(RegexService.CheckUserName(value));
    }
    
    [Theory]
    [InlineData("dd")]
    [InlineData("1234567890123456789012345678901234567890")]
    [InlineData("didier@didiercom")]
    [InlineData("66 666666_666")]
    [InlineData("didier_didier.didier666")]
    [InlineData("sansrerorodéoèàa")]
    [InlineData("@abcdepfle.com")]
    public void NotValidUserName(string value)
    {
        Assert.False(RegexService.CheckUserName(value));
    }
    
    [Theory]
    [InlineData("Jean1234")]
    [InlineData("M9r-5baz@utB<qC[/?")]
    [InlineData("Ff6lo&e$lh!hADPj%EvMMoE4")]
    [InlineData("LeVandamDeLaMuerte$1")]
    [InlineData("LePoussinPiou6")]
    [InlineData("7777$$$$AbcdefPPP$____.....")]
    public void ValidPassword(string value)
    {
        Assert.True(RegexService.CheckPassword(value));
    }
    
    [Theory]
    [InlineData("didierJeanPascal")]
    [InlineData("jeanA.gmail.com")]
    [InlineData("Didier1")]
    [InlineData("didier1234")]
    [InlineData("p")]
    [InlineData("1234567890123456789012345678901234567890")]
    [InlineData("$$$$$$$$$$")]
    [InlineData("7777$$$$$____.....")]
    public void NotValidPassword(string value)
    {
        Assert.False(RegexService.CheckPassword(value));
    }

}