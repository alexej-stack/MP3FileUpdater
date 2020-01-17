using MP3FileUpdater.DAL.SQL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mp3Updater.DAL.SQL
{
    public class SQLRepository : IRepository
    {
        private SQLDatabaseContext _sqlContext = new SQLDatabaseContext();


        

        public int EndSession(DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public Session GetSessionData(int id)
        {

            var session = _sqlContext.Session.Find(id);
            return session;
        }

        public void SetFileInfo(int idSession, Mp3FileInfo fileInfo)
        {

            //  var session = new Session() { Id = idSession, ReadedFilesCount = 0, ReamainingFilesCount = 0,  Mp3File = new Mp3FileInfo() {Id=idSession, Name = fileInfo.Name, Size = fileInfo.Size } };

            var session = _sqlContext.Session.Find(idSession);
            session.ReadedFilesCount = 0;
            session.ReamainingFilesCount = 0;
            session.Mp3File = new Mp3FileInfo() { Id = idSession, Name = fileInfo.Name, Size = fileInfo.Size };

           
        }

      

        public int StartSession(DateTime endTime)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository.EndSession(DateTime endTime)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository.StartSession(DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
