using Foody.Admin.Rest.Services;

namespace Foody.Admin.Manager;

public class DataManager : IDataManager
{
    private readonly IHttpsClientHandlerService _httpsClientHandler;

    public DataManager(IHttpsClientHandlerService httpsClientHandler)
    {
        _httpsClientHandler = httpsClientHandler;

        Category = new CategoryService(_httpsClientHandler);
        Inquiries = new InquiryService(_httpsClientHandler);
    }

    public CategoryService Category { get; private set; }

    public IInquiryService Inquiries { get; private set; }
}

