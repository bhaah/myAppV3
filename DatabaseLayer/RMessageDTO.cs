namespace myFirstAppSol.DatabaseLayer
{
    public class RMessageDTO
    {
        private string content;
        private int id;

        private RMessageController rmc =new RMessageController();
        public int Id { get { return id; } }
        public string Content { get { return content; } }

        public RMessageDTO(int id,string content,bool toPersist) {

            this.id = id;
            this.content = content;
            if(toPersist ) { rmc.Insert(this); }

        }

    }
}
