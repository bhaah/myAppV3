namespace WebApplication2.DatabaseLayer
{
    public class CornerDTO
    {
        private int _boardId;
        private int _id;
        private string _name;
        private string _description;
        private int _progress;

        //for database
        private bool isPersisted;
        private CornerController cc=new CornerController();

        public string Name { get { return _name; } }
        public string Description { get { return _description; } }
        public int Id { get { return _id; } }
        public int BoardId { get { return _boardId; } }
        public int Progress { get { return _progress; } }

        public CornerDTO(int boardId, int id, string name, string description, int progress, bool toPersist)
        {
            _boardId = boardId;
            _id = id;
            _name = name;
            _description = description;
            _progress = progress;
            if (toPersist)
            {
                persist();
            }
        }


        public void persist()
        {
            cc.InsertCorner(this);
            isPersisted= true;
        }
    }
}
