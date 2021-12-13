using System;

namespace job;

public class ModelFila
{
    public readonly string container;
    public readonly Guid id;

    public ModelFila(string container, Guid id)
    {
        this.container = container;
        this.id = id;
    }
}

// [1fc0b86aaada=>a7a6c491-974f-42d8-b802-ffe9b553ebe1]