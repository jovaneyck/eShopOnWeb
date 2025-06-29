using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.DogAggregate;

public enum DogType
{
    Pointer,
    Retriever,
    Labrador,
    Pekinese
}

public abstract class Dog
{
    public static Dog Create(DogType type, int numberOfBones, double litersOfWater, bool isPottyTrained, int amountOfFood = 0, int unused = 0)
    {
        return type switch
        {
            DogType.Pointer => new PointerDog(),
            DogType.Retriever => new RetrieverDog(numberOfBones),
            DogType.Labrador => new LabradorDog(litersOfWater, isPottyTrained),
            DogType.Pekinese => new PekineseDog(amountOfFood),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public abstract double GetRunningSpeed();
    public abstract string GetBark();

    protected double GetBaseRunningSpeed()
    {
        return 12.0;
    }
}

public class PointerDog : Dog
{
    public override double GetRunningSpeed()
    {
        return GetBaseRunningSpeed();
    }

    public override string GetBark()
    {
        return "Woof!";
    }
}

public class RetrieverDog : Dog
{
    private readonly int _numberOfBones;

    public RetrieverDog(int numberOfBones)
    {
        _numberOfBones = numberOfBones;
    }

    public override double GetRunningSpeed()
    {
        return Math.Max(0, GetBaseRunningSpeed() - (GetBoneFactor() * _numberOfBones));
    }

    public override string GetBark()
    {
        return "Waaf!";
    }

    private double GetBoneFactor()
    {
        return 9.0;
    }
}

public class LabradorDog : Dog
{
    private readonly bool _isPottyTrained;
    private readonly double _litersOfWater;

    public LabradorDog(double litersOfWater, bool isPottyTrained)
    {
        _litersOfWater = litersOfWater;
        _isPottyTrained = isPottyTrained;
    }

    public override double GetRunningSpeed()
    {
        return _isPottyTrained ? 0 : GetBaseSpeed(_litersOfWater);
    }

    public override string GetBark()
    {
        return _litersOfWater > 0 ? "WOOF" : "-";
    }

    private double GetBaseSpeed(double litersOfWater)
    {
        return Math.Min(24.0, litersOfWater * GetBaseRunningSpeed());
    }
}

public class PekineseDog : Dog
{
    private readonly int _amountOfFood;

    public PekineseDog(int amountOfFood)
    {
        _amountOfFood = amountOfFood;
    }

    public override double GetRunningSpeed()
    {
        return GetBaseRunningSpeed();
    }

    public override string GetBark()
    {
        return _amountOfFood < 100 ? "HUNGRY" : "OMNOM";
    }
}
