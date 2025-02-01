using System;

namespace PassGen.Models;

// Abstrakte Basisklasse für den Passwortgenerator
public abstract class PassGenBase
{
    protected static readonly Random Random = new Random();

    // Generiert ein Passwort mit der angegebenen Länge und den erlaubten Zeichen.

    public abstract string Generate(int length, string allowedChars);

    // Generiert ein Passwort, bei dem sichergestellt wird, dass in jedem generierten Passwort
    // mindestens ein Zeichen aus jeder Pflichtgruppe enthalten ist.

    public abstract string Generate(int length, string allowedChars, string[] requiredGroups);

}
