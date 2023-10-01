namespace WebApplication2.LogicLayer.BoardFolder
{
    public class Note
    {
        private string _note;
        private int _id;


        public string Content
        {
            get { return _note; }
            set { _note = value; }
        }
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Note(int id,string content)
        {
            _id= id;
            _note = content;

        }
    }
}
