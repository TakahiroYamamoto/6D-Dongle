package com.sentinel.ldk.licgen;

public final class Int32Buffer {
    private int value = 0;

    public Int32Buffer()
    {
        value = 0;
    };

    public Int32Buffer(int value)
    {
        this.value = value;
    };

    public int get()
    {
        return value;
    };

    public void set(int value)
    {
        this.value = value;
    };
}
