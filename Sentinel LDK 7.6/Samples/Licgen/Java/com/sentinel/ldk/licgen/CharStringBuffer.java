package com.sentinel.ldk.licgen;

public final class CharStringBuffer {
    private String value = null;

    public CharStringBuffer()
    {
        value = null;
    };

    public CharStringBuffer(String value)
    {
        this.value = value;
    };

    public String get()
    {
        return value;
    };

    public void set(String value)
    {
        this.value = value;
    };
}
