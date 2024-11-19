using BudgetHero.App.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Account;

namespace BudgetHero.App.Services
{
    public class TokenService : ITokenService
    {
        private const string TokenTypeKey = "tokenType";
        private const string RefreshTokenKey = "refreshToken";
        private const string AccessTokenKey = "accessToken";
        private const string ExpiresInKey = "expiresIn";


        private readonly ISecureStorage _secureStorage;

        public TokenService()
        {
            _secureStorage = SecureStorage.Default;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var accessToken = await _secureStorage.GetAsync(AccessTokenKey);
            return string.IsNullOrEmpty(accessToken) ? string.Empty : accessToken;
        }

        public async Task<int> GetExpiresInAsync()
        {
            var expiresIn = await _secureStorage.GetAsync(ExpiresInKey);
            return string.IsNullOrEmpty(expiresIn) ? 0 : int.Parse(expiresIn);
        }

        public async Task<string> GetRefreshTokenAsync()
        {
            var refreshToken = await _secureStorage.GetAsync(RefreshTokenKey);
            return string.IsNullOrEmpty(refreshToken) ? string.Empty : refreshToken;
        }

        public async Task<string> GetTokenTypeAsync()
        {
            var tokenType = await _secureStorage.GetAsync(TokenTypeKey);
            return string.IsNullOrEmpty(tokenType) ? string.Empty : tokenType;
        }

        public async Task RemoveTokensAsync()
        {
            await Task.Run(() =>
            {
                _secureStorage.Remove(TokenTypeKey);
                _secureStorage.Remove(AccessTokenKey);
                _secureStorage.Remove(RefreshTokenKey);
                _secureStorage.Remove(ExpiresInKey);
            });
        }

        public async Task SaveTokensAsync(LoginResponseModel responseModel)
        {
            await _secureStorage.SetAsync(TokenTypeKey, responseModel.TokenType);
            await _secureStorage.SetAsync(AccessTokenKey, responseModel.AccessToken);
            await _secureStorage.SetAsync(RefreshTokenKey, responseModel.RefreshToken);
            await _secureStorage.SetAsync(ExpiresInKey, responseModel.ExpiresIn.ToString());
        }
    }
}
