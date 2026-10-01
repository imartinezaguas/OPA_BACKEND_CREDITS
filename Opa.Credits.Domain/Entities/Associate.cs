using System;

namespace Opa.Credits.Domain.Entities;

public class Associate
{
    public int Id { get; private set; }
    public string Identification { get; private set; }
    public string Name { get; private set; }

    // Navegación hacia los créditos
    public virtual System.Collections.Generic.ICollection<Credit> Credits { get; private set; } = new System.Collections.Generic.List<Credit>();

    // Constructor vacío requerido por Entity Framework Core
    protected Associate() { }

    public Associate(string identification, string name)
    {
        Identification = identification ?? throw new ArgumentNullException(nameof(identification));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("El nombre no puede estar vacío.");
        
        Name = newName;
    }
}
