using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.DogTests;

public class DogTest
{
    [Fact]
    public void GetRunningSpeedLabrador_PottyTrained()
    {
        var dog = new Dog(DogType.LABRADOR, 0, 0, true);
        Assert.Equal(0.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedLabrador_PottyTrained_With_Water()
    {
        var dog = new Dog(DogType.LABRADOR, 0, 1.5, true);
        Assert.Equal(0.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedLabrador_Not_PottyTrained()
    {
        var dog = new Dog(DogType.LABRADOR, 0, 1.5, false);
        Assert.Equal(18.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedLabrador_Not_PottyTrained_High_Water()
    {
        var dog = new Dog(DogType.LABRADOR, 0, 4, false);
        Assert.Equal(24.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningOfRetriever_With_No_Bones()
    {
        var dog = new Dog(DogType.RETRIEVER, 0, 0, false);
        Assert.Equal(12.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningOfRetriever_With_One_Bone()
    {
        var dog = new Dog(DogType.RETRIEVER, 1, 0, false);
        Assert.Equal(3.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningOfRetriever_With_Two_Bone()
    {
        var dog = new Dog(DogType.RETRIEVER, 2, 0, false);
        Assert.Equal(0.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedOfPointer()
    {
        var dog = new Dog(DogType.POINTER, 0, 0, false);
        Assert.Equal(12.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetBarkOfPointer()
    {
        var dog = new Dog(DogType.POINTER, 0, 0, false);
        Assert.Equal("Woof!", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfRetriever()
    {
        var dog = new Dog(DogType.RETRIEVER, 2, 0, false);
        Assert.Equal("Waaf!", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfLabrador_High_Water()
    {
        var dog = new Dog(DogType.LABRADOR, 0, 4, false);
        Assert.Equal("WOOF", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfLabrador_No_Water()
    {
        var dog = new Dog(DogType.LABRADOR, 0, 0, false);
        Assert.Equal("-", dog.GetBark());
    }
}
