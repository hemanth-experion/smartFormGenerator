using System;

namespace Experion.Marina.Core
{
    public enum Keys
    {
        UserId,
        User,
        UserSessionId,
        Message,
        UserLanguage,
        ApplicationSettings,
        LanguageResources,
        EmailConfigurations,
        SmsConfigurations,
        WebConfigSettings,
        UserSessionData
    }

    public interface IApplicationContext
    {
        void AddError(string error);

        void AddError(string error, string messageParam);

        void AddError(string error, string[] messageParams);

        void AddInformation(string info);

        void AddInformation(string info, string messageParam);

        void AddInformation(string info, string[] messageParams);

        void AddQuestion(string question);

        void AddQuestion(string question, string messageParam);

        void AddQuestion(string question, string[] messageParams);

        void AddWarning(string warning);

        void AddWarning(string warning, string messageParam);

        void AddWarning(string warning, string[] messageParams);

        void ClearContext();

        void ClearMessages();

        bool ContainsMessage(string messageKey);

        string GetBrowserName();

        string GetBrowserVersion();

        DateTime GetCurrentDateTime();

        System.Collections.Specialized.NameValueCollection GetCurrentRequestHeaders();

        string GetHostAddress();

        object GetMessages();

        object GetSharedObject(string objectKey);

        string GetUser();

        string GetUserId();

        object GetUserObject(string objectKey);

        string GetUserToken();

        void RemoveMessage(string messageKey);

        void SetSharedObject(string objectKey, object obj);

        void SetUserObject(string objectKey, object obj);
    }
}