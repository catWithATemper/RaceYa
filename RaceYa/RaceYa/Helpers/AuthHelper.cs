﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceYa.Helpers
{
    public interface IAuth
    {
        Task<bool> RegisterUser(string email, string password);
        Task<bool> LoginUser(string email, string password);
        bool IsAuthenticated();
        string GetCurrentUserId();
        bool LogOutUser();
    }

    public class Auth
    {
        private static IAuth auth = DependencyService.Get<IAuth>();

        public static async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                return await auth.RegisterUser(email, password);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
                return false;
            }
        }

        public static async Task<bool> LogInUser(string email, string password)
        {
            try
            {
                // if user does not exist, register
                return await auth.LoginUser(email, password);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Ok");
                string registerMessage = "There is no user record corresponding to this identifier";
                if (e.Message.Contains(registerMessage))
                    return await RegisterUser(email, password);
                return false;
            }
        }

        public static bool IsAuthenticated()
        {
            return auth.IsAuthenticated();
        }

        public static string GetCurrentUserId()
        {
            return auth.GetCurrentUserId();
        }

        public static bool LogOutUser()
        {
            return auth.LogOutUser();
        }
    }
}
