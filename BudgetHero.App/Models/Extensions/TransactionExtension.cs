using HomeBudget.Common.EntityDTOs.Transaction;

namespace BudgetHero.App.Models.Extensions
{
    public static class TransactionExtension
    {
        public static void FromGetResponse(this Transaction transaction, TransactionGetResponseModel responseModel)
        {
            transaction.Id = responseModel.Id;
            transaction.BudgetId = responseModel.BudgetId;
            transaction.TransactionCategoryId = responseModel.TransactionCategoryId;
            transaction.CreatorId = responseModel.CreatorId;
            transaction.Name = responseModel.Name;
            transaction.Description = responseModel.Description;
            transaction.TotalAmount = responseModel.TotalAmount;
            transaction.Date = responseModel.Date;
        }

        public static Transaction FromGetResponse(this TransactionGetResponseModel responseModel)
        {
            return new Transaction()
            {
                Id = responseModel.Id,
                TransactionCategoryId = responseModel.TransactionCategoryId,
                CreatorId = responseModel.CreatorId,
                BudgetId = responseModel.BudgetId,
                Name = responseModel.Name,
                Description = responseModel.Description,
                TotalAmount = responseModel.TotalAmount,
                Date = responseModel.Date,
            };
        }

        public static TransactionCreateRequestModel ToCreateRequest(this Transaction transaction)
        {
            return new TransactionCreateRequestModel()
            {
                TransactionCategoryId = transaction.TransactionCategoryId,
                CreatorId = transaction.CreatorId,
                BudgetId = transaction.BudgetId,
                Name = transaction.Name,
                Description = transaction.Description,
                TotalAmount = transaction.TotalAmount,
                Date = transaction.Date,
            };
        }

        public static Transaction FromCreateResponse(this TransactionCreateResponseModel responseModel)
        {
            return new Transaction()
            {
                Id = responseModel.Id,
                TransactionCategoryId = responseModel.TransactionCategoryId,
                CreatorId = responseModel.CreatorId,
                BudgetId = responseModel.BudgetId,
                Name = responseModel.Name,
                Description = responseModel.Description,
                TotalAmount = responseModel.TotalAmount,
                Date = responseModel.Date,
            };
        }

        public static void FromUpdateResponse(this Transaction transaction, TransactionUpdateResponseModel responseModel)
        {
            transaction.Id = responseModel.Id;
            transaction.BudgetId = responseModel.BudgetId;
            transaction.TransactionCategoryId = responseModel.TransactionCategoryId;
            transaction.CreatorId = responseModel.CreatorId;
            transaction.Name = responseModel.Name;
            transaction.Description = responseModel.Description;
            transaction.TotalAmount = responseModel.TotalAmount;
            transaction.Date = responseModel.Date;
        }

        public static TransactionUpdateRequestModel ToUpdateRequest(this Transaction transaction)
        {
            return new TransactionUpdateRequestModel()
            {
                TransactionCategoryId = transaction.TransactionCategoryId,
                CreatorId = transaction.CreatorId,
                BudgetId = transaction.BudgetId,
                Name = transaction.Name,
                Description = transaction.Description,
                TotalAmount = transaction.TotalAmount,
                Date = transaction.Date,
            };
        }

        public static ValidationResult IsRequestValid(this TransactionCreateRequestModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.BudgetId))
                return new ValidationResult(false, "BudgetId cannot be null or empty.");

            if (string.IsNullOrEmpty(requestModel.CreatorId))
                return new ValidationResult(false, "CreatorId cannot be null or empty.");

            if (string.IsNullOrEmpty(requestModel.TransactionCategoryId))
                return new ValidationResult(false, "TransactionCategoryId cannot be null or empty.");

            if (string.IsNullOrEmpty(requestModel.Name))
                return new ValidationResult(false, "Name cannot be null or empty.");

            if (requestModel.TotalAmount == 0)
                return new ValidationResult(false, "TotalAmount cannot be zero.");

            return new ValidationResult(true, string.Empty);
        }

        //public static bool IsRequestValid(this TransactionUpdateRequestModel requestModel)
        //{

        //}

        public static bool IsNullOrEmpty(this Transaction transaction)
        {
            if (transaction == null)
                return true;

            if (string.IsNullOrEmpty(transaction.Id))
                return true;

            if (string.IsNullOrEmpty(transaction.BudgetId))
                return true;

            if (string.IsNullOrEmpty(transaction.CreatorId))
                return true;

            if (string.IsNullOrEmpty(transaction.TransactionCategoryId))
                return true;

            if (string.IsNullOrEmpty(transaction.Name))
                return true;

            return false;
        }
    }
}
