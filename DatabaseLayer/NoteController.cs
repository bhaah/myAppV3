namespace WebApplication2.DatabaseLayer
{
    public class NoteController
    {
        private const string TableName = "NoteTable" ;
        private const string ColId = "NoteId";
        private const string ColName = "name" ;
        private const string ColBoardId = "boardId" ;


        //for connection :
        private string path ;
        private string con ;


        //constructor 
        public NoteController()
        {
            path = DBF.path;
            con = DBF.con;
        }

        //insert and geters

        public void Insert(NoteDTO noteDTO)
        {
            int id = 



        }





    }
}
