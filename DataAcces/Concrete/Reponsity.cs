using DataAccess.Abstract;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DataAcces.Concrete
{
    public class Reponsity : IRepository
    {
        public void createtodo(Test test)
        {
          using(var dbContext = new DBContext())
            {

                dbContext.Tessts.Add(test);
                dbContext.SaveChanges();
            }
        }

        public void deletetodo(int id)
        {
            using (var dbContext = new DBContext())
            {
                var deletetodo = getotodoid(id);
                dbContext.Tessts.Remove(deletetodo);
                dbContext.SaveChanges();
            }
        }

        public Test getotodoid(int id)
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.Tessts.Find(id);
            }
        }

        public List<Test> gettodoall()
        {
            using (var dbContext = new DBContext())
            {
                return dbContext.Tessts.ToList();
            }
        }

        public void listtodofalse(int id)
        {
            using (var dbContext = new DBContext())
            {
                Test t = dbContext.Tessts.Find(id);
                t.RowStatus = false;
                dbContext.SaveChanges();

            }
        }

        public void listtodotrue(int id)
        {
            using (var dbContext = new DBContext())
            {
                Test t = dbContext.Tessts.Find(id);
            t.RowStatus = true;
            dbContext.SaveChanges();
            }
        }

        public void updatetodo(Test test)
        {
            using (var dbContext = new DBContext())
            {
                 dbContext.Tessts.Update(test);
                dbContext.SaveChanges();
            }
        }
    }
}
