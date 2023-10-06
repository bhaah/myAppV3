namespace WebApplication2.DatabaseLayer
{
    public class NoteDTO
    {
        private string _note;
        private int _id;
        private int _boardId;
        private bool isPersisted;
        private NoteController nc = new NoteController();


        public string Note
        {
            get { return _note; }
            set {
                if(isPersisted) { nc.updateNote(Id,value)}
                _note = value; 
            }

        }
        public int Id
        {
            get { return _id; }
        }
        public int BoardId
        {
            get { return _boardId; }
        }



        public NoteDTO(string note, int id,bool toPersisit)
        {
            _note = note;
            _id = id;
            if (toPersisit) persist();
            else isPersisted= true;
         
        }

        public void persist() { }
    }
}
