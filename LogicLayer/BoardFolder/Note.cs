using WebApplication2.DatabaseLayer;

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class Note
    {
        private string _note;
        private int _id;
        private int _boardId;
        private NoteDTO ndto;
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
        public int BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }
        public Note(int id,string content, int boardId)
        {
            _id = id;
            _note = content;
            _boardId = boardId;
            ndto = new NoteDTO(content, id, boardId, true);
        }
        public Note(NoteDTO ndto)
        {
            _id = ndto.Id;
            _note = ndto.Note;
            _boardId = ndto.BoardId;
            this.ndto = new NoteDTO(ndto.Note, ndto.Id, ndto.BoardId, false);
        }
    }
}
