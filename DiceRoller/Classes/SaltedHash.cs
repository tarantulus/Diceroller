﻿using System;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

public class SaltedHash
{
    public string Hash { get;  set; }
    public string Salt { get;  set; }

    public SaltedHash()
    {
    }

    public SaltedHash(string password)
    {
        var saltBytes = new byte[32];
        using (var provider = RandomNumberGenerator.Create())
        {
            provider.GetNonZeroBytes(saltBytes);
        }
        Salt = Convert.ToBase64String(saltBytes);
        var passwordAndSaltBytes = Concat(password, saltBytes);
        Hash = ComputeHash(passwordAndSaltBytes);
    }

    static string ComputeHash(byte[] bytes)
    {
        using (var sha256 = SHA256.Create())
        {
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
    }

    static byte[] Concat(string password, byte[] saltBytes)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        return passwordBytes.Concat(saltBytes).ToArray();
    }

    public bool Verify(string salt, string hash, string password)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var passwordAndSaltBytes = Concat(password, saltBytes);
        var hashAttempt = ComputeHash(passwordAndSaltBytes);
        return hash == hashAttempt;
    }
}