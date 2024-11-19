using HomeBudget.Common.EntityDTOs.User;
using System.Text.RegularExpressions;

namespace BudgetHero.App.Models.Extensions
{
    public static class UserExtension
    {
        public static void FromGetResponse(this User user, UserGetResponseModel responseModel)
        {
            user.Id = responseModel.Id;
            user.FirstName = responseModel.FirstName;
            user.LastName = responseModel.LastName;
            user.IsAccountConfirmed = responseModel.IsAccountConfirmed;
            user.IsAccountSetup = responseModel.IsAccountSetup;
            user.Email = responseModel.Email;
            user.PhoneNumber = responseModel.PhoneNumber;
        }

        public static User FromGetResponse(this UserGetResponseModel responseModel)
        {
            return new User()
            {
                Id = responseModel.Id,
                FirstName = responseModel.FirstName,
                LastName = responseModel.LastName,
                IsAccountConfirmed = responseModel.IsAccountConfirmed,
                IsAccountSetup = responseModel.IsAccountSetup,
                Email = responseModel.Email,
                PhoneNumber = responseModel.PhoneNumber,
            };
        }

        public static void FromUpdateResponse(this User user, UserUpdateResponseModel responseModel)
        {
            user.Id = responseModel.Id;
            user.FirstName = responseModel.FirstName;
            user.LastName = responseModel.LastName;
            user.IsAccountConfirmed = responseModel.IsAccountConfirmed;
            user.IsAccountSetup = responseModel.IsAccountSetup;
            user.Email = responseModel.Email;
            user.PhoneNumber = responseModel.PhoneNumber;
        }

        public static UserUpdateRequestModel ToUpdateRequest(this User user)
        {
            return new UserUpdateRequestModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAccountConfirmed = user.IsAccountConfirmed,
                IsAccountSetup = user.IsAccountSetup,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static bool IsUpdateRequestValid(this UserUpdateRequestModel requestModel)
        {
            if (string.IsNullOrWhiteSpace(requestModel.FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(requestModel.LastName))
                return false;

            if (!IsValidPhoneNumber(requestModel.PhoneNumber))
                return false;

            return true;
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var phonePattern = @"^\d{7,15}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
    }
}
