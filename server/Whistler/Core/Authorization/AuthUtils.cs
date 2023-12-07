using System;

namespace Whistler.Core.Authorization
{
    public static class AuthUtils
    {
        private const string AllowedNonAlphaNum = "!@#$%^&*()_-+=[{]};:<>|./?";
        private const string AllowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        
        public static string GenerateRandomPassword(int length, int nonAlphaNumericChars)
        {
            var rd = new Random();

            if (nonAlphaNumericChars > length || length <= 0 || nonAlphaNumericChars < 0)
                throw new ArgumentOutOfRangeException();

            var pass = new char[length];
            var pos = new int[length];
            int i = 0, j = 0, temp = 0;
            var flag = false;

            //Random the position values of the pos array for the string Pass
            while (i < length - 1)
            {
                j = 0;
                flag = false;
                temp = rd.Next(0, length);
                for (j = 0; j < length; j++)
                    if (temp == pos[j])
                    {
                        flag = true;
                        j = length;
                    }

                if (!flag)
                {
                    pos[i] = temp;
                    i++;
                }
            }

            //Random the AlphaNumericChars
            for (i = 0; i < length - nonAlphaNumericChars; i++)
                pass[i] = AllowedChars[rd.Next(0, AllowedChars.Length)];

            //Random the NonAlphaNumericChars
            for (i = length - nonAlphaNumericChars; i < length; i++)
                pass[i] = AllowedNonAlphaNum[rd.Next(0, AllowedNonAlphaNum.Length)];

            //Set the sorted array values by the pos array for the rigth posistion
            var sorted = new char[length];
            for (i = 0; i < length; i++)
                sorted[i] = pass[pos[i]];

            var generatedPassword = new string(sorted);

            return generatedPassword;
        }
    }
}