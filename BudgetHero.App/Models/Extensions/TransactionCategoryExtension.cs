using HomeBudget.Common.EntityDTOs.TransactionCategory;

namespace BudgetHero.App.Models.Extensions
{
    public static class TransactionCategoryExtension
    {
        public static void FromGetResponse(this TransactionCategory transactionCategory, TransactionCategoryGetResponseModel responseModel)
        {
            transactionCategory.Id = responseModel.Id;
            transactionCategory.BudgetId = responseModel.BudgetId;
            transactionCategory.IconUnicode = responseModel.IconUnicode;
            transactionCategory.Name = responseModel.Name;
        }

        public static TransactionCategory FromGetResponse(this TransactionCategoryGetResponseModel responseModel)
        {
            return new TransactionCategory()
            {
                Id = responseModel.Id,
                BudgetId = responseModel.BudgetId,
                IconUnicode = responseModel.IconUnicode,
                Name = responseModel.Name,
            };
        }

        public static TransactionCategoryCreateRequestModel ToCreateRequest(this TransactionCategory transactionCategory)
        {
            return new TransactionCategoryCreateRequestModel()
            {
                Name = transactionCategory.Name,
                IconUnicode = transactionCategory.IconUnicode,
            };
        }

        public static TransactionCategory FromCreateResponse(this TransactionCategoryCreateResponseModel responseModel)
        {
            return new TransactionCategory()
            {
                Id = responseModel.Id,
                BudgetId = responseModel.BudgetId,
                Name = responseModel.Name,
                IconUnicode = responseModel.IconUnicode,
            };
        }

        public static void FromCreateResponse(this TransactionCategory transactionCategory, TransactionCategoryCreateResponseModel responseModel)
        {
            transactionCategory.Id = responseModel.Id;
            transactionCategory.BudgetId = responseModel.BudgetId;
            transactionCategory.Name = responseModel.Name;
            transactionCategory.IconUnicode = responseModel.IconUnicode;
        }

        public static void FromUpdateResponse(this TransactionCategory transactionCategory, TransactionCategoryUpdateResponseModel responseModel)
        {
            transactionCategory.Id = responseModel.Id;
            transactionCategory.BudgetId = responseModel.BudgetId;
            transactionCategory.Name = responseModel.Name;
            transactionCategory.IconUnicode = responseModel.IconUnicode;
        }

        public static TransactionCategoryUpdateRequestModel ToUpdateRequest(this TransactionCategory transactionCategory)
        {
            return new TransactionCategoryUpdateRequestModel()
            {
                Name = transactionCategory.Name,
                IconUnicode = transactionCategory.IconUnicode,
            };
        }

        public static bool IsRequestValid(this TransactionCategoryCreateRequestModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.Name))
                return false;

            if (string.IsNullOrEmpty(requestModel.IconUnicode))
                return false;

            return true;
        }

        public static bool IsRequestValid(this TransactionCategoryUpdateRequestModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.Name))
                return false;

            if (string.IsNullOrEmpty(requestModel.IconUnicode))
                return false;

            return true;
        }


        public static bool IsNullOrEmpty(this TransactionCategory transactionCategory)
        {
            if (transactionCategory == null)
                return true;

            if (string.IsNullOrEmpty(transactionCategory.Id))
                return true;

            if (string.IsNullOrEmpty(transactionCategory.BudgetId))
                return true;

            if (string.IsNullOrEmpty(transactionCategory.Name))
                return true;

            if (string.IsNullOrEmpty(transactionCategory.IconUnicode))
                return true;

            return false;
        }


    }
}
