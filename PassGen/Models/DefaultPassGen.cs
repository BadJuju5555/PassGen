using System;
using PassGen.Models;

namespace PassGen.Models;

public class DefaultPassGen : PassGenBase
{

    public override string Generate(int length, string allowedChars)
    {

        // Funktion um Fehler abzufangen
        if(string.IsNullOrEmpty(allowedChars))
        {
            throw new ArgumentException("Es müssen erlaubte Zeichen übergeben werden.", nameof(allowedChars));
        }

        char[] password = new char[length];
        for(int i = 0; i < length; i++)
        {
            int index = Random.Next(allowedChars.Length);
            password[i] = allowedChars[index];
        }
        return new string (password);
    }

    public override string Generate(int length, string allowedChars, string[] requiredGroups)
    {
        if(requiredGroups == null)
        {
            throw new ArgumentNullException(nameof(requiredGroups));
        }

        if(string.IsNullOrEmpty(allowedChars))
        {
            throw new ArgumentException("Es müssen erlaubte Zeichen übergeben werden.", nameof(allowedChars));
        }

        if(length < requiredGroups.Length)
        {
            throw new ArgumentException("Die Passwortlänge muss mindestens so groß sein wie die Anzahl der ausgewählten Zeichengruppen.", nameof(length));

        }

        char[] password = new char[length];
        int index = 0;

        // Für jede Pflicht-Zeichengruppe wird zunächst ein Zeichen ausgehwählt
        foreach (var group in requiredGroups)
        {
            if (string.IsNullOrEmpty(group))
            {
                continue;
            }

            int charIndex = Random.Next(group.Length);
            password[index++] = group[charIndex];
        }

        // Restliche Zeichen zufällig aus der Gesamtmenge auswählen
        for(; index < length; index++)
        {
            int charIndex = Random.Next(allowedChars.Length);
            password[index] = allowedChars[charIndex];
        }

        // Mische dass Array, um zu verhindern dass die Pflichtzeichen immer am Anfang stehen.
        Shuffle(password);
        return new string (password);   
    }

    // Mischt ein Array von Zeichen mithilfe des Fisher-Yates-Algorithmus.
    private void Shuffle(char[] array)
    {
        for(int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Next(i + 1);
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}

