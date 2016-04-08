<Query Kind="Program" />

void Main()
{
    MyDouble? my = new MyDouble(1.0);
    Boolean compare = my == 0.0;
}

struct MyDouble
{
    Double? _value;

    public MyDouble(Double value)
    {
        _value = value;
    }

    public static implicit operator Double(MyDouble value)
    {
        if (value._value.HasValue)
        {
            return value._value.Value;
        }

        throw new InvalidCastException("MyDouble value cannot convert to System.Double: no value present.");
    }
}
