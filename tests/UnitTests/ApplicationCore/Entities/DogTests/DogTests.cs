using Microsoft.eShopWeb.ApplicationCore.Entities.DogAggregate;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Entities.DogTests;

public class DogTest
{
    [Fact]
    public void GetRunningSpeedLabrador_PottyTrained()
    {
        var dog = Dog.Create(DogType.Labrador, 0, 0, true);
        Assert.Equal(0.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedLabrador_PottyTrained_With_Water()
    {
        var dog = Dog.Create(DogType.Labrador, 0, 1.5, true);
        Assert.Equal(0.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedLabrador_Not_PottyTrained()
    {
        var dog = Dog.Create(DogType.Labrador, 0, 1.5, false);
        Assert.Equal(18.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedLabrador_Not_PottyTrained_High_Water()
    {
        var dog = Dog.Create(DogType.Labrador, 0, 4, false);
        Assert.Equal(24.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningOfRetriever_With_No_Bones()
    {
        var dog = Dog.Create(DogType.Retriever, 0, 0, false);
        Assert.Equal(12.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningOfRetriever_With_One_Bone()
    {
        var dog = Dog.Create(DogType.Retriever, 1, 0, false);
        Assert.Equal(3.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningOfRetriever_With_Two_Bone()
    {
        var dog = Dog.Create(DogType.Retriever, 2, 0, false);
        Assert.Equal(0.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetRunningSpeedOfPointer()
    {
        var dog = Dog.Create(DogType.Pointer, 0, 0, false);
        Assert.Equal(12.0, dog.GetRunningSpeed());
    }

    [Fact]
    public void GetBarkOfPointer()
    {
        var dog = Dog.Create(DogType.Pointer, 0, 0, false);
        Assert.Equal("Woof!", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfRetriever()
    {
        var dog = Dog.Create(DogType.Retriever, 2, 0, false);
        Assert.Equal("Waaf!", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfLabrador_High_Water()
    {
        var dog = Dog.Create(DogType.Labrador, 0, 4, false);
        Assert.Equal("WOOF", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfLabrador_No_Water()
    {
        var dog = Dog.Create(DogType.Labrador, 0, 0, false);
        Assert.Equal("-", dog.GetBark());
    }
    
    [Fact]
    public void GetBarkOfPekinese_No_Food()
    {
        var amountOfFood = 99;
        var dog = Dog.Create(DogType.Pekinese, 0, 0, false, amountOfFood);
        Assert.Equal("HUNGRY", dog.GetBark());
    }


    [Fact]
    public void GetBarkOfPekinese_Just_Enough_Food()
    {
        var amountOfFood = 100;
        var dog = Dog.Create(DogType.Pekinese, 0, 0, false, amountOfFood);
        Assert.Equal("OMNOM", dog.GetBark());
    }

    [Fact]
    public void GetBarkOfPekinese_Plenty_Of_Food()
    {
        var amountOfFood = 101;
        var dog = Dog.Create(DogType.Pekinese, 0, 0, false, amountOfFood);
        Assert.Equal("OMNOM", dog.GetBark());
    }

    [Fact]
    public void GetRunningSpeedOfPekinese()
    {
        var dog = Dog.Create(DogType.Pekinese, 0, 0, false, 0, 0);
        Assert.Equal(12.0, dog.GetRunningSpeed());
    }
}
