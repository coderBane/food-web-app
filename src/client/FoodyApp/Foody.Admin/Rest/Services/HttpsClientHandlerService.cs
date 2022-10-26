// ref: https://github.com/dotnet/maui-samples/blob/main/6.0/WebServices/TodoREST/TodoREST/Services/HttpsClientHandlerService.cs

using System.Net.Security;


namespace Foody.Admin.Rest.Services;

public class HttpsClientHandlerService : IHttpsClientHandlerService
{
    public void Initialize(out HttpClient client)
    {
        var handler = GetPlatformHandler();
        client = handler is not null ? new(handler) : null;
    }

    public HttpMessageHandler GetPlatformHandler()
    {

#if WINDOWS || MACCATALYST
        return null;

#elif ANDROID
        var handler = new CustomAndriodMessageHandler();
        handler.ServerCertificateCustomValidationCallback = (_, cert, _, errors) =>
        {
            if (cert is not null && cert.Issuer.Equals("CN=localhost"))
                return true;

            return errors == SslPolicyErrors.None;
        };
        return handler;

#elif IOS
        var handler = new NSUrlSessionHandler
        {
            TrustOverrideForUrl = IsHttpsLocalhost
        };
        return handler;
#else
        throw new PlatformNotSupportedException("Only Android, iOS, MacCatalyst, and Windows supported.");

#endif

    }

#if ANDROID

    internal sealed class CustomAndriodMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
    {
        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
                => new CustomHostnameVerifier();

        private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
        {
            public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session)
            {
                return Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session) ||
                        hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
            }
        }
    }

#elif IOS

    public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
    {
        if (url.StartsWith("https://localhost"))
            return true;
        return false;
    }

#endif
}

