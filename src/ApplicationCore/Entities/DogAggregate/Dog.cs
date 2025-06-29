using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities.DogAggregate;

public enum DogType
{
    Pointer,
    Retriever,
    Labrador
}

public class Dog
{
    private readonly bool _isPottyTrained;
    private readonly int _numberOfBones;
    private readonly DogType _type;
    private readonly double _litersOfWater;

    public Dog(DogType type, int numberOfBones, double litersOfWater, bool isPottyTrained)
    {
        _type = type;
        _numberOfBones = numberOfBones;
        _litersOfWater = litersOfWater;
        _isPottyTrained = isPottyTrained;
    }

    public double GetRunningSpeed()
    {
        return _type switch
        {
            DogType.Pointer => GetBaseRunningSpeed(),
            DogType.Retriever => Math.Max(0, GetBaseRunningSpeed() - (GetBoneFactor() * _numberOfBones)),
            DogType.Labrador => _isPottyTrained ? 0 : GetBaseSpeed(_litersOfWater),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private double GetBaseSpeed(double litersOfWater)
    {
        return Math.Min(24.0, litersOfWater * GetBaseRunningSpeed());
    }

    private double GetBoneFactor()
    {
        return 9.0;
    }

    private double GetBaseRunningSpeed()
    {
        return 12.0;
    }

    public string GetBark()
    {
        string value = _type switch
        {
            DogType.Pointer => "Woof!",
            DogType.Retriever => "Waaf!",
            DogType.Labrador => _litersOfWater > 0 ? "WOOF" : "-",
            _ => throw new ArgumentOutOfRangeException()
        };

        return value;
    }
}
