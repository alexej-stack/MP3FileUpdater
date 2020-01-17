using MP3FileUpdater.Core;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mp3Updater.DAL.Memory
{
   
    public class MemoryRepository : IRepository
    {
        //public SQLDatabaseContext _memoryContext = new SQLDatabaseContext();
        private List<Session> sessions = new List<Session>();
        
        public Task<int> EndSession(DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public Session GetSessionData(int id)
        {

            var session = sessions[id];
            return session;
        }

        public void SetFileInfo(int idSession, Mp3FileInfo fileInfo)
        {
            var session = GetSessionData(idSession);
            session.ReadedFilesCount = 0;
            session.ReamainingFilesCount = 0;
            session.Mp3File = fileInfo;

            
            
        }

        public Task<int> StartSession(DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
