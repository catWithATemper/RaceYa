using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using RaceYa.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.Auth))]
namespace RaceYa.Droid.Dependencies
{
    public class Auth : IAuth
    {
        public string GetCurrentUserId()
        {
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public bool IsAuthenticated()
        {
            return FirebaseAuth.Instance.CurrentUser != null;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                throw new Exception("There is no user record corresponding to this identifier");
            }
            catch (Exception e)
            {
                throw new Exception("There was an unknown error.");
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthUserCollisionException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("There was an unknown error.");
            }
        }
    }
}