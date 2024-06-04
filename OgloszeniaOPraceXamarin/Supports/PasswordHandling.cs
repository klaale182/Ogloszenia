using System;
using System.Security.Cryptography;
using System.Text;

namespace OgloszeniaOPraceXamarin.Supports {
    public static class PasswordHandling {
        public static string HashPassword(string password) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public static bool VerifyPassword(string enteredPassword, string hashedPassword) {
            return hashedPassword == HashPassword(enteredPassword);
        }
    }
}
