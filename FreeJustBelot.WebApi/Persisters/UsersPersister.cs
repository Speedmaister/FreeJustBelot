using FreeJustBelot.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FreeJustBelot.WebApi.Persisters
{
    public class UsersPersister
    {
        private const string UsernameLetters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_.";
        private const string NicknameLetters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890_.- ";
        private const string SessionKeyGenerationLetters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        private const string SessionKeyLetters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
        private const int NameMaxLength = 30;
        private const int NameMinLength = 6;
        private const int SessionKeyLength = 50;
        private const int AuthCodeLength = 34;

        private static Random rand = new Random();

        public static string GenerateSessionKey(int userId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(userId);
            while (sb.Length < SessionKeyLength)
            {
                int index = rand.Next(SessionKeyGenerationLetters.Length);
                sb.Append(SessionKeyGenerationLetters[index]);
            }

            return sb.ToString();
        }

        public static bool ValidateSessionKey(string sessionKey)
        {
            if (sessionKey == null && sessionKey.Length != SessionKeyLength && sessionKey.Any(x => !SessionKeyLetters.Contains(x)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ValidateUserRegistrationModel(UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Null model sent.");
            }

            if (model.Username == null)
            {
                throw new ArgumentNullException("Username is null.");
            }

            if (model.Username.Length < NameMinLength || model.Username.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException("Username length has to be exactly between 6 and 30 characters.");
            }

            if (model.AuthCode == null)
            {
                throw new ArgumentNullException("Null authcode.");
            }

            if (model.AuthCode.Length != AuthCodeLength)
            {
                throw new ArgumentOutOfRangeException("Auth code has to be exactly 40 characters.");
            }

            if (model.Nickname == null)
            {
                throw new ArgumentNullException("Null nickname.");
            }

            if (model.Nickname.Length < NameMinLength || model.Nickname.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException("Name length hast to be exactly between 6 and 30 characters.");
            }

            if (model.Username.Any(x => !UsernameLetters.Contains(x)))
            {
                throw new FormatException("Illegal letters in username.");
            }

            if (model.Nickname.Any(x => !NicknameLetters.Contains(x)))
            {
                throw new FormatException("Illegal letters in nickname.");
            }
        }

        public static void ValidateUserLoginModel(UserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Null model sent.");
            }

            if (model.Username == null)
            {
                throw new ArgumentNullException("Username is null.");
            }

            if (model.Username.Length < NameMinLength || model.Username.Length > NameMaxLength)
            {
                throw new ArgumentOutOfRangeException("Username length has to be exactly between 6 and 30 characters.");
            }

            if (model.AuthCode == null)
            {
                throw new ArgumentNullException("Null authcode.");
            }

            if (model.AuthCode.Length != AuthCodeLength)
            {
                throw new ArgumentOutOfRangeException("Auth code has to be exactly 40 characters.");
            }

            if (model.Username.Any(x => !UsernameLetters.Contains(x)))
            {
                throw new FormatException("Illegal letters in username.");
            }
        }
    }
}