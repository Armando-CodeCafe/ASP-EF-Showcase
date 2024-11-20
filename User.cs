using System;
using System.Security.Cryptography;
using System.Text;

namespace ASPLes;

// Definieer de User-klasse die een gebruiker in de applicatie vertegenwoordigt
public class User
{
    // Eigenschappen van de gebruiker
    public int Id { get; set; } // Unieke identificatie voor de gebruiker
    public string Email { get; set; } // E-mailadres van de gebruiker
    public string Password { get; set; } // Wachtwoord van de gebruiker
    public bool IsAdmin { get; set; } // Geef aan of de gebruiker een admin is

    public string Token { get; set; } // Token voor authenticatie

    // Constructor voor de User-klasse
    public User()
    {
        // Genereer een token door het e-mailadres en wachtwoord te hashen
        string toHash = Email + Password;
        Token = ComputeSha256Hash(toHash);
    }

    // Methode om een SHA256-hash van een string te berekenen
    static string ComputeSha256Hash(string rawData)
    {
        // Maak een SHA256-hashobject aan
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // Bereken de hash - retourneert een byte-array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Converteer de byte-array naar een hexadecimale string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // Voeg elke byte toe als hexadecimale string
            }
            return builder.ToString(); // Retourneer de resulterende hash
        }
    }
}