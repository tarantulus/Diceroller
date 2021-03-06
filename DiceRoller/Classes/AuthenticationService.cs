﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DiceRoller.DataLayer;

namespace DiceRoller.Classes
{
    public class AuthenticationService
    {
        JsonStore _database = new JsonStore("users");
        public AuthenticationService()
        {
        }
        public RegisterResult RegisterUser(User user, string password)
        {
            user.SaltedHash = new SaltedHash(password);
            var data = GetAllUsers();
            if (data.Any(u => u.Name == user.Name))
            {
                return RegisterResult.UserExists;
            }
            if (data.Any(u => u.Email == user.Email))
            {
                return RegisterResult.EmailExists;
            }
            data.Add(user);
            _database.Update<UserCollection>(data);
            return RegisterResult.Success;
            
        }

        public enum RegisterResult
        {
            UserExists,
            EmailExists,
            Success
        };

        public string Hash(string salt, string data)
        {
            return null;
        }

        public bool AuthenticateUser(string username, string password)
        {
            var user = GetUserByName(username);
            if (user!= null && user.SaltedHash.Verify(user.SaltedHash.Salt, user.SaltedHash.Hash, password))
            {
                return true;
            }
            return false;
        }
        private UserCollection GetAllUsers()
        {
            var data = _database.GetData<UserCollection>() as UserCollection;
            return data ?? new UserCollection();
        }

        public User GetUserByName(string userName)
        {
            var allUsers = _database.GetData<UserCollection>() as UserCollection;
            if (allUsers != null)
            {
                return allUsers.FirstOrDefault(u => u.Name == userName);
            }
            return null;
        }

        public void UpdateUser(User user)
        {
            var data = GetAllUsers();
            data.Remove(data.First(u => u.Name == user.Name));
            data.Add(user);
            _database.Update<UserCollection>(user);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

    
    }
}