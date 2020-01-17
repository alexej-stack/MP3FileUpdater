using System;
using System.IO;
using System.Threading.Tasks;

namespace Mp3Updater.DAL
{
    public interface IRepository
    {
        // int GetSession(Session session);
        //void SetData(Session session, FileInfo fileInfo);
        Session GetSessionData(int id);

       /* Task<int>*/void  SetFileInfo(int idSession, Mp3FileInfo fileInfo);

        Task<int> EndSession(DateTime endTime);
        Task<int> StartSession(DateTime endTime);
    }
}
