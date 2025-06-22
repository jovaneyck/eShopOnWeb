using System;

public enum DogType
{
    POINTER,
    RETRIEVER,
    LABRADOR
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
        switch (_type)
        {
            case DogType.POINTER:
                return GetBaseRunningSpeed();
            case DogType.RETRIEVER:
                return Math.Max(0, GetBaseRunningSpeed() - (GetBoneFactor() * _numberOfBones));
            case DogType.LABRADOR:
                return _isPottyTrained ? 0 : GetBaseSpeed(_litersOfWater);
            default:
                throw new ArgumentOutOfRangeException();
        }
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
        string value;
        switch (_type)
        {
            case DogType.POINTER:
                value = "Woof!";
                break;
            case DogType.RETRIEVER:
                value = "Waaf!";
                break;
            case DogType.LABRADOR:
                value = _litersOfWater > 0 ? "WOOF" : "-";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return value;
    }
}
