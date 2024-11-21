using HomeBudget.Common.EntityDTOs.Budget;

namespace BudgetHero.App.Models.Extensions
{
    public static class BudgetExtension
    {
        public static void FromGetResponse(this Budget budget, BudgetGetResponseModel responseModel)
        {
            budget.Id = responseModel.Id;
            budget.OwnerId = responseModel.OwnerId;
            budget.Name = responseModel.Name;
            budget.Description = responseModel.Description;
            budget.IconUnicode = responseModel.IconUnicode;
        }

        public static Budget FromGetResponse(this BudgetGetResponseModel responseModel)
        {
            return new Budget()
            {
                Id = responseModel.Id,
                OwnerId = responseModel.OwnerId,
                Name = responseModel.Name,
                Description = responseModel.Description,
                IconUnicode = responseModel.IconUnicode
            };
        }

        public static BudgetCreateRequestModel ToCreateRequest(this Budget budget)
        {
            return new BudgetCreateRequestModel()
            {
                Name = budget.Name,
                Description = budget.Description,
                IconUnicode = budget.IconUnicode
            };
        }

        public static Budget FromCreateResponse(this BudgetCreateResponseModel responseModel)
        {
            return new Budget()
            {
                Id = responseModel.Id,
                OwnerId = responseModel.OwnerId,
                Name = responseModel.Name,
                IconUnicode = responseModel.IconUnicode,
                Description = responseModel.Description,
            };
        }

        public static void FromCreateResponse(this Budget budget, BudgetCreateResponseModel responseModel)
        {
            budget.Id = responseModel.Id;
            budget.OwnerId = responseModel.OwnerId;
            budget.Name = responseModel.Name;
            budget.Description = responseModel.Description;
            budget.IconUnicode = responseModel.IconUnicode;
        }

        public static void FromUpdateResponse(this Budget budget, BudgetCreateResponseModel responseModel)
        {
            budget.Id = responseModel.Id;
            budget.OwnerId = responseModel.OwnerId;
            budget.Name = responseModel.Name;
            budget.Description = responseModel.Description;
            budget.IconUnicode = responseModel.IconUnicode;
        }

        public static BudgetUpdateRequestModel ToUpdateRequest(this Budget budget)
        {
            return new BudgetUpdateRequestModel()
            {
                Name = budget.Name,
                Description = budget.Description,
                IconUnicode = budget.IconUnicode,
            };
        }

        public static bool IsCreateRequestValid(this BudgetCreateRequestModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.Name))
                return false;

            if (string.IsNullOrEmpty(requestModel.Description))
                return false;

            if (string.IsNullOrEmpty(requestModel.IconUnicode))
                return false;

            return true;
        }

        public static bool IsNullOrEmpty(this Budget budget)
        {
            if (budget == null)
            {
                return true;
            }

            if (budget.Id.Equals(string.Empty))
            {
                return true;
            }

            if (budget.OwnerId.Equals(string.Empty))
            {
                return true;
            }

            return false;
        }
    }
}
