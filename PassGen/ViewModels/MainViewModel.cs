using PassGen.Models;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PassGen.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly DefaultPassGen _base;


    //
    private int _passwordLength = 12;
    private bool _includeUppercase = true;
    private bool _includeLowercase = true;
    private bool _includeDigits = true;
    private bool _includeSpecial = true;
    private bool _ensureAllSelectedCategories = true;
    private string _generatePassword;



    // Konstruktor
    public MainViewModel()
    {
        _base = new DefaultPassGen();

        GeneratePasswordCommand = new RelayCommand(GeneratePassword);
    }

    //
    public int PasswordLength
    {
        get => _passwordLength;
        set => SetProperty(ref _passwordLength, value);
    }

    public bool IncludeUppercase
    {
        get => _includeUppercase;
        set => SetProperty( ref _includeUppercase, value);
    }

    public bool IncludeLowercase
    {
        get => _includeLowercase;
        set => SetProperty( ref _includeLowercase, value);
    }

    public bool IncludeDigits
    {
        get => _includeDigits;
        set => SetProperty(ref _includeDigits, value);
    }

    public bool IncludeSpecial
    {
        get => _includeSpecial;
        set => SetProperty(ref _includeSpecial,  value);
    }

    public bool EnsureAllSelectedCategories
    {
        get => _ensureAllSelectedCategories;
        set => SetProperty(ref _ensureAllSelectedCategories, value);
    }

    public string GeneratedPassword
    {
        get => _generatePassword;
        set => SetProperty(ref _generatePassword, value);
    }

    // Commands
    public ICommand GeneratePasswordCommand { get; }

    // Wird durch den Command zum Generieren des Passworts aufgerufen
    private void GeneratePassword(object parameter)
    {
        // Validierung der Passwortlänge
        if(PasswordLength <= 0)
        {
            MessageBox.Show("Bitte geben sie eine gültige, positive Zahl für die Passwortlänge ein.",
                "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Definition der Zeichengruppen
        const string uppercase = "ABCDEFGHIJKLMNOPRSTUVWXYZ";
        const string lowercase = "abcdefghijklmnoprstuvwxyz";
        const string digits = "0123456789";
        const string special = "!@#$%^&*()_+-=[]{}|;:,.<>/?";

        StringBuilder allowedChars = new StringBuilder();
        var requiredGroups = new List<string>();

        if(IncludeUppercase)
        {
           allowedChars.Append(uppercase);
           requiredGroups.Add(uppercase);
        }

        if(IncludeLowercase)
        {
            allowedChars.Append(lowercase);
            requiredGroups.Add(lowercase);
        }

        if(IncludeDigits)
        {
            allowedChars.Append(digits);
            requiredGroups.Add(digits);
        }

        if(IncludeSpecial)
        {
            allowedChars.Append(special);
            requiredGroups.Add(special);
        }

        if(allowedChars.Length == 0)
        {
            MessageBox.Show("Bitte wählen sie mindestens eine Zeichengruppe aus.",
                "Keine Zeichengruppe ausgewählt", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Falls sichergestellt werden soll, dass alle ausgewählten Kategorien vertreten sind,
        // prüfen wir, ob die Passwortlänge ausreichend ist.

        if(EnsureAllSelectedCategories && PasswordLength < requiredGroups.Count)
        {
            MessageBox.Show("Die Passwortlänge muss mindestens so groß sein wie die Anzahl der ausgewählten Zeichengruppen.",
                    "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if(EnsureAllSelectedCategories)
            {
                GeneratedPassword = _base.Generate(PasswordLength, allowedChars.ToString(), requiredGroups.ToArray());

            }
            else
            {
                GeneratedPassword = _base.Generate(PasswordLength, allowedChars.ToString());
            }
        }
        catch(System.Exception ex)
        {
            MessageBox.Show("Fehler beim Generieren des Passworts: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }  
}

