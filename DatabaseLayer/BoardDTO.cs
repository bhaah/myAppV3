﻿using myFirstAppSol.DatabaseLayer;
using MySql.Data.MySqlClient;
namespace WebApplication2.DatabaseLayer
{
    public class BoardDTO : DTO
    {
        private int _id;
        private string _name;
        private string _email;

        private bool isPersisted;
        private BoardController bc=new BoardController();

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if(isPersisted)
                {
                    bc.updateName(Id, value);
                }
                _name = value;
            }
        }
        public string Email
        {
            get { return _email; }
            set { }
        }


        public BoardDTO(int id,string name,string email,bool toPersist)
        {
            _id = id;
            _name = name;
            _email = email;
            if(toPersist)
            {
                persist();
            }
            else
            {
                isPersisted = true;
            }
        }


        public void persist()
        {
            bc.addBoard(this);
            isPersisted= true;
        }

        public void delete()
        {
            bc.deleteBoad(Id);
            isPersisted= false;
        }


    }
}
